using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeCurveBase() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			CurveBaseExport blendSpace = Exports[Asset.mainExport - 1] as CurveBaseExport;

			if (blendSpace != null) {

				ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(Asset).ObjectName.ToName());
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				asdata.Add("AssetClass", GetFullName(blendSpace.ClassIndex.Index));
				JObject jdata = SerializaListOfProperties(blendSpace.Data);
				jdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
				asdata.Add("AssetObjectData", jdata);
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}