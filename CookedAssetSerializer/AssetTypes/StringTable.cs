using Newtonsoft.Json.Linq;
using System.IO;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeStringTable() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not StringTableExport stringTable) return;
            ja.Add("AssetClass", "StringTable");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);
            asData.Add("TableNamespace", stringTable.Table.TableNamespace == null ? "" : stringTable.Table.TableNamespace.ToString());
            var strings = new JObject();
            foreach (var key in stringTable.Table.Keys) {
                stringTable.Table.TryGetValue(key, out var value);
                strings.Add(key.ToString(), value.ToString());
            }

            asData.Add("SourceStrings", strings);
            asData.Add("MetaData", new JObject());

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
