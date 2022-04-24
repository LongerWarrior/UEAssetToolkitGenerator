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

		public static void SerializeCurveBase() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			CurveBaseExport blendSpace = exports[asset.mainExport - 1] as CurveBaseExport;

			if (blendSpace != null) {

				ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(asset).ObjectName.ToName());
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				asdata.Add("AssetClass", GetFullName(blendSpace.ClassIndex.Index));
				asdata.Add("AssetObjectData", SerializaListOfProperties(blendSpace.Data));
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}