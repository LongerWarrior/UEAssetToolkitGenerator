using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using static CookedAssetSerializer.Globals;
using UAssetAPI.PropertyTypes;
using Textures;
using SkiaSharp;
using System.Security.Cryptography;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeMaterial() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			NormalExport material = Exports[Asset.mainExport - 1] as NormalExport;

			DisableGeneration.Add("Metallic");
			DisableGeneration.Add("Specular");
			DisableGeneration.Add("Anisotropy");
			DisableGeneration.Add("Normal");
			DisableGeneration.Add("Tangent");
			DisableGeneration.Add("EmissiveColor");
			DisableGeneration.Add("WorldPositionOffset");
			DisableGeneration.Add("Refraction");
			DisableGeneration.Add("PixelDepthOffset");
			DisableGeneration.Add("ShadingModelFromMaterialExpression");
			DisableGeneration.Add("MaterialAttributes");

			DisableGeneration.Add("FunctionInfos");
			DisableGeneration.Add("DefaultLayers");
			DisableGeneration.Add("DefaultLayerBlends");

			if (material != null) {

				ja.Add("AssetClass", "Material");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				
				if (CIRCULAR_DEPENDENCY.Contains( GetFullName(material.ClassIndex.Index))) {
					asdata.Add("SkipDependecies", true);
				}

				PopulateCachedExpressionDataEntries(ref material.Data);
				SerializeReferencedFunctions(material.Data, out JProperty[] functions);

				JObject aodata = SerializaListOfProperties(material.Data);
				aodata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
				RefObjects = new List<int>();
				asdata.Add("AssetObjectData", aodata);
				asdata.Add(functions);
				ja.Add("AssetSerializedData", asdata);
				
				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}

		private static void SerializeReferencedFunctions(List<PropertyData> data, out JProperty[] _functions) {

			 _functions = new JProperty[3];
			JArray ReferencedFunctions = new();
			JArray MaterialLayers = new();
			JArray MaterialLayersBlends = new();

			if (FindPropertyData(data, "CachedExpressionData", out PropertyData cedata)) {
				if (FindPropertyData(((StructPropertyData)cedata).Value, "FunctionInfos", out PropertyData _parameters)) {
					PropertyData[] functionsinfo = ((ArrayPropertyData)_parameters).Value;
					foreach (PropertyData _func in functionsinfo) {
						if (FindPropertyData(((StructPropertyData)_func).Value, "Function", out PropertyData func)) {
							ReferencedFunctions.Add(GetFullName(((ObjectPropertyData)func).Value.Index));

						}
					}
				}

				if (FindPropertyData(((StructPropertyData)cedata).Value, "MaterialLayers", out PropertyData _layers)) {

					PropertyData[] layers = ((ArrayPropertyData)_layers).Value;
					foreach (ObjectPropertyData _func in layers) {
						MaterialLayers.Add(GetFullName(_func.Value.Index));
					}
				}

				if (FindPropertyData(((StructPropertyData)cedata).Value, "MaterialLayerBlends", out PropertyData _layersblend)) {
					PropertyData[] layersblends = ((ArrayPropertyData)_layersblend).Value;
					foreach (ObjectPropertyData _func in layersblends) {
						MaterialLayersBlends.Add(GetFullName(_func.Value.Index));
					}
				}

			}

			_functions[0] = new JProperty("ReferencedFunctions", ReferencedFunctions.Distinct());
			_functions[1] = new JProperty("MaterialLayers", MaterialLayers.Distinct());
			_functions[2] = new JProperty("MaterialLayerBlends", MaterialLayersBlends.Distinct());
		
        }

        private static void PopulateCachedExpressionDataEntries(ref List<PropertyData> data, int v=5) {
			var entriesname = "Entries";
			if (Asset.EngineVersion >= UE4Version.VER_UE4_26) {
				entriesname = "RuntimeEntries";
			}

			if (FindPropertyData(data, "CachedExpressionData", out PropertyData cedata)) {
				if (FindPropertyData(((StructPropertyData)cedata).Value, "Parameters", out PropertyData _parameters)) {
					List<PropertyData> parameters = ((StructPropertyData)_parameters).Value;

					List<int> fullentries = Enumerable.Range(0, v).ToList();
					List<int> entries = new List<int>();
					for (int i = 0; i < parameters.Count; i++) {
						if (parameters[i].Name.ToName() == entriesname) {
							entries.Add(parameters[i].DuplicationIndex);
						}
					}

					if (entries.Count > 0) {
						var missing = fullentries.Except(entries).ToList();
						foreach (int missed in missing) {

							parameters.Add(new StructPropertyData(new FName(entriesname), new FName("MaterialCachedParameterEntry"), missed) {
								Value = new List<PropertyData> {
									new ArrayPropertyData(new FName("NameHashes")),
									new ArrayPropertyData(new FName("ParameterInfos")),
									new ArrayPropertyData(new FName("ExpressionGuids")),
									new ArrayPropertyData(new FName("Overrides"))
								}
							});
						}
						parameters.Sort((x, y) => {
							var ret = x.Name.ToName().CompareTo(y.Name.ToName());
							if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
							return ret;
						});
					}
				}
			}

        }
    }


}