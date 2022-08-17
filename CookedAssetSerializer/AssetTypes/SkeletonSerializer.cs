namespace CookedAssetSerializer.AssetTypes;

public class SkeletonSerializer : Serializer<SkeletonExport>
{
    public SkeletonSerializer(Settings settings, UAsset asset)
    {
        Settings = settings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;

        if (!SetupAssetInfo()) return;

        SerializeHeaders();
        
        AssetData.Add("ReferenceSkeleton", ClassExport.ReferenceSkeleton.ToJson());
        var animRetargets = new JArray();
        foreach (var entry in ClassExport.AnimRetargetSources) 
        {
            animRetargets.Add(entry.Value.ToJson());
        }
        AssetData.Add("AnimRetargetSources", animRetargets);
        
        var assetObjData = new JProperty("AssetObjectData");
        var properties = new JObject();
        foreach (var property in ClassExport.Data) 
        {
            if (property.Name.ToName() != "BoneTree") 
            {
                properties.Add(SerializePropertyData(property, AssetInfo, ref RefObjects));
            } 
            else
            {
                var newBoneTree = new JArray();
                var oldBoneTree = (JArray)SerializePropertyData(property, AssetInfo, ref RefObjects)[0].Value;
                foreach (var bone in oldBoneTree)
                {
                    var obj = (JObject)bone;
                    if (Enum.TryParse(obj.Properties().First().Value.ToString().Split("::")[1],
                            out EBoneTranslationRetargetingMode res)) newBoneTree.Add(res);
                    else throw new NotImplementedException("EBoneTranslationRetargetingMode unknown value");
                }
                AssetData.Add("BoneTree", newBoneTree);
            }
        }
        
        var hasVB = false;
        var hasBP = false;
        foreach (var property in properties.Properties()) {
            if (property.Name == "VirtualBones") 
            {
                hasVB = true;
                AssetData.Add(property.DeepClone());
            }
            if (property.Name == "BlendProfiles") hasBP = true;
        }

        if (!hasVB) AssetData.Add("VirtualBones", new JArray());
        else properties.Remove("VirtualBones");
        if (!hasBP) properties.Add("BlendProfiles", new JArray());

        //properties.Add("SmartNames", new JObject());
        //properties.Add("AssetUserData", new JArray());
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        assetObjData.Value = properties;
        AssetData.Add(assetObjData);
        
        AssignAssetSerializedData();
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
    
    //public static JProperty SerializeSkeletonData(List<PropertyData> Data) {
    //	JProperty jdata;
    //	refobjects = new List<int>();
    //	jdata = new JProperty("AssetObjectData");


    //	JObject jdatavalue = new JObject();
    //	foreach (PropertyData property in Data) {
    //		if (property.Name.ToName() != "BoneTree") {
    //			jdatavalue.Add(SerializePropertyData(property));
    //		} else {
    //			JArray newbonetree = new JArray();
    //			JArray oldbonetree = (JArray)SerializePropertyData(property)[0].Value;
    //			foreach (JObject obj in oldbonetree) {

    //				if (Enum.TryParse<EBoneTranslationRetargetingMode>(obj.Properties().First().Value.ToString().Split("::")[1], out EBoneTranslationRetargetingMode res)) {
    //					newbonetree.Add(res);
    //				} else {
    //					throw new NotImplementedException("EBoneTranslationRetargetingMode unknow value");
    //				}

    //			}

    //			jdatavalue.Add("BoneTree", newbonetree);
    //		}
    //	}
    //	bool hasVB = false;
    //	bool hasBP = false;

    //	foreach (JProperty jprop in jdatavalue.Properties()) {
    //		if (jprop.Name == "VirtualBones") { hasVB = true; }
    //		if (jprop.Name == "BlendProfiles") { hasBP = true; }
    //	}
    //	if (!hasVB) { jdatavalue.Add("VirtualBones", new JArray()); }
    //	if (!hasBP) { jdatavalue.Add("BlendProfiles", new JArray()); }

    //	jdatavalue.Add("SmartNames", new JObject());
    //	jdatavalue.Add("AssetUserData", new JArray());
    //	jdatavalue.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));
    //	refobjects = new List<int>();
    //	jdata.Value = jdatavalue;

    //	return jdata;

    //}
}