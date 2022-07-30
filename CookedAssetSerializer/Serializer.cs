using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer
{
    public class Serializer<T> where T: Export
    {
        protected Settings Settings;
        
        protected UAsset Asset;
        private string AssetName;
        private string AssetPath;
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
            if (!Settings.RefreshAssets && File.Exists(OutPath)) return false;

            Asset = new UAsset(fullAssetPath, Settings.GlobalUEVersion, false);

            FixIndexes(Dict, Asset);

            return true;
        }
        
        protected bool SetupAssetInfo()
        {
            ClassExport = (T)Asset.Exports[Asset.mainExport - 1];
            if (ClassExport == null) return false;
            ClassName = ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName();

            AssetInfo.Asset = Asset;
            AssetInfo.Dict = Dict;
            AssetInfo.DisableGeneration = DisableGeneration;
            AssetInfo.GeneratedVariables = GeneratedVariables;

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
}