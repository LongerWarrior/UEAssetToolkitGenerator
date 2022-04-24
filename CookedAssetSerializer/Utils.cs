using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.Serializers;

namespace CookedAssetSerializer {
	public static class Utils {


		public static UAsset asset;
		public static List<Export> exports => asset.Exports;
		public static List<string> importvariables = new List<string>();
		public static Dictionary<int, int> dict = new Dictionary<int, int>();
		public static List<int> refobjects = new List<int>();
		public static List<string> GeneratedVariables = new List<string>();
		public static List<string> DisableGeneration = new List<string>();

		public static string ContentDir = @"C:\Games\DRGNEW\FSD\Content";
		public static string JsonDir = @"C:\Games\DRGNEW\FSD\Json";

		public static void SetupSerialization(out string name, out string gamepath, out string path1) {
			dict = new();
			DisableGeneration = new();
			GeneratedVariables = new();
			refobjects = new();

			FixIndexes();

			string fullpath = asset.FilePath;
			name = Path.GetFileNameWithoutExtension(fullpath);
			var directory = Path.GetDirectoryName(fullpath);
			var relativepath = Path.GetRelativePath(ContentDir, directory);
			gamepath = Path.Join("\\Game", relativepath, name).Replace("\\", "/");
			path1 = Path.Join(JsonDir, gamepath) + ".json";

			Directory.CreateDirectory(Path.GetDirectoryName(path1));
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
			if (export.IsExport() && export.ToExport(asset) is NormalExport exp) {
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

		public static void FixIndexes() {
			int index = 0;
			dict.Add(0, -1);

			for (int i = 1; i <= asset.Imports.Count; i++) {
				//string importname = asset.Imports[i - 1].ObjectName.Value.Value;
				dict.Add(-i, index);
				index++;
			}

			for (int i = 1; i <= asset.Exports.Count; i++) {
				if (asset.Exports[i - 1] is FunctionExport) {
					/*if (asset.Exports[i - 1].ObjectName.ToName().StartsWith("ExecuteUbergraph_")) {
						dict.Add(i, index);
						index++;
					}*/
				} else {
					dict.Add(i, index);
					index++;
				}
			}
		}

		public static int GetClassIndex() {
			for (int i = 1; i <= asset.Exports.Count; i++) {
				if (asset.Exports[i - 1] is ClassExport) {
					return i;
				}
			}
			return 0;
		}

		public static int Index(int index) {
			if (dict.TryGetValue(index, out int validindex)) {
				return validindex;
			} else {
				//Console.WriteLine("Non valid Import index : "+index);
				return -1;
			}
		}


		public static string GetName(int index) {
			if (index > 0) {
				return asset.Exports[index - 1].ObjectName.ToName();
			} else if (index < 0) {
				return asset.Imports[-index - 1].ObjectName.ToName();
			} else {
				return "";
			}
		}

		public static string GetFullName(int index, bool alt = false) {

			if (index > 0) {
				if (asset.Exports[index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index);
					return parent + "." + asset.Exports[index - 1].ObjectName.ToName();
				} else {
					return asset.Exports[index - 1].ObjectName.ToName();
				}

			} else if (index < 0) {

				if (asset.Imports[-index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index);
					return parent + "." + asset.Imports[-index - 1].ObjectName.ToName();
				} else {
					return asset.Imports[-index - 1].ObjectName.ToName();
				}

			} else {
				return "";
			}
		}

		public static string GetParentName(int index) {
			if (index > 0) {
				if (asset.Exports[index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index);
					return parent;
				} else {
					return "";
				}

			} else if (index < 0) {

				if (asset.Imports[-index - 1].OuterIndex.Index != 0) {
					string parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index);
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

				string klass = asset.Imports[-index - 1].ClassName.ToName();
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


				importvariables.Add(asset.Imports[-index - 1].ClassName.ToName() + " " + GetFullName(index) + " " + propname.ToName());

				property = new FObjectProperty();
				return false;

			}
			Export export = asset.Exports[index - 1];
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
						Data.Insert(0, new StructPropertyData(Data[i].Name, (Data[i] as StructPropertyData).StructType, 0));
						(Data[0] as StructPropertyData).Value = new List<PropertyData> { new MovieSceneFloatChannelPropertyData() };
						return false;
					}
				}

			}
			return true;
		}


		public static void ScanAssetTypes(string[] files, UE4Version version, string typetofind = "") {
			
			Dictionary<string, List<string>> types = new();

			foreach (string file in files) {

				string type = GetAssetType(file, version);
				var path = "/" + Path.GetRelativePath(ContentDir, file).Replace("\\", "/");

				if (types.ContainsKey(type)) {
					types[type].Add(path); 
				} else {
					types[type] = new List<string> { path };
				}

				if (type == typetofind) {
					Console.WriteLine(type + " : " + path);
				}

			}
			Console.WriteLine("Find all files " + files.Length);
			JObject jtypes = new JObject(); 
			foreach (KeyValuePair<string, List<string>> entry in types) {
				Console.WriteLine(entry.Key + " : " + entry.Value.Count);
				jtypes.Add(entry.Key, JArray.FromObject(entry.Value)); 
			}
			File.WriteAllText(ContentDir+"\\AssetTypes.json", jtypes.ToString());
		}
		public static void MoveAssets(string dirfrom, string dirto, List<string> types, UE4Version version, bool copy = true) {

			string[] files = Directory.GetFiles(dirfrom, "*.uasset", SearchOption.AllDirectories);

			foreach (string file in files) {
				var uexpfile = Path.ChangeExtension(file, "uexp");
				var ubulkfile = Path.ChangeExtension(file, "ubulk");
				string type = GetAssetType(file, version);
				if (types.Contains(type)) {
					var relativepath = Path.GetRelativePath(dirfrom, file);
					var newpath = Path.Combine(dirto, relativepath);
					if (copy) {
						Directory.CreateDirectory(Path.GetDirectoryName(newpath));
						File.Copy(file, newpath,true);
						if (File.Exists(uexpfile)) {
							File.Copy(uexpfile, Path.ChangeExtension(newpath, "uexp"), true);
                        }
						if (File.Exists(ubulkfile)) {
							File.Copy(ubulkfile, Path.ChangeExtension(newpath, "ubulk"), true);
						}
					}
				}

			}
		}

		public static string GetAssetType(string file, UE4Version version) {
			string name = Path.GetFileNameWithoutExtension(file).ToLower();
			asset = new UAsset(file, version, true);
			if (asset.Exports.Count == 1) {
				return GetFullName(asset.Exports[0].ClassIndex.Index);
			} else {
				List<Export> exportnames = new();
				List<Export> isasset = new();
				foreach (Export exp in asset.Exports) {
					if (exp.ObjectName.ToName().ToLower() == name + "_c") {
						exportnames.Add(exp);
					}
					if (exp.bIsAsset) {
						isasset.Add(exp);
                    }
				}
				if (exportnames.Count == 0) {
					foreach (Export exp in asset.Exports) {
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
