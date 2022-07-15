using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeUserDefinedEnum() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			EnumExport uenum = Exports[Asset.mainExport - 1] as EnumExport;

			if (uenum != null) {

				ja.Add("AssetClass", "UserDefinedEnum");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				ja.Add("AssetSerializedData", asdata);
				JArray names = new JArray();
				foreach ((FName fname, long index) in uenum.Enum.Names) {
					names.Add(new JObject(new JProperty("Value", index), new JProperty("Name", fname.ToName())));
				}
				asdata.Add("Names", names);
				asdata.Add("CppForm", (uint)uenum.Enum.CppForm);

				JArray namesmap = new JArray();
				TMap<PropertyData, PropertyData> map = (uenum.Data[0] as MapPropertyData).Value;
				foreach (PropertyData key in map.Keys) {
					JObject pair = new JObject();
					map.TryGetValue(key, out PropertyData value);
					pair.Add("Name", SerializePropertyData(key)[0].Value);
					pair.Add("DisplayName", SerializePropertyData(value)[0].Value);
					namesmap.Add(pair);
				}


				asdata.Add("DisplayNameMap", namesmap);

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}