using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializePhysicalMaterial() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			NormalExport material = Exports[Asset.mainExport-1] as NormalExport;

			if (material != null) {

				ja.Add("AssetClass", "PhysicalMaterial");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				if (CircularDependency.Contains( GetFullName(material.ClassIndex.Index))) {
					asdata.Add("SkipDependecies", true);
				}
				
				JObject aodata = SerializaListOfProperties(material.Data);
				aodata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
				RefObjects = new List<int>();
				asdata.Add("AssetObjectData", aodata);
				ja.Add("AssetSerializedData", asdata);
				
				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}	

    }

}