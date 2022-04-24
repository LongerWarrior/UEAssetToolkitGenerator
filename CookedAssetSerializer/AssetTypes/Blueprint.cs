using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Program;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.FieldTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeBPAsset() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			ClassExport mainobject = exports[asset.mainExport - 1] as ClassExport;

			if (mainobject != null) {

				string classname = mainobject.ClassIndex.ToImport(asset).ObjectName.ToName();
				switch (classname) {
					case "BlueprintGeneratedClass": ja.Add("AssetClass", "Blueprint"); break;
					case "WidgetBlueprintGeneratedClass": ja.Add("AssetClass", "WidgetBlueprint"); break;
					case "AnimBlueprintGeneratedClass": ja.Add("AssetClass", "AnimBlueprint"); break;
					default: ja.Add("AssetClass", "UknownType"); break;
				}

				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();

				asdata.Add("SuperStruct", Index(mainobject.SuperIndex.Index));
				JArray Children = new JArray();
				List<FunctionExport> functions = new List<FunctionExport>();
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
				asdata.Add("ClassFlags", ((Int64)mainobject.ClassFlags).ToString());
				asdata.Add("ClassWithin", Index(mainobject.ClassWithin.Index));
				asdata.Add("ClassConfigName", mainobject.ClassConfigName.Value.Value);
				asdata.Add("Interfaces", SerializeInterfaces(mainobject.Interfaces.ToList()));
				asdata.Add("ClassDefaultObject", Index(mainobject.ClassDefaultObject.Index));
				ja.Add("AssetSerializedData", asdata);
				asdata.Add(SerializeData(mainobject.Data, true));

				CollectGeneratedVariables(mainobject);
				asdata.Add("GeneratedVariableNames", JArray.FromObject(GeneratedVariables));

				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}