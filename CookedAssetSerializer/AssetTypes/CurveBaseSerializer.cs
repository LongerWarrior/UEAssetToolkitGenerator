using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class CurveBaseSerializer : SimpleAssetSerializer<CurveBaseExport>
    {
        public CurveBaseSerializer(Settings settings) : base(settings)
        {
            SerializeAsset();
        }
    }
}