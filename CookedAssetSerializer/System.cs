using System.Diagnostics;
using Serilog;

namespace CookedAssetSerializer;

public class System
{
    private readonly JSONSettings JSONSettings;

    private int AssetTotal;
    private int AssetCount;
    public List<string> Files = new();

    public System(JSONSettings jsonsettings)
    {
        JSONSettings = jsonsettings;
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
        JSONSettings.CircularDependency.Clear();
        JSONSettings.SimpleAssets.Clear();
        JSONSettings.TypesToCopy.Clear();
    }

    public void ScanAssetTypes(string typeToFind = "")
    {
        Dictionary<string, List<string>> types = new();
        List<string> allTypes = new();
        var files = MakeFileList();
        foreach (var dir in files)
        {
            foreach (var file in dir)
            {
                var type = GetAssetTypeAR(file);
                if (type == "null") type = GetAssetType(file, JSONSettings.GlobalUEVersion);

                var path = "/" + Path.GetRelativePath(JSONSettings.ContentDir, file).Replace("\\", "/");

                PrintOutput(path, "Scan");
                AssetCount++;

                if (types.ContainsKey(type)) types[type].Add(path);
                else types[type] = new List<string> { path };

                if (type == typeToFind) PrintOutput(type + ": " + path, "Scan");
            }
        }
        PrintOutput("Find all files " + files.Count, "Scan");
        var jTypes = new JObject();
        foreach (var entry in types)
        {
            PrintOutput(entry.Key + " : " + entry.Value.Count, "Scan");
            jTypes.Add(entry.Key, JArray.FromObject(entry.Value));
            allTypes.Add("\"" + entry.Key + "\",");
        }

        File.WriteAllText(JSONSettings.InfoDir + "\\AssetTypes.json", jTypes.ToString());
        File.WriteAllText(JSONSettings.InfoDir + "\\AllTypes.txt", string.Join("\n", allTypes));
    }

    public void ScanNativeAssets()
    {
        ENativizationMethod nativizationMethod = ENativizationMethod.Disabled;
        var nativeAssets = new List<string>();
        foreach (var line in File.ReadAllLines(JSONSettings.DfltGamCfgDir))
        {
            if (line.StartsWith("BlueprintNativizationMethod="))
            {
                nativizationMethod = (ENativizationMethod)Enum.Parse(typeof(ENativizationMethod), line.Split('=')[1]);
            }
            
            if (line.StartsWith("+NativizeBlueprintAssets="))
            {
                var path = line.Split('"')[1];
                path.Remove(path.Length - 1, 1);
                nativeAssets.Add(path);
            }
        }

        if (nativeAssets.Count > 0)
        {
            // Check AR scan for assets' parents
        }

        if (!nativizationMethod.Equals(0))
        {
            // Scan AR for enums and structs

            // Dummy each asset in this format
            JObject data = new JObject
            {
                { "AssetClass", "Dummy" },
                { "AssetPackage", "Dummy" },
                { "AssetName", "Dummy" },
                { "AssetSerializedData", new JObject("SkipDependecies", false) },
                { "ObjectHierarchy", new JArray() }
            };
            File.WriteAllText("path", data.ToString());
        }
    }
    
