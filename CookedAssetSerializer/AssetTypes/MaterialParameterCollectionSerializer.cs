namespace CookedAssetSerializer.AssetTypes;

public class MaterialParameterCollectionSerializer : SimpleAssetSerializer<NormalExport>
{
    public MaterialParameterCollectionSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset();
    }
}