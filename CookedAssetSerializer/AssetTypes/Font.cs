using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using Textures;
using SkiaSharp;
using System.Security.Cryptography;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeFont() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			DisableGeneration.Add("Textures");
			JObject ja = new JObject();
			FontExport font = Exports[Asset.mainExport-1] as FontExport;

			if (font != null) {

				ja.Add("AssetClass", "Font");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				JObject aodata = new JObject();

				var boffline = FindPropertyData(font, "FontCacheType", out PropertyData prop);

				if (boffline) {
					EnumPropertyData fontcachetype = (EnumPropertyData)prop;
					if (fontcachetype.Value.ToName() == "EFontCacheType::Runtime") {
						asdata.Add("IsRuntimeFont", true);


						List<FontDataPropertyData> allfonts = new();
						List<string> allfontsref = new();

						if (FindPropertyData(font, "CompositeFont", out PropertyData _cfont)) {
							StructPropertyData cfont = (StructPropertyData)_cfont;
							if (FindPropertyData(cfont.Value, "DefaultTypeface", out PropertyData _deffontface)) {
								StructPropertyData deffontface = (StructPropertyData)_deffontface;
								if (FindPropertyData(deffontface.Value, "Fonts", out PropertyData _fonts)) {
									ArrayPropertyData fonts = (ArrayPropertyData)_fonts;
                                    foreach (StructPropertyData _font in fonts.Value) {
										if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
											StructPropertyData fontdata = (StructPropertyData)_fontdata;
											allfonts.Add((FontDataPropertyData)fontdata.Value[0]);
										}
									}

								}

							}

							if (FindPropertyData(cfont.Value, "FallbackTypeface", out PropertyData _fallbackfontface)) {
								StructPropertyData fallbackfontface = (StructPropertyData)_fallbackfontface;
								if (FindPropertyData(fallbackfontface.Value, "Typeface", out PropertyData _typeface)) {
									StructPropertyData typeface = (StructPropertyData)_typeface;
									if (FindPropertyData(typeface.Value, "Fonts", out PropertyData _fonts)) {
										ArrayPropertyData fonts = (ArrayPropertyData)_fonts;
										foreach (StructPropertyData _font in fonts.Value) {
											if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
												StructPropertyData fontdata = (StructPropertyData)_fontdata;
												allfonts.Add((FontDataPropertyData)fontdata.Value[0]);
											}
										}
									}
								}

							}

							if (FindPropertyData(cfont.Value, "SubTypefaces", out PropertyData _subtypefaces)) {
								ArrayPropertyData subtypefaces = (ArrayPropertyData)_subtypefaces;
								foreach (StructPropertyData _typefaces in subtypefaces.Value) {
									if (FindPropertyData(_typefaces.Value, "Typeface", out PropertyData _typeface)) {
										StructPropertyData typeface = (StructPropertyData)_typeface;
										if (FindPropertyData(typeface.Value, "Fonts", out PropertyData _fonts)) {
											ArrayPropertyData fonts = (ArrayPropertyData)_fonts;
											foreach (StructPropertyData _font in fonts.Value) {
												if (FindPropertyData(_font.Value, "Font", out PropertyData _fontdata)) {
													StructPropertyData fontdata = (StructPropertyData)_fontdata;
													allfonts.Add((FontDataPropertyData)fontdata.Value[0]);
												}
											}

										}
									}
								}

							}
						}

						foreach (FontDataPropertyData fontref in allfonts) {
							if (fontref.Value.LocalFontFaceAsset.Index < 0) {
								allfontsref.Add(GetParentName(fontref.Value.LocalFontFaceAsset.Index));
                            } else {
								Console.WriteLine("exportfontface");
                            }
                        } 

						asdata.Add("ReferencedFontFacePackages", JArray.FromObject(allfontsref.Distinct<string>()));
					} else {
						asdata.Add("IsOfflineFont", true);
					}
				}/* else {
					asdata.Add("IsOfflineFont", true);
				}*/

				ja.Add("AssetSerializedData", asdata);
				JObject jdata = SerializaListOfProperties(font.Data);
				asdata.Add("AssetObjectData", jdata);
				jdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
				


				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
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