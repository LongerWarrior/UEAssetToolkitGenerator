using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.Globals;
using static CookedAssetSerializer.Serializers;

namespace CookedAssetSerializer {
	public static class Utils {
		public static UAsset Asset;
		public static List<Export> Exports => Asset.Exports;
		public static List<string> importvariables = new List<string>();
		public static Dictionary<int, int> Dict = new Dictionary<int, int>();
		public static List<int> RefObjects = new List<int>();
		public static List<string> GeneratedVariables = new List<string>();
		public static List<string> DisableGeneration = new List<string>();
		
		public static void PrintOutput(string output, string type = "debug") {
			Console.WriteLine(output);
			var filename = type == "debug" ? "debug" : "output";
			using StreamWriter sw = File.AppendText(Path.Combine(JSON_DIR, filename + "_log.txt"));
			sw.WriteLine($"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}");
		}

		public static bool SetupSerialization(out string name, out string gamepath, out string path1) {
			Dict = new();
			DisableGeneration = new();
			GeneratedVariables = new();
			RefObjects = new();

			string fullpath = Asset.FilePath;
			name = Path.GetFileNameWithoutExtension(fullpath);
			var directory = Path.GetDirectoryName(fullpath);
			var relativepath = Path.GetRelativePath(CONTENT_DIR, directory);
			if (relativepath.StartsWith(".")) {
				relativepath = "\\";
			}
			gamepath = Path.Join("\\Game", relativepath, name).Replace("\\", "/");
			path1 = Path.Join(JSON_DIR, gamepath) + ".json";

			Directory.CreateDirectory(Path.GetDirectoryName(path1));
			if (!REFRESH_ASSETS && File.Exists(path1)) return false;

			Asset = new UAsset(fullpath, GLOBAL_UE_VERSION, false);

			FixIndexes();

			return true;

		}

		public static bool FindExternalProperty(UAsset asset, FName propname, out FProperty property) {
			ClassExport export = asset.GetClassExport();
			if (export != null) {
				foreach (FProperty prop in export.LoadedProperties) {
					if (prop.Name == propname) {
						property = prop;
						return true;
					}
				}
				property = new FObjectProperty();
				Console.WriteLine("No " + propname.ToName() + "in ClassExport");
				return false;

			} else {
				property = new FObjectProperty();
				Console.WriteLine("No ClassExport");
				return false;
			}

		}

		public static bool FindPropertyData(FPackageIndex export, string propname, out PropertyData prop) {

			prop = null;
			if (export.IsExport() && export.ToExport(Asset) is NormalExport exp) {
				foreach (PropertyData property in exp.Data) {
					if (property.Name.ToName() == propname) {
						prop = property;
						return true;
                    }
                }
            }

			return false;
        }

		public static bool FindPropertyData(Export export, string propname, out PropertyData prop) {

			prop = null;
			if (export is NormalExport exp) {
				foreach (PropertyData property in exp.Data) {
					if (property.Name.ToName() == propname) {
						prop = property;
						return true;
					}
				}
			}

			return false;
		}
		public static bool FindPropertyData(List<PropertyData> list, string propname, out PropertyData prop) {

			prop = null;
			foreach (PropertyData property in list) {
				if (property.Name.ToName() == propname) {
					prop = property;
					return true;
				}
			}

			return false;
		}

		public static bool FindPropertyData(List<PropertyData> list, string propname, out PropertyData[] props) {

			props = null;
			List<PropertyData> temp = new();
			foreach (PropertyData property in list) {
				if (property.Name.ToName() == propname) {
					temp.Add(property);
				}
			}
			if (temp.Count > 0) {
				props = temp.ToArray();
				return true;
            }
			return false;
		}

