namespace CookedAssetSerializer.AssetTypes;

public class FileMediaSourceSerializer : SimpleAssetSerializer<FileMediaSourceExport>
{
    public FileMediaSourceSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        Setup(true);
        SerializeAsset(null, null, new JProperty("PlayerName", 
            ClassExport.PlayerName.ToName()));
    }
}