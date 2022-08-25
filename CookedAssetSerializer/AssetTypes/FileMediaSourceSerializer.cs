namespace CookedAssetSerializer.AssetTypes;

public class FileMediaSourceSerializer : SimpleAssetSerializer<FileMediaSourceExport>
{
    public FileMediaSourceSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset(null, null, new JProperty("PlayerName", 
            ClassExport.PlayerName.ToName()));
    }
}