namespace CookedAssetSerializer.AssetTypes;

public class PhysicsAssetSerializer : SimpleAssetSerializer<NormalExport>
{
    public PhysicsAssetSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset();
    }
}