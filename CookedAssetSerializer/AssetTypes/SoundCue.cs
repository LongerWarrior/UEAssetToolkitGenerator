using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;
using static UAssetAPI.Kismet.KismetSerializer;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeSoundCue() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			DisableGeneration.Add("FirstNode");
			SoundGraphData = new();
			JObject ja = new JObject();
			SoundCueExport soundcue = Exports[System.Asset.mainExport - 1] as SoundCueExport;

			if (soundcue != null) {

				ja.Add("AssetClass", "SoundCue");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				asdata.Add("SoundCueGraph", String.Join(Environment.NewLine, soundcue.GetCueGraph()));

				JObject jdata = SerializaListOfProperties(soundcue.Data);

				jdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

				asdata.Add("AssetObjectData", jdata);
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(System.Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}

    }


}