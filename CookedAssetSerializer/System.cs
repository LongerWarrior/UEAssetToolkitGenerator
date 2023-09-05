using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UAssetAPI.AssetRegistry;

namespace CookedAssetSerializer;

public class System
{
    private readonly JSONSettings Settings;
    private Dictionary<string, AssetData> AssetList;

    private int AssetTotal;
    private int AssetCount;

    public System(JSONSettings jsonsettings)
    {
        Settings = jsonsettings;

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(Settings.InfoDir + "/output_log.txt", 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Message}{NewLine}{Exception}")
            .CreateLogger();
    }

    public int GetAssetTotal()
    {
        return AssetTotal;
    }

    public int GetAssetCount()
    {
        return AssetCount;
    }

    public void ClearLists()
    {
        Settings.CircularDependency.Clear();
        Settings.SimpleAssets.Clear();
        Settings.TypesToCopy.Clear();
    }

    private void ScanAR()
    {
        var AR = new FAssetRegistryState(Settings.AssetRegistry, Settings.GlobalUEVersion);
        AssetList = new Dictionary<string, AssetData>(AR.PreallocatedAssetDataBuffers.Length);
        foreach (var data in AR.PreallocatedAssetDataBuffers) 
        {
            if (data.PackageName.ToName().StartsWith("/Game")) 
            {
                AssetList[data.PackageName.ToName()] = new AssetData(data.AssetClass, data.AssetName, data.TagsAndValues);
            }
        }
        AR = null;
        GC.Collect();
    }

    public void ScanAssetTypes(string typeToFind = "")
    {
        ScanAR();
        
        ConcurrentDictionary<string, ConcurrentBag<string>> types = new();
        List<string> allTypes = new();
        var files = MakeFileList(Settings.ContentDir);
        AssetCount = 0;
        AssetTotal = files.Count;
        
        Parallel.ForEach(files, file =>
        {
            Interlocked.Increment(ref AssetCount);
            
            if (CheckPNGAsset(file)) return;

            // Cannot use AR types because it (most of the time) does not include the /Script/<type>. data which is required
            var /*type = GetAssetTypeAR(file);
            if (type == "null")*/ type = GetAssetType(file, Settings.GlobalUEVersion);

            var path = "/" + Path.GetRelativePath(Settings.ContentDir, file).Replace("\\", "/");

            PrintOutput("/Game" + path, "Scan");
            
            types.AddOrUpdate(
                type, 
                _ => new ConcurrentBag<string> { path }, 
                (_, paths) => { paths.Add(path); return paths; }
            );

            if (type == typeToFind) PrintOutput(type + ": " + path, "Scan");
        });

        Log.Information($"[Scan]: Found {files.Count} files");
        var jTypes = new JObject();
        foreach (var entry in types)
        {
            Log.Information($"[Scan]: {entry.Key} : {entry.Value.Count}");
        jTypes.Add(entry.Key, JArray.FromObject(entry.Value.ToList()));
            allTypes.Add("\"" + entry.Key + "\",");
        }
        allTypes.Sort();

        File.WriteAllText(Settings.InfoDir + "\\AssetTypes.json", jTypes.ToString());
        File.WriteAllText(Settings.InfoDir + "\\AllTypes.txt", string.Join("\n", allTypes));
    }

    public void SerializeNativeAssets()
    {
        ScanAR();
        
        ENativizationMethod method = ENativizationMethod.Disabled;
        var nativeAssets = new List<string>();
        foreach (var line in File.ReadAllLines(Settings.DefaultGameConfig))
        {
            if (line.StartsWith("BlueprintNativizationMethod="))
            {
                method = (ENativizationMethod)Enum.Parse(typeof(ENativizationMethod), line.Split('=')[1]);
            }
            
            if (line.StartsWith("+NativizeBlueprintAssets="))
            {
                var path = line.Split('"')[1];
                path.Remove(path.Length - 1, 1);
                nativeAssets.Add(path);
            }
        }
        
        AssetCount = 0;
        AssetTotal = nativeAssets.Count;
        if (nativeAssets.Count > 0)
        {
            // We cannot only filter by parse dir because the nativized assets don't show up in the tree view,
            // so they would be skipped regardless
            foreach (var asset in nativeAssets)
            {
                AssetCount++;
                foreach (var ARAsset in AssetList)
                {
                    if (ARAsset.Key == asset)
                    {
                        // Need to use asset's value instead of ARAsset due to ARAsset's value name having the _C
                        new RawDummy(Settings, ARAsset, asset);
                        PrintOutput("Creating dummy blueprint for nativized asset: " + ARAsset.Key, "Native Asset Serializer");
                    }
                }
            }
        }
        
        if (method != ENativizationMethod.Disabled) // Inclusive OR exclusive
        {
            foreach (var ARAsset in AssetList)
            {
                if (!Settings.SkipSerialization.Contains(EAssetType.UserDefinedEnum)
                    && ARAsset.Value.AssetClass == "UserDefinedEnum")
                {
                    AssetTotal++;
                    AssetCount++;
                    new RawDummy(Settings, ARAsset, ARAsset.Value.AssetName);
                    PrintOutput("Creating dummy enum for nativized asset: " + ARAsset.Key, "Native Asset Serializer");
                }

                // Sadly we cannot dummy structs without property data because it will cause issues when being referenced,
                // and even parsing the C++ header dump does not provide enough information (need actual values)
                /*if (!Settings.SkipSerialization.Contains(EAssetType.UserDefinedStruct) 
                    && ARAsset.Value.AssetClass == "UserDefinedStruct") new RawDummy(Settings, ARAsset);*/
            }
        }
    }
    
