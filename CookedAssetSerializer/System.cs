using System.Text;

using System.Diagnostics;
using Serilog;

namespace CookedAssetSerializer;

public class System
{
    private readonly Settings Settings;

    private int AssetTotal;
    private int AssetCount;

    public System(Settings settings)
    {
        Settings = settings;
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

    public void ScanAssetTypes(string typeToFind = "")
    {
        Dictionary<string, List<string>> types = new();
        List<string> allTypes = new();

        var files = Directory.GetFiles(Settings.ParseDir, "*.uasset", SearchOption.AllDirectories);

        AssetTotal = files.Length;
        AssetCount = 0;
        foreach (var file in files)
        {
            AssetCount++;
            
            if (CheckPNGAsset(file)) continue;

            var type = GetAssetType(file, Settings.GlobalUEVersion);
            var path = "/" + Path.GetRelativePath(Settings.ContentDir, file).Replace("\\", "/");

            PrintOutput(path, "Scan");
            
            if (types.ContainsKey(type)) types[type].Add(path);
            else types[type] = new List<string> { path };

            if (type == typeToFind) PrintOutput(type + ": " + path, "Scan");
        }
        PrintOutput("Find all files " + files.Length, "Scan");
        var jTypes = new JObject();
        foreach (var entry in types)
        {
            PrintOutput(entry.Key + " : " + entry.Value.Count, "Scan");
            jTypes.Add(entry.Key, JArray.FromObject(entry.Value));
            allTypes.Add("\"" + entry.Key + "\",");
        }
        allTypes.Sort();

        File.WriteAllText(Settings.InfoDir + "\\AssetTypes.json", jTypes.ToString());
        File.WriteAllText(Settings.InfoDir + "\\AllTypes.txt", string.Join("\n", allTypes));
    }
    
    public void GetCookedAssets(bool copy = true)
    {
        var files = Directory.GetFiles(Settings.ParseDir, "*.uasset", SearchOption.AllDirectories);

        AssetTotal = files.Length;
        AssetCount = 0;
        foreach (var file in files)
        {
            AssetCount++;
            
            if (CheckPNGAsset(file)) continue;
            
            var uexpFile = Path.ChangeExtension(file, "uexp");
            var ubulkFile = Path.ChangeExtension(file, "ubulk");
            var type = GetAssetType(file, Settings.GlobalUEVersion);
            
            if (!Settings.TypesToCopy.Contains(type))
            {
                PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                continue;
            }

            var relativePath = Path.GetRelativePath(Settings.ContentDir, file);
            var newPath = Path.Combine(Settings.CookedDir, relativePath);

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
    
    public void SerializeAssets()
    {
        var files = Directory.GetFiles(Settings.ParseDir, "*.uasset", SearchOption.AllDirectories);

        AssetTotal = files.Length;
        AssetCount = 0;
        foreach (var file in files)
        {
            AssetCount++;
            
            if (CheckPNGAsset(file)) continue;
            
            UAsset asset = new UAsset(file, Settings.GlobalUEVersion, true);
            
            if (Settings.SkipSerialization.Contains(asset.assetType))
            {
                PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                continue;
            }
            
            PrintOutput("Serializing " + file, "SerializeAssets");

            bool skip = false;
            string skipReason = "";
            if (asset.assetType != EAssetType.Uncategorized)
            {
                if (Settings.DummyAssets.Contains(asset.assetType))
                {
                    if (Settings.DummyWithProps) skip = CheckDeleteAsset(asset, new DummyWithProps(Settings, asset).IsSkipped);
                    else skip = CheckDeleteAsset(asset, new DummySerializer(Settings, asset).IsSkipped);
                }
                else
                {
                    switch (asset.assetType)
                    {
                        case EAssetType.Blueprint:
                        case EAssetType.WidgetBlueprint:
                        case EAssetType.AnimBlueprint:
                            skip = CheckDeleteAsset(asset, new BlueprintSerializer(Settings, asset, false).IsSkipped);
                            break;
                        case EAssetType.DataTable:
                            skip = CheckDeleteAsset(asset, new DataTableSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.StringTable:
                            skip = CheckDeleteAsset(asset, new StringTableSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.UserDefinedStruct:
                            skip = CheckDeleteAsset(asset, new UserDefinedStructSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.BlendSpaceBase:
                            skip = CheckDeleteAsset(asset, new BlendSpaceSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.AnimSequence:
                            skip = CheckDeleteAsset(asset, new AnimSequenceSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.AnimMontage:
                            skip = CheckDeleteAsset(asset, new DummyWithProps(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.CameraAnim:
                            skip = CheckDeleteAsset(asset, new DummySerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.LandscapeGrassType:
                            skip = CheckDeleteAsset(asset, new DummyWithProps(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.MediaPlayer:
                            skip = CheckDeleteAsset(asset, new DummyWithProps(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.MediaTexture:
                            skip = CheckDeleteAsset(asset, new DummySerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.SubsurfaceProfile:
                            skip = CheckDeleteAsset(asset, new SubsurfaceProfileSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.Skeleton:
                            skip = CheckDeleteAsset(asset, new SkeletonSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.MaterialParameterCollection:
                            skip = CheckDeleteAsset(asset, new MaterialParameterCollectionSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.PhycialMaterial:
                            skip = CheckDeleteAsset(asset, new PhysicalMaterialSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.Material:
                            skip = CheckDeleteAsset(asset, new MaterialSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.MaterialInstanceConstant:
                            skip = CheckDeleteAsset(asset, new MaterialInstanceConstantSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.UserDefinedEnum:
                            skip = CheckDeleteAsset(asset, new UserDefinedEnumSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.SoundCue:
                            skip = CheckDeleteAsset(asset, new SoundCueSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.Font:
                            skip = CheckDeleteAsset(asset, new FontSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.FontFace:
                            skip = CheckDeleteAsset(asset, new FontFaceSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.CurveBase:
                            skip = CheckDeleteAsset(asset, new CurveBaseSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.Texture2D:
                            skip = CheckDeleteAsset(asset, new Texture2DSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.SkeletalMesh:
                            skip = CheckDeleteAsset(asset, new SkeletalMeshSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.FileMediaSource:
                            skip = CheckDeleteAsset(asset, new FileMediaSourceSerializer(Settings, asset).IsSkipped);
                            break;
                        case EAssetType.StaticMesh:
                            var sm = new StaticMeshSerializer(Settings, asset);
                            if (sm.SkippedCode != "") skipReason = sm.SkippedCode;
                            skip = CheckDeleteAsset(asset, sm.IsSkipped);
                            break;
                    }
                }
            }
            else
            {
                if (asset.mainExport == 0) continue;
                if (!Settings.SimpleAssets.Contains(GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset))) continue;
                skip = CheckDeleteAsset(asset, new UncategorizedSerializer(Settings, asset).IsSkipped);
            }

            if (skip)
            {
                if (skipReason == "") PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                else PrintOutput("Skipped serialization on " + file + " due to: " + skipReason, "SerializeAssets");
            }
            else PrintOutput(file, "SerializeAssets");
        }
    }

    public bool CheckDeleteAsset(UAsset asset, bool isSkipped)
    {
        if (isSkipped && Settings.DeleteAssets.Contains(asset.assetType)) 
        {
            File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
        }
        return isSkipped;
    }

    private void PrintOutput(string output, string type = "debug")
    {
        //string logLine = $"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}";
        Log.ForContext("Context", type).Information($"{AssetCount}/{AssetTotal} {output}");
    }

    private bool CheckPNGAsset(string file)
    {
        // If there is another file with the same name but has the .png extension, skip it
        // TODO: Make JSON for the PNG just existing for Asset Gen (currently UAGUI does not support this uasset type)
        return File.Exists(file.Replace(".uasset", ".png"));
    }

    private string GetAssetType(string file, UE4Version version)
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
}