namespace CookedAssetSerializer.AssetTypes;

public class CurveBaseSerializer : SimpleAssetSerializer<CurveBaseExport>
{
    public CurveBaseSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(true)) return;
        SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }
}