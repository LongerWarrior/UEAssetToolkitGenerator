using System;
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
            var fullEntries = Enumerable.Range(0, v).ToList();
            var entries = (from t in data where t.Name.ToName() == "BlendParameters" select t.DuplicationIndex).ToList();

            if (entries.Count <= 0) return;
            var missing = fullEntries.Except(entries).ToList();
            data.AddRange(missing.Select(missed => new StructPropertyData(new FName("BlendParameters"),
                new FName("BlendParameter"), missed)
                { Value = new List<PropertyData> { new StrPropertyData(new FName("DisplayName"))
                { Value = new FString("None") }, new FloatPropertyData(new FName("Max"))
                { Value = 100.0f } } }));
            data.Sort((x, y) => {
                var ret = string.Compare(x.Name.ToName(), y.Name.ToName(), StringComparison.Ordinal);
                if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                return ret;
            });
        }
    }
}