		public static void FixIndexes() {
			int index = 0;
			Dict.Add(0, -1);

			for (int i = 1; i <= Asset.Imports.Count; i++) {
				//string importname = asset.Imports[i - 1].ObjectName.Value.Value;
				Dict.Add(-i, index);
				index++;
			}

			for (int i = 1; i <= Asset.Exports.Count; i++) {
				if (Asset.Exports[i - 1] is FunctionExport) {
					/*if (asset.Exports[i - 1].ObjectName.ToName().StartsWith("ExecuteUbergraph_")) {
						dict.Add(i, index);
						index++;
					}*/
				} else {
					Dict.Add(i, index);
					index++;
				}
			}
		}

		public static int GetClassIndex() {
			for (int i = 1; i <= Asset.Exports.Count; i++) {
				if (Asset.Exports[i - 1] is ClassExport) {
					return i;
				}
			}
			return 0;
		}

		public static int Index(int index) {
			if (Dict.TryGetValue(index, out int validindex)) {
				return validindex;
			} else {
				//Console.WriteLine("Non valid Import index : "+index);
				return -1;
			}
		}


		public static string GetName(int index) {
			if (index > 0) {
				return Asset.Exports[index - 1].ObjectName.ToName();
			} else if (index < 0) {
				return Asset.Imports[-index - 1].ObjectName.ToName();
			} else {
				return "";
			}
		}

