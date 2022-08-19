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
        AssetCount = 1;
        foreach (var file in files)
        {
            var type = GetAssetType(file, Settings.GlobalUEVersion);
            var path = "/" + Path.GetRelativePath(Settings.ContentDir, file).Replace("\\", "/");

            PrintOutput(path, "Scan");
            AssetCount++;

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

        File.WriteAllText(Settings.JSONDir + "\\AssetTypes.json", jTypes.ToString());
        File.WriteAllText(Settings.JSONDir + "\\AllTypes.txt", string.Join("\n", allTypes));
    }
    
    public void GetCookedAssets(bool copy = true)
    {
        var files = Directory.GetFiles(Settings.ParseDir, "*.uasset", SearchOption.AllDirectories);

        AssetTotal = files.Length;
        AssetCount = 0;
        foreach (var file in files)
        {
            var uexpFile = Path.ChangeExtension(file, "uexp");
            var ubulkFile = Path.ChangeExtension(file, "ubulk");
            var type = GetAssetType(file, Settings.GlobalUEVersion);

            AssetCount++;
            if (!Settings.TypesToCopy.Contains(type))
            {
                PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                continue;
            }

            var relativePath = Path.GetRelativePath(Settings.ContentDir, file);
            var newPath = Path.Combine(Settings.OutputDir, relativePath);

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
            UAsset asset = new UAsset(file, Settings.GlobalUEVersion, true);
            AssetCount++;

            if (Settings.SkipSerialization.Contains(asset.assetType))
            {
                PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                continue;
            }

            bool skip = false;
            if (asset.assetType != EAssetType.Uncategorized)
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
                        // Temp - change false to true if you want this delete to run
                        skip = new DummySerializer(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.AnimMontage:
                        // Temp - change false to true if you want this delete to run
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.CameraAnim:
                        // Temp - change false to true if you want this delete to run
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.LandscapeGrassType:
                        // Temp - change false to true if you want this delete to run
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.MediaPlayer:
                        // Temp - change false to true if you want this delete to run
                        skip = new DummyWithProps(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.MediaTexture:
                        // Temp - change false to true if you want this delete to run
                        skip = new DummySerializer(Settings, asset).IsSkipped;
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
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
                        // Temp - change false to true if you want this delete to run
                        if (skip && false) File.Delete(Path.Join(Settings.JSONDir, Path.Join("\\Game", 
                            Path.GetRelativePath(Settings.ContentDir, Path.GetDirectoryName(asset.FilePath)), 
                            Path.GetFileNameWithoutExtension(asset.FilePath)).Replace("\\", "/")) + ".json");
                        break;
                    case EAssetType.FileMediaSource:
                        skip = new FileMediaSourceSerializer(Settings, asset).IsSkipped;
                        break;
                    case EAssetType.StaticMesh:
                        skip = new StaticMeshSerializer(Settings, asset).IsSkipped;
                        break;
                }
            }
            else
            {
                if (asset.mainExport == 0) continue;
                if (!Settings.SimpleAssets.Contains(GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset))) continue;
                skip = new UncategorizedSerializer(Settings, asset).IsSkipped;
            }
            
            if (skip) PrintOutput("Skipped serialization on " + file, "SerializeAssets");
            else PrintOutput(file, "SerializeAssets");
        }
    }

    private void PrintOutput(string output, string type = "debug")
    {
        Console.WriteLine(output);
        var filename = type == "debug" ? "debug" : "output";
        using var sw = File.AppendText(Path.Combine(Settings.JSONDir, filename + "_log.txt"));
        sw.WriteLine($"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}");
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

        Console.WriteLine("Couldn't identify asset type : " + file);
        return "null";
    }
}