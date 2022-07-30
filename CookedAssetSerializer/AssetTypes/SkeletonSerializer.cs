using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using UAssetAPI;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class SkeletonSerializer : Serializer<SkeletonExport>
    {
        public SkeletonSerializer(Settings settings)
        {
            Settings = settings;
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
    }
}