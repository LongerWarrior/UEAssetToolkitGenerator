using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.Globals;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeSimpleAsset(bool isSimple = true, bool skipDependencies = false) {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not NormalExport simple) return;
            ja.Add("AssetClass", isSimple ? "SimpleAsset" : simple.ClassIndex.ToImport(Asset).ObjectName.ToName());
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject();
            if (isSimple) {
                asData.Add("AssetClass", GetFullName(simple.ClassIndex.Index));
                if (CIRCULAR_DEPENDENCY.Contains(GetFullName(simple.ClassIndex.Index))) skipDependencies = true;
                asData.Add("SkipDependencies", skipDependencies);
            }

            var jData = SerializaListOfProperties(simple.Data);

            if (GetFullName(simple.ClassIndex.Index) == "/Script/Paper2D.PaperSprite") {
                if (UAssetAPI.Kismet.KismetSerializer.FindPropertyData(simple, "BakedSourceTexture", out var _source)) {
                    var source = (ObjectPropertyData)_source;
                    jData.Add(new JProperty("SourceTexture", GetFullName(source.Value.Index)));
                }

                for (var i = 0; i < jData.Properties().Count(); i++) {
                    var jProp = jData.Properties().ElementAt(i);
                    switch (jProp.Name) {
                        case "BakedSourceDimension":
                            jData.Add("SourceDimension", jProp.Value);
                            break;
                        case "BakedRenderData":
                            jData.Add("RenderData", jProp.Value);
                            break;
                    }
                }
            }

            jData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));

            asData.Add("AssetObjectData", jData);
            ja.Add("AssetSerializedData", asData);
            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
