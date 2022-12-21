namespace CookedAssetSerializer.AssetTypes;

public class CurveBaseSerializer : SimpleAssetSerializer<CurveBaseExport>
{
    public CurveBaseSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }
}