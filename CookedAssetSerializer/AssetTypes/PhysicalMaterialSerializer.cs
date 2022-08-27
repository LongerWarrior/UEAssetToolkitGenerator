namespace CookedAssetSerializer.AssetTypes;

public class PhysicalMaterialSerializer : SimpleAssetSerializer<NormalExport>
{
    public PhysicalMaterialSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        Setup(true);
        SerializeAsset();
    }
}