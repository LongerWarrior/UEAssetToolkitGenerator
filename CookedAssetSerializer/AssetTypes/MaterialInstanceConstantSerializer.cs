using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class MaterialInstanceConstantSerializer : SimpleAssetSerializer<NormalExport>
    {
        public MaterialInstanceConstantSerializer(Settings settings) : base(settings)
        {
            Setup();

            JProperty property = null;
            
            if (!FindPropertyData(ClassExport, "StaticParameters", out PropertyData prop)) 
            {
                property = new JProperty("StaticParameters", new JObject());
            }
            if (!FindPropertyData(ClassExport, "AssetUserData", out prop)) 
            {
                property = new JProperty("AssetUserData", new JArray());
            }
            
            SerializeAsset(null, null, null, 
                property);
        }
    }
}