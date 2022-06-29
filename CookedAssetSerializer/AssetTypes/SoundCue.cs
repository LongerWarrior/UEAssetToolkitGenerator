using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using static UAssetAPI.Kismet.KismetSerializer;
using System;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeSoundCue() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            DisableGeneration.Add("FirstNode");
            SoundGraphData = new Dictionary<int, List<int>>();
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not SoundCueExport soundCue) return;
            ja.Add("AssetClass", "SoundCue");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject { { "SoundCueGraph", string.Join(Environment.NewLine, soundCue.GetCueGraph()) } };
            var jData = SerializaListOfProperties(soundCue.Data);
            jData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

            asData.Add("AssetObjectData", jData);
            ja.Add("AssetSerializedData", asData);
            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