    public void GetCookedAssets(bool copy = true)
    {
        var files = MakeFileList();
        foreach (var dir in files)
        {
            foreach (var file in dir)
            {
                var uexpFile = Path.ChangeExtension(file, "uexp");
                var ubulkFile = Path.ChangeExtension(file, "ubulk");
                var type = GetAssetTypeAR(file);
                if (type == "null") {
                    Debug.WriteLine(file);
                    type = GetAssetType(file, JSONSettings.GlobalUEVersion);
                }
    
                AssetCount++;
                if (!JSONSettings.TypesToCopy.Contains(type))
                {
                    PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                    continue;
                }
    
                var relativePath = Path.GetRelativePath(JSONSettings.ContentDir, file);
                var newPath = Path.Combine(JSONSettings.CookedDir, relativePath);
    
                PrintOutput(newPath, "GetCookedAssets");
    
                Directory.CreateDirectory(Path.GetDirectoryName(newPath) ?? string.Empty);
                if (copy) File.Copy(file, newPath, true);
                else File.Move(file, newPath, true);
    
                if (File.Exists(uexpFile))
                {
                    if (copy) File.Copy(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                    else File.Move(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                }
    
                if (!File.Exists(ubulkFile)) continue;
                if (copy) File.Copy(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
                else File.Move(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
            }
            
        }
    }
    
    public void SerializeAssets()
    {
        var files = MakeFileList();
        foreach (var dir in files)
        {
            foreach (var file in dir)
            {
                UAsset asset = new UAsset(file, JSONSettings.GlobalUEVersion, true);
            AssetCount++;

            if (JSONSettings.SkipSerialization.Contains(asset.assetType))
            {
                PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                continue;
            }

            bool skip = false;
            if (asset.assetType != EAssetType.Uncategorized)
            {
                if (JSONSettings.DummyAssets.Contains(asset.assetType))
                {
                    if (JSONSettings.DummyWithProps) skip = CheckDeleteAsset(asset, new DummyWithProps(JSONSettings, asset).IsSkipped);
                    else skip = CheckDeleteAsset(asset, new DummySerializer(JSONSettings, asset).IsSkipped);
                }
                else
                {
                    switch (asset.assetType)
                    {
                        case EAssetType.Blueprint:
                        case EAssetType.WidgetBlueprint:
                        case EAssetType.AnimBlueprint:
                            skip = CheckDeleteAsset(asset, new BlueprintSerializer(JSONSettings, asset, false).IsSkipped);
                            break;
                        case EAssetType.DataTable:
                            skip = CheckDeleteAsset(asset, new DataTableSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.StringTable:
                            skip = CheckDeleteAsset(asset, new StringTableSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.UserDefinedStruct:
                            skip = CheckDeleteAsset(asset, new UserDefinedStructSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.BlendSpaceBase:
                            skip = CheckDeleteAsset(asset, new BlendSpaceSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.AnimSequence:
                            skip = CheckDeleteAsset(asset, new DummySerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.AnimMontage:
                            skip = CheckDeleteAsset(asset, new DummySerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.CameraAnim:
                            skip = CheckDeleteAsset(asset, new DummySerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.LandscapeGrassType:
                            skip = CheckDeleteAsset(asset, new DummyWithProps(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.MediaPlayer:
                            skip = CheckDeleteAsset(asset, new DummyWithProps(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.MediaTexture:
                            skip = CheckDeleteAsset(asset, new DummySerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.SubsurfaceProfile:
                            skip = CheckDeleteAsset(asset, new SubsurfaceProfileSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.Skeleton:
                            skip = CheckDeleteAsset(asset, new SkeletonSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.MaterialParameterCollection:
                            skip = CheckDeleteAsset(asset, new MaterialParameterCollectionSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.PhycialMaterial:
                            skip = CheckDeleteAsset(asset, new PhysicalMaterialSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.Material:
                            skip = CheckDeleteAsset(asset, new MaterialSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.MaterialInstanceConstant:
                            skip = CheckDeleteAsset(asset, new MaterialInstanceConstantSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.UserDefinedEnum:
                            skip = CheckDeleteAsset(asset, new UserDefinedEnumSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.SoundCue:
                            skip = CheckDeleteAsset(asset, new SoundCueSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.Font:
                            skip = CheckDeleteAsset(asset, new FontSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.FontFace:
                            skip = CheckDeleteAsset(asset, new FontFaceSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.CurveBase:
                            skip = CheckDeleteAsset(asset, new CurveBaseSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.Texture2D:
                            skip = CheckDeleteAsset(asset, new Texture2DSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.SkeletalMesh:
                            skip = CheckDeleteAsset(asset, new SkeletalMeshSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.FileMediaSource:
                            skip = CheckDeleteAsset(asset, new FileMediaSourceSerializer(JSONSettings, asset).IsSkipped);
                            break;
                        case EAssetType.StaticMesh:
                            skip = CheckDeleteAsset(asset, new StaticMeshSerializer(JSONSettings, asset).IsSkipped);
                            break;
                    }
                }
            }
            else
            {
                if (asset.mainExport == 0) continue;
                if (!JSONSettings.SimpleAssets.Contains(GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset))) continue;
                skip = CheckDeleteAsset(asset, new UncategorizedSerializer(JSONSettings, asset).IsSkipped);
            }
            
            if (skip) PrintOutput("Skipped serialization on " + file, "SerializeAssets");
            else PrintOutput(file, "SerializeAssets");
            }
        }
    }

    private List<string[]> MakeFileList()
    {
        List<string[]> ret = new List<string[]>();
        AssetTotal = 0;
        AssetCount = 1;
        foreach (var dir in JSONSettings.ParseDir)
        {
            var files = Directory.GetFiles(dir, "*.uasset", SearchOption.TopDirectoryOnly);
            ret.Add(files);
            AssetTotal += files.Length;
        }

        return ret;
    }

    private bool CheckDeleteAsset(UAsset asset, bool isSkipped)
    {
        if (isSkipped && JSONSettings.DeleteAssets.Contains(asset.assetType)) 
        {
            File.Delete(Path.Join(JSONSettings.JSONDir, Path.Join("\\Game", 
            Path.GetRelativePath(JSONSettings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
        }
        return isSkipped;
    }

    private void PrintOutput(string output, string type = "debug")
    {
        Log.ForContext("Context", type).Information($"{AssetCount}/{AssetTotal} {output}");
    }

    private string GetAssetType(string file, UE4Version version, bool shortname = true)
    {
        var name = Path.GetFileNameWithoutExtension(file).ToLower();
        UAsset asset = new UAsset(file, version, true);
        if (asset.Exports.Count == 1)
        {
            return shortname ? GetName(asset.Exports[0].ClassIndex.Index, asset) : GetFullName(asset.Exports[0].ClassIndex.Index, asset);
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
            return shortname ? GetName(exportnames[0].ClassIndex.Index, asset): GetFullName(exportnames[0].ClassIndex.Index, asset);
        }

        if (isasset.Count == 1)
        {
            return shortname ? GetName(isasset[0].ClassIndex.Index, asset) : GetFullName(isasset[0].ClassIndex.Index, asset);
        }
        Log.ForContext("Context", "AssetType").Warning("Couldn't identify asset type : " + file);
        return "null";
    }

    private string GetAssetTypeAR(string fullAssetPath) {
        if (AssetList.Count == 0) return "null";

        var AssetName = Path.GetFileNameWithoutExtension(fullAssetPath);
        var directory = Path.GetDirectoryName(fullAssetPath);
        var relativeAssetPath = Path.GetRelativePath(JSONSettings.ContentDir, directory);
        if (relativeAssetPath.StartsWith(".")) relativeAssetPath = "\\";
        var AssetPath = Path.Join("\\Game", relativeAssetPath, AssetName).Replace("\\", "/").ToLower();

        if (AssetList.ContainsKey(AssetPath)) 
        {
            var artype = AssetList[AssetPath].AssetClass;
            return artype;
        }
        Log.ForContext("Context", "AssetType").Warning("Couldn't identify asset type with AR: " + fullAssetPath);
        return "null";
    }
}