namespace CookedAssetSerializer.AssetTypes;

public class MaterialInstanceConstantSerializer : SimpleAssetSerializer<NormalExport>
{
    public MaterialInstanceConstantSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(true)) return;

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