namespace CookedAssetSerializer.AssetTypes;

public class PhysicalMaterialSerializer : SimpleAssetSerializer<NormalExport>
{
    public PhysicalMaterialSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset();
    }
}