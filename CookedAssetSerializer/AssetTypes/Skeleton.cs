using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeSkeleton() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not SkeletonExport mainObject) return;
            ja.Add("AssetClass", "Skeleton");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);

            var asData = new JObject { { "ReferenceSkeleton", mainObject.ReferenceSkeleton.ToJson() } };
            var jDict = new JArray();
            foreach (var entry in mainObject.AnimRetargetSources) jDict.Add(entry.Value.ToJson());
            asData.Add("AnimRetargetSources", jDict);

            var jData = new JProperty("AssetObjectData");
            var jDataValue = new JObject();
            foreach (var property in mainObject.Data)
                if (property.Name.ToName() != "BoneTree") {
                    jDataValue.Add(SerializePropertyData(property));
                } else {
                    var newBoneTree = new JArray();
                    var oldBoneTree = (JArray)SerializePropertyData(property)[0].Value;
                    foreach (var jToken in oldBoneTree) {
                        var obj = (JObject)jToken;
                        if (Enum.TryParse<EBoneTranslationRetargetingMode>(
                                obj.Properties().First().Value.ToString().Split("::")[1], out var res))
                            newBoneTree.Add(res);
                        else
                            throw new NotImplementedException("EBoneTranslationRetargetingMode unknown value");
                    }

                    asData.Add("BoneTree", newBoneTree);
                }

            var hasVB = false;
            var hasBP = false;
            foreach (var jProp in jDataValue.Properties()) {
                if (jProp.Name == "VirtualBones") {
                    hasVB = true;
                    asData.Add(jProp.DeepClone());
                }

                if (jProp.Name == "BlendProfiles") hasBP = true;
            }

            if (!hasVB)
                asData.Add("VirtualBones", new JArray());
            else
                jDataValue.Remove("VirtualBones");
            if (!hasBP) jDataValue.Add("BlendProfiles", new JArray());

            //jDataValue.Add("SmartNames", new JObject());
            //jDataValue.Add("AssetUserData", new JArray());
            jDataValue.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            jData.Value = jDataValue;

            asData.Add(jData);
            ja.Add("AssetSerializedData", asData);
            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
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
}
