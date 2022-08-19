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

    public void SerializeAsset(JProperty assetClass = null, JProperty extraAssetData = null, 
        JProperty extraAssetObjectData = null, JProperty extraProperties = null, bool skipRefs = false, bool skipAOD = false)
    {
        if (assetClass != null) AssetData.Add(assetClass);

        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);

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
        
        if (extraAssetData != null) AssetData.Add(extraAssetData);
        
        if (extraProperties != null) properties.Add(extraProperties);
        
        if (!skipRefs) properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        
        if (!skipAOD) AssetData.Add("AssetObjectData", properties);
        else
        {
            AssetData.Add(SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects).Properties());
            AssetData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        }
        
        if (extraAssetObjectData != null) AssetData.Add(extraAssetObjectData);
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}