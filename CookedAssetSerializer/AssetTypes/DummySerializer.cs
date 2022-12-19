using UAssetAPI.AssetRegistry;

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

public class RawDummy
{
    public RawDummy(JSONSettings settings, KeyValuePair<string, AssetData> assetInfo, string assetName)
    {
        string outPath = Path.Join(settings.JSONDir, assetInfo.Key) + ".json";
        Directory.CreateDirectory(Path.GetDirectoryName(outPath));
        
        JObject assetData = new JObject();
        JObject data = new JObject
        {
            new JProperty("AssetClass", assetInfo.Value.AssetClass),
            new JProperty("AssetPackage", assetInfo.Key),
            new JProperty("AssetName", assetName)
        };
        assetData.Add("SkipDependecies", false);
        data.Add("AssetSerializedData", assetData);
        data.Add(new JProperty("ObjectHierarchy", new JArray()));
        File.WriteAllText(outPath, data.ToString());
    }
}