using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Program;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void SerializeBPAsset(bool dummy) {
            if (!SetupSerialization(out var name, out var gamepath, out var path1)) return;

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
            var mainobject = Exports[Asset.mainExport - 1] as ClassExport;

            if (mainobject != null) {
                var classname = mainobject.ClassIndex.ToImport(Asset).ObjectName.ToName();
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
                        ja.Add("AssetClass", "UknownType");
                        break;
                }

                ja.Add("AssetPackage", gamepath);
                ja.Add("AssetName", name);
                var asdata = new JObject();

                asdata.Add("SuperStruct", Index(mainobject.SuperIndex.Index));

                var Children = new JArray();
                var functions = new List<FunctionExport>();
                foreach (var package in mainobject.Children)
                    if (Exports[package.Index - 1] is FunctionExport func)
                        Children.Add(SerializeFunction(func));
                if (Asset.assetType != EAssetType.AnimBlueprint) {
                    if (!dummy)
                        asdata.Add("Children", Children);
                    else
                        asdata.Add("Children", new JArray());
                } else {
                    asdata.Add("Children", new JArray());
                }

                var ChildProperties = new JArray();

                foreach (var property in mainobject.LoadedProperties) ChildProperties.Add(SerializeProperty(property));

                if (!dummy)
                    asdata.Add("ChildProperties", ChildProperties);
                else
                    asdata.Add("ChildProperties", new JArray());


                asdata.Add("ClassFlags", ((long)mainobject.ClassFlags).ToString());
                asdata.Add("ClassWithin", Index(mainobject.ClassWithin.Index));
                asdata.Add("ClassConfigName", mainobject.ClassConfigName.ToName());
                asdata.Add("Interfaces", SerializeInterfaces(mainobject.Interfaces.ToList()));
                asdata.Add("ClassDefaultObject", Index(mainobject.ClassDefaultObject.Index));
                ja.Add("AssetSerializedData", asdata);
                asdata.Add(SerializeData(mainobject.Data, true));

                CollectGeneratedVariables(mainobject);

                if (!dummy)
                    asdata.Add("GeneratedVariableNames", JArray.FromObject(GeneratedVariables));
                else
                    asdata.Add("GeneratedVariableNames", new JArray());


                ja.Add(ObjectHierarchy(Asset, false));
                File.WriteAllText(path1, ja.ToString());
            }
        }

        private static void FixMovieSceneSections() {
            foreach (NormalExport normal in Asset.Exports) {
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

        private static void PopulateMovieScene2DTransformSection(ref List<PropertyData> Data, string propname,
            int v = 2) {
            if (FindPropertyData(Data, propname, out PropertyData[] _parameters)) {
                var fullentries = Enumerable.Range(0, v).ToList();
                var entries = new List<int>();
                for (var i = 0; i < _parameters.Length; i++) entries.Add(_parameters[i].DuplicationIndex);

                var missing = fullentries.Except(entries).ToList();
                foreach (var missed in missing)
                    Data.Add(new StructPropertyData(new FName(propname), new FName("MovieSceneFloatChannel"), missed) {
                        Value = new List<PropertyData> { new MovieSceneFloatChannelPropertyData() }
                    });
                Data.Sort((x, y) => {
                    var ret = x.Name.ToName().CompareTo(y.Name.ToName());
                    if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
                    return ret;
                });
            }
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
