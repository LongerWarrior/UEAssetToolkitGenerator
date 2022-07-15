using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeUserDefinedStruct() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			UserDefinedStructExport mainobject = Exports[Asset.mainExport - 1] as UserDefinedStructExport;

			if (mainobject != null) {

				ja.Add("AssetClass", "UserDefinedStruct");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				ja.Add("AssetSerializedData", asdata);

				asdata.Add("SuperStruct", Index(mainobject.SuperIndex.Index));
				RefObjects.Add(Index(mainobject.SuperIndex.Index));
				JArray Children = new JArray();
				foreach (FPackageIndex package in mainobject.Children) {

					if (Exports[package.Index - 1] is FunctionExport func) {
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
				asdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}