using Newtonsoft.Json.Linq;
using System;
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

		public static void SerializeMaterialInstanceConstant() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			NormalExport material = exports[asset.mainExport - 1] as NormalExport;
			DisableGeneration.Add("MaterialLayersParameters");
			
			if (material != null) {

				ja.Add("AssetClass", "MaterialInstanceConstant");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();


				JObject aodata = SerializaListOfProperties(material.Data);
				if (!FindPropertyData(material, "StaticParameters", out PropertyData prop)) {
					aodata.Add("StaticParameters", new JObject());
				}
				if (!FindPropertyData(material, "AssetUserData", out prop)) {
					aodata.Add("AssetUserData", new JArray());
				}
				aodata.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));
				refobjects = new List<int>();
				asdata.Add("AssetObjectData", aodata);
				ja.Add("AssetSerializedData", asdata);
				
				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}


    }


}