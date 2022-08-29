namespace CookedAssetSerializer.AssetTypes;

public class UncategorizedSerializer : SimpleAssetSerializer<NormalExport>
{
    public UncategorizedSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(false)) return;
        SerializeAsset();
    }
}