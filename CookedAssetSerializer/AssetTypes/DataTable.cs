using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeDataTable() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not DataTableExport dataTable) return;
            ja.Add("AssetClass", "DataTable");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);
            //asData.Add(SerializePropertyData(dataTable.Data[0]));
            asData.Add(SerializaListOfProperties(dataTable.Data).Properties());
            asData.Add(SerializeDataTable(dataTable.Table));
            asData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }

        private static JProperty SerializeDataTable(UDataTable table) {
            var jData = new JProperty("RowData");

            if (table.Data.Count > 0) {
                var jDataValue = new JObject();
                foreach (PropertyData property in table.Data) jDataValue.Add(SerializePropertyData(property));
                jData.Value = jDataValue;
            } else {
                jData.Value = -1;
            }

            return jData;
        }
    }
}
