namespace CookedAssetSerializer.AssetTypes;

public class StringTableSerializer : Serializer<StringTableExport>
{
    public StringTableSerializer(JSONSettings assetSettings, UAsset asset)
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
        
        //AssignAssetSerializedData();

        AssetData.Add("TableNamespace", 
            ClassExport.Table.TableNamespace == null ? "" : ClassExport.Table.TableNamespace.ToString());
        var strings = new JObject();
        foreach (var key in ClassExport.Table.Keys) 
        {
            ClassExport.Table.TryGetValue(key, out var value);
            strings.Add(key?.ToString() ?? "", value?.ToString() ?? "");
        }
        AssetData.Add("SourceStrings", strings);
        AssetData.Add("MetaData", new JObject());
        
        AssignAssetSerializedData();
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}