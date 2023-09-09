namespace CookedAssetSerializer.AssetTypes;

public class BehaviorTreeSerializer : SimpleAssetSerializer<NormalExport>
{
    public BehaviorTreeSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset();
    }
}