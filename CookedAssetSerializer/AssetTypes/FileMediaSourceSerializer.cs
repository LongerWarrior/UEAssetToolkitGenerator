using Newtonsoft.Json.Linq;
using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class FileMediaSourceSerializer : SimpleAssetSerializer<FileMediaSourceExport>
    {
        public FileMediaSourceSerializer(Settings settings) : base(settings)
        {
            SerializeAsset(null, null, new JProperty("PlayerName", 
                ClassExport.PlayerName.ToName()));
        }
    }
}