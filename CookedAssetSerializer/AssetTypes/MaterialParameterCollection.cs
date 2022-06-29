using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeMaterialParameterCollection() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();
            var material = Exports[Asset.mainExport - 1] as NormalExport;

            if (material == null) return;
            ja.Add("AssetClass", "MaterialParameterCollection");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject();
            var aoData = SerializaListOfProperties(material.Data);
            aoData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            RefObjects = new List<int>();
            asData.Add("AssetObjectData", aoData);
            ja.Add("AssetSerializedData", asData);

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
