using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeDataTable() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			DataTableExport dataTable = exports[asset.mainExport - 1] as DataTableExport;

			if (dataTable != null) {

				ja.Add("AssetClass", "DataTable");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				ja.Add("AssetSerializedData", asdata);
				//asdata.Add(SerializePropertyData(dataTable.Data[0]));
				asdata.Add(SerializaListOfProperties(dataTable.Data).Properties());
				asdata.Add(SerializeDataTable(dataTable.Table));
				asdata.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));

				ja.Add(ObjectHierarchy(asset));
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