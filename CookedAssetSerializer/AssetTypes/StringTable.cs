using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeStringTable() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			StringTableExport stringTable = Exports[Asset.mainExport - 1] as StringTableExport;

			if (stringTable != null) {

				ja.Add("AssetClass", "StringTable");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();


				ja.Add("AssetSerializedData", asdata);
				if (stringTable.Table.TableNamespace == null) {
					asdata.Add("TableNamespace", "");
				} else {
					asdata.Add("TableNamespace", stringTable.Table.TableNamespace.ToString());
				}
				JObject strings = new JObject();
				foreach (FString key in stringTable.Table.Keys) {
					stringTable.Table.TryGetValue(key, out FString value);
					strings.Add(key.ToString(), value.ToString());
				}
				asdata.Add("SourceStrings", strings);
				asdata.Add("MetaData", new JObject());

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}