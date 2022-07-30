using Newtonsoft.Json.Linq;
using System.Linq;
using UAssetAPI;
using UAssetAPI.Kismet;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class StringTableSerializer : Serializer<StringTableExport>
    {
        public StringTableSerializer(Settings assetSettings)
        {
            Settings = assetSettings;
            SerializeAsset();
        }

        private void SerializeAsset()
        {
            if (!SetupSerialization()) return;

            if (!SetupAssetInfo()) return;

            SerializeHeaders();
        }
    }
}