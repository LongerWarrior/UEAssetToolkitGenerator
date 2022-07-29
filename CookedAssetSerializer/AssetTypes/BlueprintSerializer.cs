using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class BlueprintSerializer : Serializer<ClassExport>
    {
        public BlueprintSerializer(Settings assetSettings)
        {
            Settings = assetSettings;
        }
        
        public void SerializeAsset(bool dummy)
        {
            if (!SetupSerialization()) return;
            
            // Disable generation for a bunch of types
            DisableGeneration.Add("UberGraphFunction");
            DisableGeneration.Add("CookedComponentInstancingData");
            DisableGeneration.Add("NumReplicatedProperties");
            DisableGeneration.Add("bHasNativizedParent");
            DisableGeneration.Add("bHasCookedComponentInstancingData");
            if (Asset.assetType == EAssetType.WidgetBlueprint) 
            {
                DisableGeneration.Add("bHasScriptImplementedTick");
                DisableGeneration.Add("bHasScriptImplementedPaint");
            }

            SetupAssetInfo();

            // Write the class name depending on the type of blueprint
            switch (ClassName) 
            {
                case "BlueprintGeneratedClass": ClassName = "Blueprint"; break;
                case "WidgetBlueprintGeneratedClass": ClassName = "WidgetBlueprint"; FixMovieSceneSections(); break;
                case "AnimBlueprintGeneratedClass": ClassName = "AnimBlueprint"; break;
                default: ClassName = "UknownType"; break;
            }
            
            // Generate the extra header info required by blueprints
            var extraHeaderInfo = new JObject
            {
                {"AssetPackage", AssetPath},
                {"AssetName", AssetName}
            };
            SerializeHeaders(extraHeaderInfo);

            // Start the AssetSerializedData object with blueprint's required SuperStruct field
            AssetData.Add("SuperStruct", Index(ClassExport.SuperIndex.Index, Dict));

            // Serialize the blueprint's functions
            var assetChildren = new JArray();
            foreach (var package in ClassExport.Children) 
            {
                if (Asset.Exports[package.Index - 1] is FunctionExport func)
                {
                    assetChildren.Add(SerializeFunction(func, AssetInfo));
                }
            }
            
            // Serialize the blueprint's children
            if (Asset.assetType != EAssetType.AnimBlueprint) AssetData.Add("Children", !dummy ? assetChildren : new JArray());
            else AssetData.Add("Children", new JArray());

            // Serialize the blueprint's properties
            var childProperties = new JArray();
            foreach (var property in ClassExport.LoadedProperties)
            {
                childProperties.Add(SerializeProperty(property, AssetInfo));
            }
            AssetData.Add("ChildProperties", !dummy ? childProperties : new JArray());

            // Serialize the blueprint's class data/interfaces
            AssetData.Add("ClassFlags", ((Int64)ClassExport.ClassFlags).ToString());
            AssetData.Add("ClassWithin", Index(ClassExport.ClassWithin.Index, Dict));
            AssetData.Add("ClassConfigName", ClassExport.ClassConfigName.ToName());
            AssetData.Add("Interfaces", SerializeInterfaces(ClassExport.Interfaces.ToList(), AssetInfo));
            AssetData.Add("ClassDefaultObject", Index(ClassExport.ClassDefaultObject.Index, Dict));
            
            AssignAssetSerializedData();
            
            AssetData.Add(SerializeData(ClassExport.Data, AssetInfo, ref RefObjects));

            // Write the generated variables, if not a dummy serialization
            GeneratedVariables = CollectGeneratedVariables(ClassExport, AssetInfo);
            AssetData.Add("GeneratedVariableNames", !dummy ? JArray.FromObject(GeneratedVariables) : new JArray());
            
            WriteJSONOut(ObjectHierarchy(AssetInfo, ref RefObjects));
        }
        
        private void FixMovieSceneSections() {
            foreach(var export in Asset.Exports) {
                var normal = (NormalExport) export;
                if (normal.ClassIndex.Index < 0 && normal.ClassIndex.ToImport(Asset).ObjectName.ToName() == "MovieScene2DTransformSection") {
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Translation");
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Scale");
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Shear");
                }
                if (normal.ClassIndex.Index < 0 && normal.ClassIndex.ToImport(Asset).ObjectName.ToName() == "MovieSceneVectorSection") {
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Curves", 4);
                }
            }
        }

        private void PopulateMovieScene2DTransformSection(ref List<PropertyData> data, string propName, int v = 2)
        {
            if (!FindPropertyData(data, propName, out PropertyData[] parameters)) return;
            
            List<int> fullEntries = Enumerable.Range(0, v).ToList();
            List<int> entries = parameters.Select(t => t.DuplicationIndex).ToList();
            var missing = fullEntries.Except(entries).ToList();
            data.AddRange(missing.Select(missed => new StructPropertyData(new FName(propName), 
                new FName("MovieSceneFloatChannel"), missed) 
                {Value = new List<PropertyData> {new MovieSceneFloatChannelPropertyData()}}));
            data.Sort((x, y) => {
                var ret = String.Compare(x.Name.ToName(), y.Name.ToName(), StringComparison.Ordinal);
                if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                return ret;
            });
        }
    }
}