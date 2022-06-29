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
        public static void SerializeFont() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            DisableGeneration.Add("Textures");
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not FontExport font) return;
            ja.Add("AssetClass", "Font");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();
            var aoData = new JObject();

            var bOffline = FindPropertyData(font, "FontCacheType", out var prop);

            if (bOffline) {
                var fontCacheType = (EnumPropertyData)prop;
                if (fontCacheType.Value.ToName() == "EFontCacheType::Runtime") {
                    asData.Add("IsRuntimeFont", true);


                    List<FontDataPropertyData> allFonts = new();
                    List<string> allFontsRef = new();

                    if (FindPropertyData(font, "CompositeFont", out var _cfont)) {
                        var cfont = (StructPropertyData)_cfont;
                        if (FindPropertyData(cfont.Value, "DefaultTypeface", out PropertyData _deffontface)) {
                            var deffontface = (StructPropertyData)_deffontface;
                            if (FindPropertyData(deffontface.Value, "Fonts", out PropertyData _fonts)) {
                                var fonts = (ArrayPropertyData)_fonts;
                                foreach (StructPropertyData _font in fonts.Value)
                                    if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
                                        var fontData = (StructPropertyData)_fontdata;
                                        allFonts.Add((FontDataPropertyData)fontData.Value[0]);
                                    }
                            }
                        }

                        if (FindPropertyData(cfont.Value, "FallbackTypeface", out PropertyData _fallbackfontface)) {
                            var fallBackFontFace = (StructPropertyData)_fallbackfontface;
                            if (FindPropertyData(fallBackFontFace.Value, "Typeface", out PropertyData _typeface)) {
                                var typeface = (StructPropertyData)_typeface;
                                if (FindPropertyData(typeface.Value, "Fonts", out PropertyData _fonts)) {
                                    var fonts = (ArrayPropertyData)_fonts;
                                    foreach (StructPropertyData _font in fonts.Value)
                                        if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
                                            var fontData = (StructPropertyData)_fontdata;
                                            allFonts.Add((FontDataPropertyData)fontData.Value[0]);
                                        }
                                }
                            }
                        }

                        if (FindPropertyData(cfont.Value, "SubTypefaces", out PropertyData _subtypefaces)) {
                            var subTypeFaces = (ArrayPropertyData)_subtypefaces;
                            foreach (StructPropertyData _typefaces in subTypeFaces.Value)
                                if (FindPropertyData(_typefaces.Value, "Typeface", out PropertyData _typeface)) {
                                    var typeface = (StructPropertyData)_typeface;
                                    if (!FindPropertyData(typeface.Value, "Fonts", out PropertyData _fonts)) continue;
                                    var fonts = (ArrayPropertyData)_fonts;
                                    foreach (StructPropertyData _font in fonts.Value)
                                        if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
                                            var fontData = (StructPropertyData)_fontdata;
                                            allFonts.Add((FontDataPropertyData)fontData.Value[0]);
                                        }
                                }
                        }
                    }

                    foreach (var fontRef in allFonts)
                        if (fontRef.Value.LocalFontFaceAsset.Index < 0)
                            allFontsRef.Add(GetParentName(fontRef.Value.LocalFontFaceAsset.Index));
                        else
                            Console.WriteLine("exportfontface");

                    asData.Add("ReferencedFontFacePackages", JArray.FromObject(allFontsRef.Distinct<string>()));
                } else {
                    asData.Add("IsOfflineFont", true);
                }
            } else {
                asData.Add("IsOfflineFont", true);
            }

            ja.Add("AssetSerializedData", asData);
            var jData = SerializaListOfProperties(font.Data);
            asData.Add("AssetObjectData", jData);
            jData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }


    //		for (const FTypeface* Typeface : AllTypefaces) {
    //		for (const FTypefaceEntry& TypefaceEntry : Typeface->Fonts) {
    //			const UObject* AssetObject = TypefaceEntry.Font.GetFontFaceAsset();

    //			if (AssetObject != NULL) {
    //				const FString PackageName = AssetObject->GetOutermost()->GetName();
    //DependencyPackageNames.Add(MakeShareable(new FJsonValueString(PackageName)));
    //			}
    //		}
    //	}
}
