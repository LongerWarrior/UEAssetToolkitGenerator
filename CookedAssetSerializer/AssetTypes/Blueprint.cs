using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void SerializeBPAsset(bool dummy) {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;

            DisableGeneration.Add("UberGraphFunction");
            DisableGeneration.Add("CookedComponentInstancingData");
            DisableGeneration.Add("NumReplicatedProperties");
            DisableGeneration.Add("bHasNativizedParent");
            DisableGeneration.Add("bHasCookedComponentInstancingData");
            if (Asset.assetType == EAssetType.WidgetBlueprint) {
                DisableGeneration.Add("bHasScriptImplementedTick");
                DisableGeneration.Add("bHasScriptImplementedPaint");
            }

            var ja = new JObject();
            if (Exports[Asset.mainExport - 1] is not ClassExport mainObject) return;
            var classname = mainObject.ClassIndex.ToImport(Asset).ObjectName.ToName();
            switch (classname) {
                case "BlueprintGeneratedClass":
                    ja.Add("AssetClass", "Blueprint");
                    break;
                case "WidgetBlueprintGeneratedClass":
                    ja.Add("AssetClass", "WidgetBlueprint");
                    FixMovieSceneSections();
                    break;
                case "AnimBlueprintGeneratedClass":
                    ja.Add("AssetClass", "AnimBlueprint");
                    break;
                default:
                    ja.Add("AssetClass", "UnknownType");
                    break;
            }

            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject { { "SuperStruct", Index(mainObject.SuperIndex.Index) } };
            var children = new JArray();
            var functions = new List<FunctionExport>();
            foreach (var package in mainObject.Children)
                if (Exports[package.Index - 1] is FunctionExport func)
                    children.Add(SerializeFunction(func));
            if (Asset.assetType != EAssetType.AnimBlueprint) {
                asData.Add("Children", !dummy ? children : new JArray());
            } else {
                asData.Add("Children", new JArray());
            }

            var childProperties = new JArray();

            foreach (var property in mainObject.LoadedProperties) childProperties.Add(SerializeProperty(property));

            asData.Add("ChildProperties", !dummy ? childProperties : new JArray());
            asData.Add("ClassFlags", ((long)mainObject.ClassFlags).ToString());
            asData.Add("ClassWithin", Index(mainObject.ClassWithin.Index));
            asData.Add("ClassConfigName", mainObject.ClassConfigName.ToName());
            asData.Add("Interfaces", SerializeInterfaces(mainObject.Interfaces.ToList()));
            asData.Add("ClassDefaultObject", Index(mainObject.ClassDefaultObject.Index));
            ja.Add("AssetSerializedData", asData);
            asData.Add(SerializeData(mainObject.Data, true));

            CollectGeneratedVariables(mainObject);

            asData.Add("GeneratedVariableNames", !dummy ? JArray.FromObject(GeneratedVariables) : new JArray());
            ja.Add(ObjectHierarchy(Asset, false));
            File.WriteAllText(path1, ja.ToString());
        }

        private static void FixMovieSceneSections() {
            foreach (var normal in Asset.Exports.Cast<NormalExport>()) {
                if (normal.ClassIndex.Index < 0 && normal.ClassIndex.ToImport(Asset).ObjectName.ToName() ==
                    "MovieScene2DTransformSection") {
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Translation");
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Scale");
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Shear");
                }

                if (normal.ClassIndex.Index < 0 &&
                    normal.ClassIndex.ToImport(Asset).ObjectName.ToName() == "MovieSceneVectorSection")
                    PopulateMovieScene2DTransformSection(ref normal.Data, "Curves", 4);
            }
        }

        private static void PopulateMovieScene2DTransformSection(ref List<PropertyData> data, string propName, int v = 2) {
            if (!FindPropertyData(data, propName, out PropertyData[] _parameters)) return;
            var fullEntries = Enumerable.Range(0, v).ToList();
            var entries = _parameters.Select(t => t.DuplicationIndex).ToList();
            var missing = fullEntries.Except(entries).ToList();

            data.AddRange(missing.Select(missed => new StructPropertyData(new FName(propName),
                new FName("MovieSceneFloatChannel"), missed)
                { Value = new List<PropertyData> { new MovieSceneFloatChannelPropertyData() } }));

            data.Sort((x, y) => {
                var ret = string.Compare(x.Name.ToName(), y.Name.ToName(), StringComparison.Ordinal);
                if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                return ret;
            });
        }
    }


//	//Serialize mapping of movie scene events to their bound functions
//	//Since we cannot serialize raw compiled function pointers, we need to just record function names
//	TSharedPtr<FJsonObject> MovieSceneEventTriggerSectionFunctions = MakeShareable(new FJsonObject);

//    for (const UWidgetAnimation* Animation : Asset->Animations) {
//        ForEachObjectWithOuter(Animation, [&] (UObject* Object){
//		if (UMovieSceneEventTriggerSection * EventSection = Cast<UMovieSceneEventTriggerSection>(Object)) {
//			FMovieSceneEventChannel & EventChannel = EventSection->EventChannel;
//			TArray<TSharedPtr<FJsonValue>> EventChannelValues;

//			for (int32 i = 0; i < EventChannel.GetNumKeys(); i++) {
//				const FMovieSceneEvent&MovieSceneEvent = EventChannel.GetData().GetValues()[i];

//				const TSharedPtr<FJsonObject> Value = MakeShareable(new FJsonObject);
//				Value->SetNumberField(TEXT("KeyIndex"), i);
//				Value->SetStringField(TEXT("FunctionName"), MovieSceneEvent.Ptrs.Function->GetName());
//				Value->SetStringField(TEXT("BoundObjectProperty"), MovieSceneEvent.Ptrs.BoundObjectProperty.ToString());

//				EventChannelValues.Add(MakeShareable(new FJsonValueObject(Value)));
//			}
//			MovieSceneEventTriggerSectionFunctions->SetArrayField(EventSection->GetName(), EventChannelValues);
//		}
//	});
//    }
//Data->SetObjectField(TEXT("MovieSceneEventTriggerSectionFunctions"), MovieSceneEventTriggerSectionFunctions);
}
