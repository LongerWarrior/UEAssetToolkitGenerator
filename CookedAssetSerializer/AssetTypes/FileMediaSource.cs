using Newtonsoft.Json.Linq;
using System.IO;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeFileMediaSource() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not FileMediaSourceExport mediaSource) return;
            ja.Add("AssetClass", mediaSource.ClassIndex.ToImport(Asset).ObjectName.ToName());
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject {
                { "AssetObjectData", SerializaListOfProperties(mediaSource.Data) },
                { "PlayerName", mediaSource.PlayerName.ToName() }
            };
            ja.Add("AssetSerializedData", asData);

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
