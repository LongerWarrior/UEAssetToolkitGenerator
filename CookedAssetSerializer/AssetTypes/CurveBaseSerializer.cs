using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class CurveBaseSerializer : SimpleAssetSerializer<CurveBaseExport>
    {
        public CurveBaseSerializer(Settings settings) : base(settings)
        {
            Setup();
            SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
        }
    }
}