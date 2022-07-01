using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.Globals;
using static CookedAssetSerializer.Serializers;

namespace CookedAssetSerializer {
    public class Utils {
        public static UAsset Asset;
        public static List<Export> Exports => Asset.Exports;
        private static List<string> import_variables = new();
        public static Dictionary<int, int> Dict = new();
        public static List<int> RefObjects = new();
        public static List<string> GeneratedVariables = new();
        public static List<string> DisableGeneration = new();

        public static void PrintOutput(string output, string type = "debug") {
            Console.WriteLine(output);
            var filename = type == "debug" ? "debug" : "output";
            using StreamWriter sw = File.AppendText(Path.Combine(JSON_DIR, filename + "_log.txt"));
            sw.WriteLine($"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}");
        }

        public static bool SetupSerialization(out string name, out string gamePath, out string path1) {
            Dict = new Dictionary<int, int>();
            DisableGeneration = new List<string>();
            GeneratedVariables = new List<string>();
            RefObjects = new List<int>();

            string fullPath = Asset.FilePath;
            name = Path.GetFileNameWithoutExtension(fullPath);
            var directory = Path.GetDirectoryName(fullPath);
            var relativePath = Path.GetRelativePath(CONTENT_DIR, directory ?? string.Empty);
            if (relativePath.StartsWith(".")) relativePath = "\\";
            gamePath = Path.Join("\\Game", relativePath, name).Replace("\\", "/");
            path1 = Path.Join(JSON_DIR, gamePath) + ".json";

            Directory.CreateDirectory(Path.GetDirectoryName(path1) ?? string.Empty);
            if (!REFRESH_ASSETS && File.Exists(path1)) return false;
            Asset = new UAsset(fullPath, GLOBAL_UE_VERSION, false);
            FixIndexes();

            return true;
        }

        public static bool FindExternalProperty(UAsset asset, FName propname, out FProperty property) {
            var export = asset.GetClassExport();
            if (export != null) {
                foreach (var prop in export.LoadedProperties) {
                    if (prop.Name != propname) continue;
                    property = prop;
                    return true;
                }

                property = new FObjectProperty();
                PrintOutput("No " + propname.ToName() + "in ClassExport");
                return false;
            }

            property = new FObjectProperty();
            PrintOutput("No ClassExport");
            return false;
        }

        public static bool FindPropertyData(FPackageIndex export, string propName, out PropertyData prop) {
            prop = null;
            if (!export.IsExport() || export.ToExport(Asset) is not NormalExport exp) return false;
            foreach (var property in exp.Data.Where(property => property.Name.ToName() == propName)) {
                prop = property;
                return true;
            }

            return false;
        }

        public static bool FindPropertyData(Export export, string propName, out PropertyData prop) {
            prop = null;
            if (export is not NormalExport exp) return false;
            foreach (var property in exp.Data.Where(property => property.Name.ToName() == propName)) {
                prop = property;
                return true;
            }

            return false;
        }

        public static bool FindPropertyData(List<PropertyData> list, string propName, out PropertyData prop) {
            prop = null;
            foreach (var property in list.Where(property => property.Name.ToName() == propName)) {
                prop = property;
                return true;
            }

            return false;
        }

        public static bool FindPropertyData(List<PropertyData> list, string propName, out PropertyData[] props) {
            props = null;
            List<PropertyData> temp = list.Where(property => property.Name.ToName() == propName).ToList();
            if (temp.Count <= 0) return false;
            props = temp.ToArray();
            return true;

        }

