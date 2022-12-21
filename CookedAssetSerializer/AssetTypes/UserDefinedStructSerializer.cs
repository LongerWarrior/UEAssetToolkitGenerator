namespace CookedAssetSerializer.AssetTypes;

public class UserDefinedStructSerializer : Serializer<UserDefinedStructExport>
{
    public UserDefinedStructSerializer(JSONSettings assetSettings, UAsset asset)
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
        
        AssetData.Add("SuperStruct", Index(ClassExport.SuperIndex.Index, Dict));
        RefObjects.Add(Index(ClassExport.SuperIndex.Index, Dict));
        
        var children = new JArray();
        foreach (var package in ClassExport.Children) 
        {
            if (Asset.Exports[package.Index - 1] is FunctionExport func) 
            {
                children.Add(SerializeFunction(func, AssetInfo));
            }
        }
        AssetData.Add("Children", children);
        
        var childProperties = new JArray();
        foreach (var property in ClassExport.LoadedProperties) 
        {
            childProperties.Add(SerializeProperty(property, AssetInfo));
        }
        AssetData.Add("ChildProperties", childProperties);
        AssetData.Add("StructFlags", (uint)ClassExport.StructFlags);

        var bGuid = false;
        if (FindPropertyData(ClassExport,"Guid",out PropertyData prop)) 
        {
            var guidProp = (StructPropertyData)prop;
            if (FindPropertyData(guidProp.Value, "Guid", out PropertyData _prop)) 
            {
                var realGuid = (GuidPropertyData)_prop;
                var guid = realGuid.Value.ToUnsignedInts();
                AssetData.Add("Guid", guid.Select(x => x.ToString("X8")).Aggregate((a, b) => a + b));
                bGuid = true;
            }
        } 
				
        if (!bGuid) AssetData.Add("Guid", new Guid("00000000000000000000000000000000"));

        AssetData.Add("StructDefaultInstance", SerializeListOfProperties(ClassExport.DefaultStructInstance, AssetInfo, ref RefObjects));
        AssetData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        
        AssignAssetSerializedData();
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}