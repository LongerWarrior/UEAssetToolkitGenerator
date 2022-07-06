using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using System;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeDummy() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			Export simple = Exports[Asset.mainExport - 1] as Export;

			if (simple != null) {

				ja.Add("AssetClass", "SimpleAsset");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				asdata.Add("AssetClass", GetFullName(simple.ClassIndex.Index));

				asdata.Add("AssetObjectData", new JObject(new JProperty("$ReferencedObjects", new JArray())));
				ja.Add("AssetSerializedData", asdata);
				ja.Add(new JProperty("ObjectHierarchy", new JArray()));
				File.WriteAllText(path1, ja.ToString());

			}
		}

		public static List<string> waveprops = new();
		public static void SerializeDummy(bool withproperties) {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			NormalExport simple = Exports[Asset.mainExport - 1] as NormalExport;

			if (simple != null) {

				ja.Add("AssetClass", "SimpleAsset");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				asdata.Add("AssetClass", GetFullName(simple.ClassIndex.Index));
				JObject jdata = SerializaListOfProperties(simple.Data);

				foreach (JProperty prop in jdata.Properties()) {
					if (!waveprops.Contains(prop.Name)) {
						waveprops.Add(prop.Name);
                    }
                }


				jdata.Add("$ReferencedObjects", new JArray());
				asdata.Add("AssetObjectData", jdata);
				ja.Add("AssetSerializedData", asdata);
				if (RefObjects.Count > 0) {
					Console.WriteLine(Asset.FilePath);
					ja.Add(ObjectHierarchy(Asset));
				} else {
					ja.Add(new JProperty("ObjectHierarchy", new JArray()));
				}
				
				File.WriteAllText(path1, ja.ToString());

			}
		}

	}


}