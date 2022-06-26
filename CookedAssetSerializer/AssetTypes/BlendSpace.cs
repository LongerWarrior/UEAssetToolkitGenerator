using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeBlendSpace() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;

            var ja = new JObject();
            if (Exports[Asset.mainExport - 1] is not BlendSpaceBaseExport blendSpace) return;
            ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(Asset).ObjectName.ToName());
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            PopulateBlendParameters(ref blendSpace.Data);

            var jData = SerializaListOfProperties(blendSpace.Data);
            asData.Add("AssetClass", GetFullName(blendSpace.ClassIndex.Index));

            jData.Add("SkeletonGuid", GuidToJson(blendSpace.SkeletonGuid));
            jData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

            asData.Add("AssetObjectData", jData);
            ja.Add("AssetSerializedData", asData);
            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }

        private static void PopulateBlendParameters(ref List<PropertyData> data, int v = 3) {
            var fullentries = Enumerable.Range(0, v).ToList();
            var entries = new List<int>();
            for (var i = 0; i < data.Count; i++)
                if (data[i].Name.ToName() == "BlendParameters")
                    entries.Add(data[i].DuplicationIndex);

            if (entries.Count > 0) {
                var missing = fullentries.Except(entries).ToList();
                foreach (var missed in missing)
                    data.Add(new StructPropertyData(new FName("BlendParameters"), new FName("BlendParameter"), missed) {
                        Value = new List<PropertyData> {
                            new StrPropertyData(new FName("DisplayName")) { Value = new FString("None") },
                            new FloatPropertyData(new FName("Max")) { Value = 100.0f }
                        }
                    });
                data.Sort((x, y) => {
                    var ret = x.Name.ToName().CompareTo(y.Name.ToName());
                    if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                    return ret;
                });
            }
        }
    }
}