    public void GetCookedAssets(bool copy)
    {
        ScanAR();

        var nameType = copy ? "Copy" : "Move"; // For logging

        // Get short version of types so that AR list can be used for efficiency
        List<string> shortTypes = new List<string>();
        foreach (var assetType in Settings.TypesToCopy)
        {
            shortTypes.Add(assetType.Split(".")[1]);
        }
        
        var files = MakeFileList(Settings.FromDir, false);
        AssetCount = 0;
        AssetTotal = files.Count;
        foreach (var file in files)
        {
            AssetCount++;
            
            if (CheckPNGAsset(file)) continue;

            var type = GetAssetTypeAR(file, Settings.FromDir);
            if (type == "null") type = GetAssetType(file, Settings.GlobalUEVersion);

            if (Settings.TypesToCopy.Contains(type) || shortTypes.Contains(type) || Settings.CopyAllTypes)
            {
                var relativePath = Path.GetRelativePath(Settings.FromDir, file);
                var newPath = Path.Combine(Settings.CookedDir, relativePath);
    
                PrintOutput(newPath, $"{nameType} Assets");
    
                Directory.CreateDirectory(Path.GetDirectoryName(newPath) ?? string.Empty);
            
                if (copy) File.Copy(file, newPath, true);
                else File.Move(file, newPath, true);
    
                var uexpFile = Path.ChangeExtension(file, "uexp");
                if (File.Exists(uexpFile))
                {
                    if (copy) File.Copy(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                    else File.Move(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                }
            
                var ubulkFile = Path.ChangeExtension(file, "ubulk");
                if (File.Exists(ubulkFile))
                {
                    if (copy) File.Copy(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
                    else File.Move(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
                }
            }
            else
            {
                PrintOutput("Skipped operation on " + file, $"{nameType} Assets");
                continue;
            }
        }

        // Delete any empty leftover directories
        if (!copy)
        {
            DeleteEmptyDirectories(Settings.FromDir);
            PrintOutput("Deleted empty directories!", $"{nameType} Assets");
        }
    }
    
    public void SerializeAssets()
    {
        var files = MakeFileList(Settings.ContentDir);
        AssetTotal = files.Count;
        AssetCount = 0;

        if (Settings.ConcurrentSerialization) Parallel.ForEach(files, SerializeAsset);
        else files.ForEach(SerializeAsset);
    }

    private void SerializeAsset(string file)
    {
        Interlocked.Increment(ref AssetCount);
        PrintOutput("Serializing " + file, "Serialize Assets");

        UAsset asset = new UAsset(file, Settings.GlobalUEVersion, true);

        if (Settings.SkipSerialization.Contains(asset.assetType) || CheckPNGAsset(file))
        {
            PrintOutput("Skipped serialization on " + file, "Serialize Assets");
            return;
        }

        bool skip = false;
        string skipReason = "";
        if (asset.assetType != EAssetType.Uncategorized)
        {
            if (Settings.DummyAssets.Contains(asset.assetType))
            {
                if (Settings.DummyWithProps) skip = new DummyWithProps(Settings, asset).IsSkipped;
                else skip = new DummySerializer(Settings, asset).IsSkipped;
            }
            else
            {
                switch (asset.assetType)
                {
                    case EAssetType.Blueprint:
                    case EAssetType.WidgetBlueprint:
                    case EAssetType.AnimBlueprint:
                        skip = new BlueprintSerializer(Settings, asset, false).IsSkipped;
                        break;
                    case EAssetType.DataTable:
                        skip = new DataTableSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.StringTable:
                        skip = new StringTableSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.UserDefinedStruct:
                        skip = new UserDefinedStructSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.BlendSpaceBase:
                        skip = new BlendSpaceSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.AnimSequence:
                        skip = new AnimSequenceSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.AnimMontage:
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.CameraAnim:
                        skip = new DummySerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.LandscapeGrassType:
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.MediaPlayer:
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.MediaTexture:
                        skip = new DummySerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.SubsurfaceProfile:
                        skip = new SubsurfaceProfileSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.Skeleton:
                        skip = new SkeletonSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.MaterialParameterCollection:
                        skip = new MaterialParameterCollectionSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.PhycialMaterial:
                        skip = new PhysicalMaterialSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.Material:
                        skip = new MaterialSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.MaterialInstanceConstant:
                        skip = new MaterialInstanceConstantSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.UserDefinedEnum:
                        skip = new UserDefinedEnumSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.SoundCue:
                        skip = new SoundCueSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.Font:
                        skip = new FontSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.FontFace:
                        skip = new FontFaceSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.CurveBase:
                        skip = new CurveBaseSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.Texture2D:
                        skip = new Texture2DSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.SkeletalMesh:
                        skip = new SkeletalMeshSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.FileMediaSource:
                        skip = new FileMediaSourceSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.StaticMesh:
                        var sm = new StaticMeshSerializer(Settings, asset);
                        if (sm.SkippedCode != "") skipReason = sm.SkippedCode;
                        skip = sm.IsSkipped;
                        break;
                }
            }
        }
        else
        {
            if (asset.mainExport == 0) return;
            if (!Settings.SimpleAssets.Contains(GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset)))
                return;
            skip = new UncategorizedSerializer(Settings, asset).IsSkipped;
        }

        if (skip)
        {
            if (skipReason == "") PrintOutput("Skipped serialization on " + file, "Serialize Assets");
            else PrintOutput("Skipped serialization on " + file + " due to: " + skipReason, "Serialize Assets");
        }
    }

    private List<string> MakeFileList(string fromDir, bool useParseDir = true)
    {
        List<string> ret = new List<string>();
        if (useParseDir)
        {
            if (Settings.ParseDir.Count == 1 && Settings.ParseDir[0].Equals("."))
            {
                ret.AddRange(Directory.GetFiles(fromDir,
                    "*.uasset", SearchOption.AllDirectories));
            }
            else
            {
                foreach (var dir in Settings.ParseDir)
                {
                    if (dir.EndsWith("uasset"))
                    {
                        ret.Add(Path.Combine(fromDir, dir));
                    }
                    else
                    {
                        ret.AddRange(Directory.GetFiles(Path.Combine(fromDir, dir),
                            "*.uasset", SearchOption.AllDirectories));
                    }
                }
            }
        }
        else
        {
            ret.AddRange(Directory.GetFiles(fromDir, "*.uasset", SearchOption.AllDirectories));
        }

        return ret;
    }

    private void PrintOutput(string output, string type = "debug", bool warning = false)
    {
        if (warning) Log.Warning($"[{type}]: {AssetCount}/{AssetTotal} {output}");
        else Log.Information($"[{type}]: {AssetCount}/{AssetTotal} {output}");
    }

    private bool CheckPNGAsset(string file)
    {
        // If there is another file with the same name but has the .png extension, skip it
        return File.Exists(file.Replace(".uasset", ".png"));
    }

    private string GetAssetType(string file, UE4Version version)
    {
        var name = Path.GetFileNameWithoutExtension(file).ToLower();
        UAsset asset = new UAsset(file, version, true);
        if (asset.Exports.Count == 1)
        {
            return GetFullName(asset.Exports[0].ClassIndex.Index, asset);
        }

        List<Export> exportnames = new();
        List<Export> isasset = new();
        foreach (var exp in asset.Exports)
        {
            if (exp.ObjectName.ToName().ToLower() == name + "_c") exportnames.Add(exp);
            if (exp.bIsAsset) isasset.Add(exp);
        }

        if (exportnames.Count == 0) 
        {
            foreach (var exp in asset.Exports) 
            {
                if (exp.ObjectName.ToName().ToLower() == name) exportnames.Add(exp);
            }
        }

        if (exportnames.Count == 1)
        {
            return GetFullName(exportnames[0].ClassIndex.Index, asset);
        }

        if (isasset.Count == 1)
        {
            return GetFullName(isasset[0].ClassIndex.Index, asset);
        }
        PrintOutput($"Couldn't identify asset type: {file}", "Get Asset Type", true);
        return "null";
    }

    private string GetAssetTypeAR(string fullAssetPath, string relativeToDir) {
        if (AssetList.Count == 0) return "null";

        var AssetPath = GetAssetPackageFromFullPath(fullAssetPath, relativeToDir);
        if (AssetList.ContainsKey(AssetPath)) 
        {
            var artype = AssetList[AssetPath].AssetClass;
            return artype;
        }
        PrintOutput($"Couldn't identify asset type with AR: {fullAssetPath}", "Get Asset Type", true);
        return "null";
    }

    private string GetAssetPackageFromFullPath(string fullAssetPath, string relativeToDir)
    {
        var AssetName = Path.GetFileNameWithoutExtension(fullAssetPath);
        var directory = Path.GetDirectoryName(fullAssetPath);
        var relativeAssetPath = Path.GetRelativePath(relativeToDir, directory);
        if (relativeAssetPath.StartsWith(".")) relativeAssetPath = "\\";
        return Path.Join("\\Game", relativeAssetPath, AssetName).Replace("\\", "/");
    }

    private void DeleteEmptyDirectories(string location)
    {
        foreach (var dir in Directory.GetDirectories(location))
        {
            DeleteEmptyDirectories(dir);
            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
            {
                Directory.Delete(dir, false);
            }
        }
    }
}
