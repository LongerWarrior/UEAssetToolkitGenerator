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
            if (!SetupSerialization(out var name, out var gamepath, out var path1)) return;

            var ja = new JObject();
            var blendSpace = Exports[Asset.mainExport - 1] as BlendSpaceBaseExport;

            if (blendSpace != null) {
                ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(Asset).ObjectName.ToName());
                ja.Add("AssetPackage", gamepath);
                ja.Add("AssetName", name);
                var asdata = new JObject();


                PopulateBlendParameters(ref blendSpace.Data);

                var jdata = SerializaListOfProperties(blendSpace.Data);
                asdata.Add("AssetClass", GetFullName(blendSpace.ClassIndex.Index));

                jdata.Add("SkeletonGuid", GuidToJson(blendSpace.SkeletonGuid));
                jdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

                asdata.Add("AssetObjectData", jdata);
                ja.Add("AssetSerializedData", asdata);
                ja.Add(ObjectHierarchy(Asset));
                File.WriteAllText(path1, ja.ToString());
            }
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
