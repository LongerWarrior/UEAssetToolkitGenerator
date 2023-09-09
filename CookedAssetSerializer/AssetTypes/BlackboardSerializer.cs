namespace CookedAssetSerializer.AssetTypes;

public class BlackboardSerializer : SimpleAssetSerializer<NormalExport>
{
    public BlackboardSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset();
    }
}