namespace CookedAssetSerializer.AssetTypes;

public class CurveBaseSerializer : SimpleAssetSerializer<CurveBaseExport>
{
    public CurveBaseSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        Setup(true);
        SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }
}