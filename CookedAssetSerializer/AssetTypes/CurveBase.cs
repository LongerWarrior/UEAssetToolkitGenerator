using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeCurveBase() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();
            var blendSpace = Exports[Asset.mainExport - 1] as CurveBaseExport;

            if (blendSpace == null) return;
            ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(Asset).ObjectName.ToName());
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject { { "AssetClass", GetFullName(blendSpace.ClassIndex.Index) } };
            var jData = SerializaListOfProperties(blendSpace.Data);
            jData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            asData.Add("AssetObjectData", jData);
            ja.Add("AssetSerializedData", asData);
            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
