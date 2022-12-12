using CookedAssetSerializer.FBX;
using static CookedAssetSerializer.FBX.SkeletonFBX;

namespace CookedAssetSerializer.AssetTypes;

public class SkeletonSerializer : Serializer<SkeletonExport>
{
    public SkeletonSerializer(JSONSettings settings, UAsset asset)
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
        
        // Export raw mesh data into seperate FBX file that can be imported back into UE
        /*var path2 = Path.ChangeExtension(OutPath, "fbx");
        string error = "";
        new SkeletonFBX(BuildSkeletonStruct(), path2, false, ref error);

        if (!File.Exists(path2)) 
        {
            IsSkipped = true;
            if (error != "")
            {
                SkippedCode = error;
            }
            else
            {
                SkippedCode = "No FBX file supplied!";
            }
            return;
        }*/
        
        AssignAssetSerializedData();
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
    
    FSkeletonStruct BuildSkeletonStruct()
    {
        FSkeletonStruct sk;
        sk.Skeleton = ClassExport.ReferenceSkeleton;
        return sk;
    }
}