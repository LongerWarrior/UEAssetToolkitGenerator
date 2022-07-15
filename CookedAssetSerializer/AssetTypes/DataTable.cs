using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeDataTable() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			DataTableExport dataTable = Exports[Asset.mainExport - 1] as DataTableExport;

			if (dataTable != null) {

				ja.Add("AssetClass", "DataTable");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				ja.Add("AssetSerializedData", asdata);
				//asdata.Add(SerializePropertyData(dataTable.Data[0]));
				asdata.Add(SerializaListOfProperties(dataTable.Data).Properties());
				asdata.Add(SerializeDataTable(dataTable.Table));
				asdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}

		public static JProperty SerializeDataTable(UDataTable table) {

			JProperty jdata = new JProperty("RowData");

			if (table.Data.Count > 0) {
				JObject jdatavalue = new JObject();
				foreach (PropertyData property in table.Data) {
					jdatavalue.Add(SerializePropertyData(property));
				}
				jdata.Value = jdatavalue;
			} else {
				jdata.Value = -1;

			}
			return jdata;
		}
	}


}