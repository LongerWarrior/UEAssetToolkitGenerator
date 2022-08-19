namespace CookedAssetSerializer.AssetTypes;

public class SubsurfaceProfileSerializer : SimpleAssetSerializer<NormalExport>
{
    public SubsurfaceProfileSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(false, false)) return;
        SerializeAsset();
    }
}