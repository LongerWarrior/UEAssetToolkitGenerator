namespace CookedAssetSerializer.AssetTypes;

public class SimpleAssetSerializer<T> : Serializer<T> where T : NormalExport
{
    public SimpleAssetSerializer(Settings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
    }

    public bool Setup(bool isInheritor = false, bool isSimple = true)
    {
        if (!SetupSerialization()) return false;

        if (!SetupAssetInfo()) return false;
        
        if (isSimple && !isInheritor) ClassName = "SimpleAsset";

        SerializeHeaders();
        
        if (!isInheritor) AssetData.Add("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset));

        return true;
    }

    public void SerializeAsset(JProperty preSlopAssetData = null, JProperty postSlopAssetData = null, 
        JProperty extraAssetData = null, JProperty postSlopProps = null, bool skipRefs = false)
    {
        if (preSlopAssetData != null) AssetData.Add(preSlopAssetData);

        var properties = SerializaListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        
        if (GetFullName(ClassExport.ClassIndex.Index, Asset) == "/Script/Paper2D.PaperSprite") 
        {
            if (FindPropertyData(ClassExport, "BakedSourceTexture", out var _source))
            {
                var source = (ObjectPropertyData)_source;
                properties.Add(new JProperty("SourceTexture", GetFullName(source.Value.Index, Asset)));
            }
            for (var i = 0; i < properties.Properties().Count(); i++) 
            {
                var props = properties.Properties().ElementAt(i);
                switch (props.Name)
                {
                    case "BakedSourceDimension":
                        properties.Add("SourceDimension", props.Value);
                        break;
                    case "BakedRenderData":
                        properties.Add("RenderData", props.Value);
                        break;
                }
            }
        }
        
        if (postSlopAssetData != null) AssetData.Add(postSlopAssetData);
        
        if (postSlopProps != null) properties.Add(postSlopProps);
        
        if (!skipRefs) properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        
        AssetData.Add("AssetObjectData", properties);
        
        if (extraAssetData != null) AssetData.Add(extraAssetData);
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}