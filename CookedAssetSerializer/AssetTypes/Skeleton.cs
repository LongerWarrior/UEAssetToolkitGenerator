using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeSkeleton() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			SkeletonExport mainobject = Exports[Asset.mainExport - 1] as SkeletonExport;

			if (mainobject != null) {
				ja.Add("AssetClass", "Skeleton");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);


				JObject asdata = new JObject();
				asdata.Add("ReferenceSkeleton", mainobject.ReferenceSkeleton.ToJson());
				JArray jdict = new JArray();
				foreach (KeyValuePair<FName, FReferencePose> entry in mainobject.AnimRetargetSources) {
					jdict.Add(entry.Value.ToJson());
				}
				asdata.Add("AnimRetargetSources", jdict);

				JProperty jdata = new JProperty("AssetObjectData");
				JObject jdatavalue = new JObject();
				foreach (PropertyData property in mainobject.Data) {
					if (property.Name.ToName() != "BoneTree") {
						jdatavalue.Add(SerializePropertyData(property));
					} else {
						JArray newbonetree = new JArray();
						JArray oldbonetree = (JArray)SerializePropertyData(property)[0].Value;
						foreach (JObject obj in oldbonetree) {

							if (Enum.TryParse(obj.Properties().First().Value.ToString().Split("::")[1], out EBoneTranslationRetargetingMode res)) {
								newbonetree.Add(res);
							} else {
								throw new NotImplementedException("EBoneTranslationRetargetingMode unknow value");
							}

						}
						asdata.Add("BoneTree", newbonetree);
					}
				}
				bool hasVB = false;
				bool hasBP = false;

				foreach (JProperty jprop in jdatavalue.Properties()) {
					if (jprop.Name == "VirtualBones") {
						hasVB = true;
						asdata.Add(jprop.DeepClone());
					}
					if (jprop.Name == "BlendProfiles") { hasBP = true; }
				}
				if (!hasVB) { asdata.Add("VirtualBones", new JArray()); } else { jdatavalue.Remove("VirtualBones"); }
				if (!hasBP) { jdatavalue.Add("BlendProfiles", new JArray()); }

				//jdatavalue.Add("SmartNames", new JObject());
				//jdatavalue.Add("AssetUserData", new JArray());
				jdatavalue.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
				jdata.Value = jdatavalue;


				asdata.Add(jdata);
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
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