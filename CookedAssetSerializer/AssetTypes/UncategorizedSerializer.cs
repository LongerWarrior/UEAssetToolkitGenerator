namespace CookedAssetSerializer.AssetTypes;

public class UncategorizedSerializer : SimpleAssetSerializer<NormalExport>
{
    public UncategorizedSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(false)) return;
        SerializeAsset();
    }
}