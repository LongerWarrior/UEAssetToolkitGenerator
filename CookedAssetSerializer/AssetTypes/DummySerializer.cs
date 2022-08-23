namespace CookedAssetSerializer.AssetTypes;

public class DummySerializer : SimpleAssetSerializer<NormalExport>
{
    public DummySerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(false)) return;
        AssignAssetSerializedData();
        WriteJsonOut(new JProperty("ObjectHierarchy", new JArray()));
    }
}

public class DummyWithProps : Serializer<NormalExport>
{
    public DummyWithProps(JSONSettings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }
    
    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;

        if (!SetupAssetInfo()) return;
        
        SerializeHeaders();
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        List<string> waveProps = new();
        foreach (var prop in properties.Properties()) 
        {
            if (!waveProps.Contains(prop.Name)) 
            {
                waveProps.Add(prop.Name);
            }
        }
        properties.Add("$ReferencedObjects", new JArray());
        AssetData.Add("AssetObjectData", properties);
        
        AssignAssetSerializedData();
        
        if (RefObjects.Count > 0) 
        {
            Console.WriteLine(Asset.FilePath);
            WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
        } 
        else WriteJsonOut(new JProperty("ObjectHierarchy", new JArray()));
    }
}