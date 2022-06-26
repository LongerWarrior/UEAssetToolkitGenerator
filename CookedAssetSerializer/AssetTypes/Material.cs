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
        public static void SerializeMaterial() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();
            var material = Exports[Asset.mainExport - 1] as NormalExport;

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

            if (material == null) return;
            ja.Add("AssetClass", "Material");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            PopulateCachedExpressionDataEntries(ref material.Data);
            SerializeReferencedFunctions(material.Data, out var functions);

            var aoData = SerializaListOfProperties(material.Data);
            aoData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            RefObjects = new List<int>();
            asData.Add("AssetObjectData", aoData);
            asData.Add(functions);
            ja.Add("AssetSerializedData", asData);

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }

        private static void SerializeReferencedFunctions(List<PropertyData> data, out JProperty[] _functions) {
            _functions = new JProperty[3];
            JArray referencedFunctions = new();
            JArray materialLayers = new();
            JArray materialLayersBlends = new();

            if (FindPropertyData(data, "CachedExpressionData", out PropertyData ceData)) {
                if (FindPropertyData(((StructPropertyData)ceData).Value, "FunctionInfos",
                        out PropertyData _parameters)) {
                    var functionsInfo = ((ArrayPropertyData)_parameters).Value;
                    foreach (var _func in functionsInfo)
                        if (FindPropertyData(((StructPropertyData)_func).Value, "Function", out PropertyData func))
                            referencedFunctions.Add(GetFullName(((ObjectPropertyData)func).Value.Index));
                }

                if (FindPropertyData(((StructPropertyData)ceData).Value, "MaterialLayers", out PropertyData _layers)) {
                    var layers = ((ArrayPropertyData)_layers).Value;
                    foreach (ObjectPropertyData _func in layers) materialLayers.Add(GetFullName(_func.Value.Index));
                }

                if (FindPropertyData(((StructPropertyData)ceData).Value, "MaterialLayerBlends",
                        out PropertyData _layersblend)) {
                    var layersBlends = ((ArrayPropertyData)_layersblend).Value;
                    foreach (ObjectPropertyData _func in layersBlends)
                        materialLayersBlends.Add(GetFullName(_func.Value.Index));
                }
            }

            _functions[0] = new JProperty("ReferencedFunctions", referencedFunctions.Distinct());
            _functions[1] = new JProperty("MaterialLayers", materialLayers.Distinct());
            _functions[2] = new JProperty("MaterialLayerBlends", materialLayersBlends.Distinct());
        }

        private static void PopulateCachedExpressionDataEntries(ref List<PropertyData> data, int v = 5) {
            var entriesName = "Entries";
            if (Asset.EngineVersion >= UE4Version.VER_UE4_26) entriesName = "RuntimeEntries";

            if (!FindPropertyData(data, "CachedExpressionData", out PropertyData ceData)) return;
            if (!FindPropertyData(((StructPropertyData)ceData).Value, "Parameters", out PropertyData _parameters)) return;
            var parameters = ((StructPropertyData)_parameters).Value;

            var fullEntries = Enumerable.Range(0, v).ToList();
            var entries = (from t in parameters where t.Name.ToName() == entriesName select t.DuplicationIndex).ToList();

            if (entries.Count <= 0) return;
            var missing = fullEntries.Except(entries).ToList();
            foreach (var missed in missing) {
               parameters.Add(new StructPropertyData(new FName(entriesName),
                   new FName("MaterialCachedParameterEntry"), missed) {
                   Value = new List<PropertyData> {
                       new ArrayPropertyData(new FName("NameHashes")),
                       new ArrayPropertyData(new FName("ParameterInfos")),
                       new ArrayPropertyData(new FName("ExpressionGuids")),
                       new ArrayPropertyData(new FName("Overrides"))
                   }
               });
            }

            parameters.Sort((x, y) => {
                var ret = string.Compare(x.Name.ToName(), y.Name.ToName(), StringComparison.Ordinal);
                if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                return ret;
            });
        }
    }
}