		public static string GetFullName(int index, bool alt = false) {

			if (index > 0) {
				if (Asset.Exports[index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(Asset.Exports[index - 1].OuterIndex.Index);
					return parent + "." + Asset.Exports[index - 1].ObjectName.ToName();
				} else {
					return Asset.Exports[index - 1].ObjectName.ToName();
				}

			} else if (index < 0) {

				if (Asset.Imports[-index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(Asset.Imports[-index - 1].OuterIndex.Index);
					return parent + "." + Asset.Imports[-index - 1].ObjectName.ToName();
				} else {
					return Asset.Imports[-index - 1].ObjectName.ToName();
				}

			} else {
				return "";
			}
		}

		public static string GetParentName(int index) {
			if (index > 0) {
				if (Asset.Exports[index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(Asset.Exports[index - 1].OuterIndex.Index);
					return parent;
				} else {
					return "";
				}

			} else if (index < 0) {

				if (Asset.Imports[-index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(Asset.Imports[-index - 1].OuterIndex.Index);
					return parent;
				} else {
					return "";
				}

			} else {
				return "";
			}
		}

		public static bool FindProperty(int index, FName propname, out FProperty property) {
			if (index < 0) {

				//Console.WriteLine(index+" , "+GetFullName(index) + " , " + propname.ToName());

				string klass = Asset.Imports[-index - 1].ClassName.ToName();
				string owner = GetName(index);
				string parent = GetParentName(index);
				/*if (parent.StartsWith("/Game")) {
					string path = ContentDir + parent.Substring(5).Replace("/","\\")+".uasset";
					if (File.Exists(path)) {
						UAsset assetcheck = new UAsset(path, UE4Version.VER_UE4_26);
						//altasset = assetcheck;
						if (FindExternalProperty(assetcheck, propname, out FProperty prop)) {
							property = prop;
							return true;
						} else {
							property = new FObjectProperty();
							return false;
						}

                    } else {
						Console.WriteLine("No such file on disk "+"Class "+klass+ " Path " +path);
                    }
				}*/


				importvariables.Add(Asset.Imports[-index - 1].ClassName.ToName() + " " + GetFullName(index) + " " + propname.ToName());

				property = new FObjectProperty();
				return false;

			}
			Export export = Asset.Exports[index - 1];
			if (export is StructExport) {
				foreach (FProperty prop in (export as StructExport).LoadedProperties) {
					if (prop.Name == propname) {
						property = prop;
						return true;
					}
				}
			}
			property = new FObjectProperty();
			return false;
		}

		public static bool CheckDuplications(ref List<PropertyData> Data) {

			for (int i = 0; i < Data.Count; i++) {
				if (i != 0) {
					if (Data[i].DuplicationIndex > 0) {

						if (Data[i].DuplicationIndex != Data[i - 1].DuplicationIndex + 1 && Data[i].Name.ToName() == Data[i - 1].Name.ToName()) {

							Console.WriteLine("Missing property with lower duplication index  Name : " + Data[i].Name.ToName() + " Type : " + Data[i].PropertyType.ToName() + " StructType : " + (Data[i] as StructPropertyData).StructType.ToName());
							
							return false;
						}
					} else {
						continue;
					}
				} else {
					if (Data[i].DuplicationIndex > 0) {
						Console.WriteLine(" i=0  Missing property with lower duplication index  Name : " + Data[i].Name.ToName() + " Type : " + Data[i].PropertyType.ToName() + " StructType : " + (Data[i] as StructPropertyData).StructType.ToName());
						return false;
					}
				}

			}
			return true;
		}
		
		public static void ScanAssetTypes(string typeToFind = "") {
			Dictionary<string, List<string>> types = new();
			List<string> allTypes = new();

			var files = Directory.GetFiles(CONTENT_DIR, "*.uasset", SearchOption.AllDirectories);

			AssetTotal = files.Length;
			AssetCount = 0;
			foreach (var file in files) {
				var type = GetAssetType(file, GLOBAL_UE_VERSION);
				var path = "/" + Path.GetRelativePath(CONTENT_DIR, file).Replace("\\", "/");

				PrintOutput(path, "Scan");
				AssetCount++;

				if (types.ContainsKey(type)) types[type].Add(path);
				else types[type] = new List<string> { path };

				if (type == typeToFind) PrintOutput(type + " : " + path, "Scan");
			}

			PrintOutput("Find all files " + files.Length, "Scan");
			var jTypes = new JObject();
			foreach (var entry in types) {
				PrintOutput(entry.Key + " : " + entry.Value.Count, "Scan");
				jTypes.Add(entry.Key, JArray.FromObject(entry.Value));
				allTypes.Add("\"" + entry.Key + "\",");
			}

			File.WriteAllText(JSON_DIR + "\\AssetTypes.json", jTypes.ToString());
			File.WriteAllText(JSON_DIR + "\\AllTypes.txt", string.Join("\n", allTypes));
		}

		public static void SerializeAssets() {
            var files = Directory.GetFiles(CONTENT_DIR, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files) {
                Asset = new UAsset(file, GLOBAL_UE_VERSION, true);
                AssetCount++;

                if (SKIP_SERIALIZATION.Contains(Asset.assetType)) {
                    PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                    continue;
                }

                PrintOutput(file, "SerializeAssets");

                if (Asset.assetType != EAssetType.Uncategorized) {
                    switch (Asset.assetType) {
                        case EAssetType.Blueprint:
                        case EAssetType.WidgetBlueprint:
                        case EAssetType.AnimBlueprint:
                            SerializeBPAsset(false);
                            break;
                        case EAssetType.DataTable:
                            SerializeDataTable();
                            break;
                        case EAssetType.StringTable:
                            SerializeStringTable();
                            break;
                        case EAssetType.UserDefinedStruct:
                            SerializeUserDefinedStruct();
                            break;
                        case EAssetType.BlendSpaceBase:
                            SerializeBlendSpace();
                            break;
                        case EAssetType.AnimMontage:
                        case EAssetType.CameraAnim:
                        case EAssetType.LandscapeGrassType:
                        case EAssetType.MediaPlayer:
                        case EAssetType.MediaTexture:
                        case EAssetType.SubsurfaceProfile:
                            SerializeSimpleAsset(false);
                            break;
                        case EAssetType.Skeleton:
                            SerializeSkeleton();
                            break;
                        case EAssetType.MaterialParameterCollection:
                            SerializeMaterialParameterCollection();
                            break;
                        case EAssetType.PhycialMaterial:
                            SerializePhysicalMaterial();
                            break;
                        case EAssetType.Material:
                            SerializeMaterial();
                            break;
                        case EAssetType.MaterialInstanceConstant:
                            SerializeMaterialInstanceConstant();
                            break;
                        case EAssetType.UserDefinedEnum:
                            SerializeUserDefinedEnum();
                            break;
                        case EAssetType.SoundCue:
                            SerializeSoundCue();
                            break;
                        case EAssetType.Font:
                            SerializeFont();
                            break;
                        case EAssetType.FontFace:
                            SerializeFontFace();
                            break;
                        case EAssetType.CurveBase:
                            SerializeCurveBase();
                            break;
                        case EAssetType.Texture2D:
                            SerializeTexture();
                            break;
                        case EAssetType.SkeletalMesh:
                            break;
                        case EAssetType.FileMediaSource:
                            SerializeFileMediaSource();
                            break;
                        case EAssetType.StaticMesh:
                            SerializeStaticMesh();
                            break;
                    }
                } else {
                    var aType = GetFullName(Exports[Asset.mainExport - 1].ClassIndex.Index);
                    if (SIMPLE_ASSETS.Contains(aType)) SerializeSimpleAsset();
                }
            }
        }

        public static void GetCookedAssets(bool copy = true) {
            var files = Directory.GetFiles(CONTENT_DIR, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files) {
                var uexpFile = Path.ChangeExtension(file, "uexp");
                var ubulkFile = Path.ChangeExtension(file, "ubulk");
                var type = GetAssetType(file, GLOBAL_UE_VERSION);

                AssetCount++;
                if (!TYPES_TO_COPY.Contains(type)) {
                    PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                    continue;
                }

                var relativePath = Path.GetRelativePath(CONTENT_DIR, file);
                var newPath = Path.Combine(OUTPUT_DIR, relativePath);

                PrintOutput(newPath, "GetCookedAssets");

                Directory.CreateDirectory(Path.GetDirectoryName(newPath) ?? string.Empty);
                if (copy) File.Copy(file, newPath, true);
                else File.Move(file, newPath, true);

                if (File.Exists(uexpFile)) {
                    if (copy) File.Copy(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                    else File.Move(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                }

                if (!File.Exists(ubulkFile)) continue;
                if (copy) File.Copy(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
                else File.Move(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
            }
        }

		public static string GetAssetType(string file, UE4Version version) {
			string name = Path.GetFileNameWithoutExtension(file).ToLower();
			Asset = new UAsset(file, version, true);
			if (Asset.Exports.Count == 1) {
				return GetFullName(Asset.Exports[0].ClassIndex.Index);
			} else {
				List<Export> exportnames = new();
				List<Export> isasset = new();
				foreach (Export exp in Asset.Exports) {
					if (exp.ObjectName.ToName().ToLower() == name + "_c") {
						exportnames.Add(exp);
					}
					if (exp.bIsAsset) {
						isasset.Add(exp);
                    }
				}
				if (exportnames.Count == 0) {
					foreach (Export exp in Asset.Exports) {
						if (exp.ObjectName.ToName().ToLower() == name) {
							exportnames.Add(exp);
						}
					}
				}

				if (exportnames.Count == 1) {
					return GetFullName(exportnames[0].ClassIndex.Index);
				} else {
					if (isasset.Count == 1) {
						return GetFullName(isasset[0].ClassIndex.Index);
					} else {
						Console.WriteLine("Couldn't identify asset type : " + file);
						return "null";
					}
				}
			}
		}

		public static JObject GuidToJson(Guid Value) {
			JObject res = new JObject();
			uint[] guid = Value.ToUnsignedInts();
			res.Add(new JProperty("A", (int)guid[0]));
			res.Add(new JProperty("B", (int)guid[1]));
			res.Add(new JProperty("C", (int)guid[2]));
			res.Add(new JProperty("D", (int)guid[3]));
			return res;
		}

	}
}