using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;

namespace CookedAssetSerializer.AssetTypes
{
    public class Serializer<T> where T: NormalExport
    {
        public UAsset Asset;
        public string AssetName;
        public string AssetPath;
        public string OutPath;

        public JObject JsonOut = new JObject();
        public JObject AssetData = new JObject();
        private T ClassExport;

        public Dictionary<int, int> Dict = new();
        public List<int> RefObjects = new();
        public List<string> GeneratedVariables = new();
        public List<string> DisableGeneration = new();

        public void SerializeAssets(UAsset asset, string assetName, string assetPath, string outPath, 
            List<string> disableGeneration)
        {
            Asset = asset;
            AssetName = assetName;
            AssetPath = assetPath;
            OutPath = outPath;
            DisableGeneration = disableGeneration;

            ClassExport = (T)Exports[Asset.mainExport - 1];
            if (ClassExport != null) Serialize();
        }

        private void SetupSerialization()
        {
            Dict = new Dictionary<int, int>();
            DisableGeneration = new List<string>();
            GeneratedVariables = new List<string>();
            RefObjects = new List<int>();

            var fullAssetPath = Asset.FilePath;
            AssetName = Path.GetFileNameWithoutExtension(fullAssetPath);
            var directory = Path.GetDirectoryName(fullAssetPath);
            var relativeAssetPath = Path.GetRelativePath(ContentDir, directory);
            if (relativeAssetPath.StartsWith(".")) relativeAssetPath = "\\";
            AssetPath = Path.Join("\\Game", relativeAssetPath, AssetName).Replace("\\", "/");
            OutPath = Path.Join(JSONDir, AssetPath) + ".json";

            Directory.CreateDirectory(Path.GetDirectoryName(OutPath));
            if (!RefreshAssets && File.Exists(OutPath)) return false;

            Asset = new UAsset(fullAssetPath, GlobalUEVersion, false);

            FixIndexes();

            return true;
        }

        private void Serialize()
        {
            // Add the required header data
            JsonOut.Add("AssetClass", ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName());
            JsonOut.Add("AssetPackage", AssetPath);
            JsonOut.Add("AssetName", AssetName);
            
            // Check for circular dependency
            if (CircularDependency.Contains(GetFullName(ClassExport.ClassIndex.Index))) {
                AssetData.Add("SkipDependecies", true);
            }
            
            AssetData.Add("AssetClass", GetFullName(ClassExport.ClassIndex.Index));
            JsonOut.Add("AssetObjectData", new JObject(new JProperty("$ReferencedObjects", new JArray())));
            
            // Add asset data to Json output
            JsonOut.Add("AssetSerializedData", AssetData);
            
            // Add object hierarchy data to Json output
            JsonOut.Add(new JProperty("ObjectHierarchy", new JArray()));
            
            // Write Json output to the output path
            File.WriteAllText(OutPath, JsonOut.ToString());
        }
    }
}