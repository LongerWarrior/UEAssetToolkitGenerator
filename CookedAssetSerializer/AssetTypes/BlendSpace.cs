using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeBlendSpace() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			BlendSpaceBaseExport blendSpace = exports[asset.mainExport - 1] as BlendSpaceBaseExport;

			if (blendSpace != null) {

				ja.Add("AssetClass", blendSpace.ClassIndex.ToImport(asset).ObjectName.ToName());
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();


				PopulateBlendParameters(ref blendSpace.Data);

				JObject jdata = SerializaListOfProperties(blendSpace.Data);
				asdata.Add("AssetClass", GetFullName(blendSpace.ClassIndex.Index));

				jdata.Add("SkeletonGuid", GuidToJson(blendSpace.SkeletonGuid));
				jdata.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));

				asdata.Add("AssetObjectData", jdata);
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}

        private static void PopulateBlendParameters(ref List<PropertyData> data, int v = 3) {
	
			List<int> fullentries = Enumerable.Range(0, v).ToList();
			List<int> entries = new List<int>();
			for (int i = 0; i < data.Count; i++) {
				if (data[i].Name.ToName() == "BlendParameters") {
					entries.Add(data[i].DuplicationIndex);
				}
			}

			if (entries.Count > 0) {
				var missing = fullentries.Except(entries).ToList();
				foreach (int missed in missing) {

					data.Add(new StructPropertyData(new FName("BlendParameters"), new FName("BlendParameter"), missed) {
						Value = new List<PropertyData> {
							new StrPropertyData(new FName("DisplayName"){ Value = new FString("None")}),
							new FloatPropertyData(new FName("Max")){ Value = 100.0f}
						}
					});
				}
				data.Sort((x, y) => {
					var ret = x.Name.ToName().CompareTo(y.Name.ToName());
					if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
					return ret;
				});
			}

		}
    }


}