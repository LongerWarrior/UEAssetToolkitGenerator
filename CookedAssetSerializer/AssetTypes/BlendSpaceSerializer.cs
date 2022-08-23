namespace CookedAssetSerializer.AssetTypes;

public struct ParameterNames
{
    public string StructPropertyData;
    public string FName;
    public string FloatPropertyData;
    public string StrPropertyData;
    public string FString;
    public float Value;
}

public class BlendSpaceSerializer : Serializer<BlendSpaceBaseExport>
{
    public BlendSpaceSerializer(JSONSettings assetSettings, UAsset asset)
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

        var blendSpaceParameterNames = new ParameterNames
        {
            StructPropertyData = "BlendParameters",
            FName = "BlendParameter",
            FloatPropertyData = "Max",
            StrPropertyData = "DisplayName",
            FString = "None",
            Value = 100.0f
        };
        PopulateParameters(ref ClassExport.Data, blendSpaceParameterNames);
        var interpolationParameterNames = new ParameterNames
        {
            StructPropertyData = "InterpolationParam",
            FName = "InterpolationParameter",
            FloatPropertyData = "InterpolationTime",
            StrPropertyData = "InterpolationType",
            FString = "BSIT_Average",
            Value = 0.0f
        };
        PopulateParameters(ref ClassExport.Data, interpolationParameterNames);
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        AssetData.Add("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset));
        properties.Add("SkeletonGuid", GuidToJson(ClassExport.SkeletonGuid));
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
        AssetData.Add("AssetObjectData", properties);
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }

    private void PopulateParameters(ref List<PropertyData> data, ParameterNames pm, int v = 3)
    {
        var fullEntries = Enumerable.Range(0, v).ToList();
        var entries = (from t in data where t.Name.ToName() == pm.StructPropertyData select t.DuplicationIndex).ToList();
        if (entries.Count <= 0) return;
        var missing = fullEntries.Except(entries).ToList();
        data.AddRange(missing.Select(missed => new StructPropertyData(new FName(pm.StructPropertyData), 
            new FName(pm.FName), missed) 
            {
                Value = new List<PropertyData> 
                {
                    new StrPropertyData(new FName(pm.StrPropertyData))
                    {
                        Value = new FString(pm.FString)
                    }, 
                    new FloatPropertyData(new FName(pm.FloatPropertyData))
                    {
                        Value = pm.Value
                    }
                }
            }
        ));
        data.Sort((x, y) => {
            var ret = x.Name.ToName().CompareTo(y.Name.ToName());
            if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
            return ret;
        });
    }
}