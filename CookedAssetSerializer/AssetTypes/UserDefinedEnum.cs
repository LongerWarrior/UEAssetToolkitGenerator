using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeUserDefinedEnum() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not EnumExport uenum) return;
            ja.Add("AssetClass", "UserDefinedEnum");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);
            var names = new JArray();
            foreach (var (fname, index) in uenum.Enum.Names)
                names.Add(new JObject(new JProperty("Value", index), new JProperty("Name", fname.ToName())));
            asData.Add("Names", names);
            asData.Add("CppForm", (uint)uenum.Enum.CppForm);

            var namesMap = new JArray();
            var map = (uenum.Data[0] as MapPropertyData)?.Value;
            if (map != null)
                foreach (var key in map.Keys) {
                    var pair = new JObject();
                    map.TryGetValue(key, out var value);
                    pair.Add("Name", SerializePropertyData(key)[0].Value);
                    pair.Add("DisplayName", SerializePropertyData(value)[0].Value);
                    namesMap.Add(pair);
                }

            asData.Add("DisplayNameMap", namesMap);

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
