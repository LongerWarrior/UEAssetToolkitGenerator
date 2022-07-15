using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeFileMediaSource() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			FileMediaSourceExport mediasource = Exports[Asset.mainExport - 1] as FileMediaSourceExport;

			if (mediasource != null) {

				ja.Add("AssetClass", mediasource.ClassIndex.ToImport(Asset).ObjectName.ToName());
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				asdata.Add("AssetObjectData", SerializaListOfProperties(mediasource.Data));
				asdata.Add("PlayerName", mediasource.PlayerName.ToName());
				ja.Add("AssetSerializedData", asdata);

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}