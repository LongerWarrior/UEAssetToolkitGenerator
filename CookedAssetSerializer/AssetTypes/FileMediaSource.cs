using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeFileMediaSource() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			FileMediaSourceExport mediasource = exports[asset.mainExport - 1] as FileMediaSourceExport;

			if (mediasource != null) {

				ja.Add("AssetClass", mediasource.ClassIndex.ToImport(asset).ObjectName.ToName());
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				asdata.Add("AssetObjectData", SerializaListOfProperties(mediasource.Data));
				asdata.Add("PlayerName", mediasource.PlayerName.ToName());
				ja.Add("AssetSerializedData", asdata);

				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}