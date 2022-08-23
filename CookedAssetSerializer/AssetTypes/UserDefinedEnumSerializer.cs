namespace CookedAssetSerializer.AssetTypes;

public class UserDefinedEnumSerializer : Serializer<EnumExport>
{
    public UserDefinedEnumSerializer(JSONSettings assetSettings, UAsset asset)
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
        
        var names = new JArray();
        foreach (var (fName, index) in ClassExport.Enum.Names) 
        {
            names.Add(new JObject(new JProperty("Value", index), new JProperty("Name", fName.ToName())));
        }
        AssetData.Add("Names", names);
        AssetData.Add("CppForm", (uint)ClassExport.Enum.CppForm);

        var namesMap = new JArray();
        var map = (ClassExport.Data[0] as MapPropertyData).Value;
        foreach (var key in map.Keys) {
            var pair = new JObject();
            map.TryGetValue(key, out var value);
            pair.Add("Name", SerializePropertyData(key, AssetInfo, ref RefObjects)[0].Value);
            pair.Add("DisplayName", SerializePropertyData(value, AssetInfo, ref RefObjects)[0].Value);
            namesMap.Add(pair);
        }
        AssetData.Add("DisplayNameMap", namesMap);
        
        AssignAssetSerializedData();
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}