using System.Diagnostics;

namespace CookedAssetSerializer;

public class Serializer<T> where T: Export
{
    protected JSONSettings Settings;

    public bool IsSkipped;
    public string SkippedCode = "";
    
    protected UAsset Asset;
    protected string AssetName;
    protected string AssetPath;
    protected string OutPath;

    private readonly JObject JsonOut = new JObject();
    protected readonly JObject AssetData = new JObject();
    
    protected T ClassExport;
    protected string ClassName;

    protected Dictionary<int, int> Dict = new();
    protected List<int> RefObjects = new();
    protected List<string> GeneratedVariables = new();
    protected readonly List<string> DisableGeneration = new();

    protected AssetInfo AssetInfo;

    protected bool SetupSerialization()
    {
        Dict = new Dictionary<int, int>();
        
        var fullAssetPath = Asset.FilePath;
        AssetName = Path.GetFileNameWithoutExtension(fullAssetPath);
        var directory = Path.GetDirectoryName(fullAssetPath);
        var relativeAssetPath = Path.GetRelativePath(Settings.ContentDir, directory);
        if (relativeAssetPath.StartsWith(".")) relativeAssetPath = "\\";
        AssetPath = Path.Join("\\Game", relativeAssetPath, AssetName).Replace("\\", "/");
        OutPath = Path.Join(Settings.JSONDir, AssetPath) + ".json";

        Directory.CreateDirectory(Path.GetDirectoryName(OutPath));
        if (!Settings.RefreshAssets && File.Exists(OutPath))
        {
            IsSkipped = true;
            return false;
        }

        Asset = new UAsset(fullAssetPath, Settings.GlobalUEVersion, false);

        /*Dict = */FixIndexes(Dict, Asset);

        return true;
    }
    
    protected bool SetupAssetInfo()
    {
        if (Asset.Exports[Asset.mainExport - 1] is RawExport)
        {
            Debug.WriteLine("Raw " + Asset.FilePath); 
            return false;
        }
        ClassExport = (T)Asset.Exports[Asset.mainExport - 1];
        if (ClassExport == null) return false;
        ClassName = ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName();

        AssetInfo.Asset = Asset;
        AssetInfo.Dict = Dict;
        AssetInfo.DisableGeneration = DisableGeneration;
        AssetInfo.GeneratedVariables = GeneratedVariables;
        AssetInfo.ImportVariables = new List<string>();

        return true;
    }
    
    protected void SerializeHeaders()
    {
        JsonOut.Add("AssetClass", ClassName);
        JsonOut.Add("AssetPackage", AssetPath);
        JsonOut.Add("AssetName", AssetName);

        AssetData.Add("SkipDependecies",
            Settings.CircularDependency.Contains(GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }

    protected void AssignAssetSerializedData()
    {
        JsonOut.Add("AssetSerializedData", AssetData);
    }

    protected void WriteJsonOut(JProperty objHierarchy)
    {
        JsonOut.Add(objHierarchy);
        File.WriteAllText(OutPath, JsonOut.ToString());
    }
}