        public static void FixIndexes() {
            var index = 0;
            Dict.Add(0, -1);

            for (var i = 1; i <= Asset.Imports.Count; i++) {
                //string importname = asset.Imports[i - 1].ObjectName.Value.Value;
                Dict.Add(-i, index);
                index++;
            }

            for (var i = 1; i <= Asset.Exports.Count; i++)
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

        public static int GetClassIndex() {
            for (var i = 1; i <= Asset.Exports.Count; i++)
                if (Asset.Exports[i - 1] is ClassExport) return i;
            return 0;
        }

        public static int Index(int index) {
            if (Dict.TryGetValue(index, out var validIndex)) return validIndex;
            return -1;
        }


        public static string GetName(int index) {
            if (index > 0) return Asset.Exports[index - 1].ObjectName.ToName();
            return index < 0 ? Asset.Imports[-index - 1].ObjectName.ToName() : "";
        }

        public static string GetFullName(int index, bool alt = false) {
            switch (index) {
                case > 0 when Asset.Exports[index - 1].OuterIndex.Index != 0: {
                    var parent = GetFullName(Asset.Exports[index - 1].OuterIndex.Index);
                    return parent + "." + Asset.Exports[index - 1].ObjectName.ToName();
                }
                case > 0:
                    return Asset.Exports[index - 1].ObjectName.ToName();
                case < 0 when Asset.Imports[-index - 1].OuterIndex.Index != 0: {
                    var parent = GetFullName(Asset.Imports[-index - 1].OuterIndex.Index);
                    return parent + "." + Asset.Imports[-index - 1].ObjectName.ToName();
                }
                case < 0:
                    return Asset.Imports[-index - 1].ObjectName.ToName();
                default:
                    return "";
            }
        }

        public static string GetParentName(int index) {
            switch (index) {
                case > 0 when Asset.Exports[index - 1].OuterIndex.Index != 0: {
                    var parent = GetFullName(Asset.Exports[index - 1].OuterIndex.Index);
                    return parent;
                }
                case > 0:
                    return "";
                case < 0 when Asset.Imports[-index - 1].OuterIndex.Index != 0: {
                    var parent = GetFullName(Asset.Imports[-index - 1].OuterIndex.Index);
                    return parent;
                }
                case < 0:
                    return "";
                default:
                    return "";
            }
        }

        public static bool FindProperty(int index, FName propname, out FProperty property) {
            if (index < 0) {
                //PrintOutput(index+" , "+GetFullName(index) + " , " + propname.ToName());

                /*var klass = Asset.Imports[-index - 1].ClassName.ToName();
                var owner = GetName(index);
                var parent = GetParentName(index);*/
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
                        PrintOutput("No such file on disk "+"Class "+klass+ " Path " +path);
                    }
                }*/

                import_variables.Add(Asset.Imports[-index - 1].ClassName.ToName() + " " + GetFullName(index) + " " +
                                    propname.ToName());

                property = new FObjectProperty();
                return false;
            }

            var export = Asset.Exports[index - 1];
            if (export is StructExport structExport)
                foreach (var prop in structExport.LoadedProperties)
                    if (prop.Name == propname) {
                        property = prop;
                        return true;
                    }

            property = new FObjectProperty();
            return false;
        }

        public static bool CheckDuplications(ref List<PropertyData> data) {
            for (var i = 0; i < data.Count; i++)
                if (i != 0) {
                    if (data[i].DuplicationIndex <= 0) continue;
                    if (data[i].DuplicationIndex == data[i - 1].DuplicationIndex + 1 ||
                        data[i].Name.ToName() != data[i - 1].Name.ToName()) continue;
                    PrintOutput("Missing property with lower duplication index  Name : " +
                                      data[i].Name.ToName() + " Type : " + data[i].PropertyType.ToName() +
                                      " StructType : " + (data[i] as StructPropertyData)?.StructType.ToName());

                    return false;
                } else {
                    if (data[i].DuplicationIndex <= 0) continue;
                    PrintOutput(" i=0  Missing property with lower duplication index  Name : " +
                                      data[i].Name.ToName() + " Type : " + data[i].PropertyType.ToName() +
                                      " StructType : " + (data[i] as StructPropertyData)?.StructType.ToName());
                    return false;
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
            var name = Path.GetFileNameWithoutExtension(file).ToLower();
            Asset = new UAsset(file, version, true);
            if (Asset.Exports.Count == 1) {
                return GetFullName(Asset.Exports[0].ClassIndex.Index);
            }

            List<Export> exportNames = new();
            List<Export> isAsset = new();
            foreach (var exp in Asset.Exports) {
                if (exp.ObjectName.ToName().ToLower() == name + "_c") exportNames.Add(exp);
                if (exp.bIsAsset) isAsset.Add(exp);
            }

            if (exportNames.Count == 0)
                exportNames.AddRange(Asset.Exports.Where(exp => exp.ObjectName.ToName().ToLower() == name));

            if (exportNames.Count == 1) {
                return GetFullName(exportNames[0].ClassIndex.Index);
            }

            if (isAsset.Count == 1) {
                return GetFullName(isAsset[0].ClassIndex.Index);
            }

            PrintOutput("Couldn't identify asset type : " + file);
            return "null";
        }

        public static JObject GuidToJson(Guid value) {
            var res = new JObject();
            var guid = value.ToUnsignedInts();
            res.Add(new JProperty("A", (int)guid[0]));
            res.Add(new JProperty("B", (int)guid[1]));
            res.Add(new JProperty("C", (int)guid[2]));
            res.Add(new JProperty("D", (int)guid[3]));
            return res;
        }
    }
}
