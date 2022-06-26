using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.FieldTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeUserDefinedStruct() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			UserDefinedStructExport mainobject = exports[asset.mainExport - 1] as UserDefinedStructExport;

			if (mainobject != null) {

				ja.Add("AssetClass", "UserDefinedStruct");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				ja.Add("AssetSerializedData", asdata);

				asdata.Add("SuperStruct", Index(mainobject.SuperIndex.Index));
				refobjects.Add(Index(mainobject.SuperIndex.Index));
				JArray Children = new JArray();
				foreach (FPackageIndex package in mainobject.Children) {

					if (exports[package.Index - 1] is FunctionExport func) {
						Children.Add(SerializeFunction(func));
					}
				}
				asdata.Add("Children", Children);
				JArray ChildProperties = new JArray();

				foreach (FProperty property in mainobject.LoadedProperties) {
					ChildProperties.Add(SerializeProperty(property));
				}
				asdata.Add("ChildProperties", ChildProperties);
				asdata.Add("StructFlags", (uint)mainobject.StructFlags);

				bool bguid = false;
				if (FindPropertyData(mainobject,"Guid",out PropertyData prop)) {
					StructPropertyData _guid = (StructPropertyData)prop;
					if (FindPropertyData(_guid.Value, "Guid", out PropertyData _prop)) {
						GuidPropertyData realguid = (GuidPropertyData)_prop;
						uint[] guid = realguid.Value.ToUnsignedInts();
						asdata.Add("Guid", guid.Select(x => x.ToString("X8")).Aggregate((a, b) => a + b));
						bguid = true;
					}
				} 
				
				if (!bguid) {
					asdata.Add("Guid", new Guid("00000000000000000000000000000000"));
				}
				
				asdata.Add("StructDefaultInstance", SerializaListOfProperties(mainobject.DefaultStructInstance));
				asdata.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));

				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}