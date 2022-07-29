using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class Serializer<T> where T: NormalExport
    {
        protected Settings Settings;
        
        protected UAsset Asset;
        protected string AssetName;
        protected string AssetPath;
        private string OutPath;

        protected readonly JObject JsonOut = new JObject();
        protected JObject AssetData = new JObject();
        
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

            Directory.CreateDirectory(Path.GetDirectoryName(OutPath) ?? string.Empty);
            if (!Settings.RefreshAssets && File.Exists(OutPath)) return false;

            Asset = new UAsset(fullAssetPath, Settings.GlobalUEVersion, false);

            FixIndexes(Dict, Asset);

            return true;
        }
        
        protected void SetupAssetInfo()
        {
            ClassExport = (T)Asset.Exports[Asset.mainExport - 1];
            if (ClassExport == null) return;
            ClassName = ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName();

            AssetInfo.Asset = Asset;
            AssetInfo.Dict = Dict;
            AssetInfo.DisableGeneration = DisableGeneration;
            AssetInfo.GeneratedVariables = GeneratedVariables;
        }
        
        protected void SerializeHeaders(JObject extra = null)
        {
            // Add the required header data
            JsonOut.Add("AssetClass", ClassName);
            JsonOut.Add("AssetPackage", AssetPath);
            JsonOut.Add("AssetName", AssetName);

            if (extra != null)
            {
                JsonOut.Add(extra);
            }

            if (Settings.CircularDependency.Contains(GetFullName(ClassExport.ClassIndex.Index, Asset)))
            {
                AssetData.Add("SkipDependecies", true);   
            }
        }

        protected void AssignAssetSerializedData()
        {
            JsonOut.Add("AssetSerializedData", AssetData);
        }

        protected void WriteJSONOut(JProperty objHierarchy)
        {
            JsonOut.Add(objHierarchy);
            File.WriteAllText(OutPath, JsonOut.ToString());
        }
    }
}