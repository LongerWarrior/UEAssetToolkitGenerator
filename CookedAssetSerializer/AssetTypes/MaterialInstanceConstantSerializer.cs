namespace CookedAssetSerializer.AssetTypes;

public class MaterialInstanceConstantSerializer : SimpleAssetSerializer<NormalExport>
{
    public MaterialInstanceConstantSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;

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