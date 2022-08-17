﻿namespace CookedAssetSerializer.AssetTypes;

public class DummySerializer : Serializer<Export>
{
    public DummySerializer(Settings assetSettings, UAsset asset)
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

        AssetData.Add("AssetObjectData", new JObject(new JProperty("$ReferencedObjects", new JArray())));
        
        AssignAssetSerializedData();

        WriteJsonOut(new JProperty("ObjectHierarchy", new JArray()));
    }
}

public class DummyWithProps : Serializer<NormalExport>
{
    public DummyWithProps(Settings assetSettings)
    {
        Settings = assetSettings;
        SerializeAsset();
    }
    
    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;

        if (!SetupAssetInfo()) return;
        
        SerializeHeaders();
        
        var properties = SerializaListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
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