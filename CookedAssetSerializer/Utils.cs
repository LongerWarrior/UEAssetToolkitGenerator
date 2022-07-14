using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using UAssetAPI.FieldTypes;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.Serializers;

namespace CookedAssetSerializer
{
    public class Utils
    {
        public string ContentDir;
        public string JSONDir;
        public string OutputDir;
        public UE4Version GlobalUEVersion;
        public bool RefreshAssets;
        public List<EAssetType> SkipSerialization;
        public List<string> CircularDependency;
        public List<string> SimpleAssets;
        public List<string> TypesToCopy;
        
        public int AssetTotal;
        public int AssetCount;

        public Utils(string contentDir, string jsonDir, string outputDir, UE4Version ueVersion, bool refreshAssets,
            List<EAssetType> skipSerialization, List<string> circularDeps, List<string> simpleAssets,
            List<string> cookedAssets)
        {
            ContentDir = contentDir;
            JSONDir = jsonDir;
            OutputDir = outputDir;
            GlobalUEVersion = ueVersion;
            RefreshAssets = refreshAssets;
            SkipSerialization = skipSerialization;
            CircularDependency = circularDeps;
            SimpleAssets = simpleAssets;
            TypesToCopy = cookedAssets;
        }

        public int GetAssetTotal()
        {
            return AssetTotal;
        }

        public int GetAssetCount()
        {
            return AssetCount;
        }

        public void ScanAssetTypes(string typeToFind = "")
        {
            Dictionary<string, List<string>> types = new();
            List<string> allTypes = new();

            var files = Directory.GetFiles(ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                var type = GetAssetType(file, GlobalUEVersion);
                var path = "/" + Path.GetRelativePath(ContentDir, file).Replace("\\", "/");

                PrintOutput(path, "Scan");
                AssetCount++;

                if (types.ContainsKey(type)) types[type].Add(path);
                else types[type] = new List<string> { path };

                if (type == typeToFind) PrintOutput(type + " : " + path, "Scan");
            }

            PrintOutput("Find all files " + files.Length, "Scan");
            var jTypes = new JObject();
            foreach (var entry in types)
            {
                PrintOutput(entry.Key + " : " + entry.Value.Count, "Scan");
                jTypes.Add(entry.Key, JArray.FromObject(entry.Value));
                allTypes.Add("\"" + entry.Key + "\",");
            }

            File.WriteAllText(JSONDir + "\\AssetTypes.json", jTypes.ToString());
            File.WriteAllText(JSONDir + "\\AllTypes.txt", string.Join("\n", allTypes));
        }
        
        public void GetCookedAssets(bool copy = true)
        {
            var files = Directory.GetFiles(ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                var uexpFile = Path.ChangeExtension(file, "uexp");
                var ubulkFile = Path.ChangeExtension(file, "ubulk");
                var type = GetAssetType(file, GlobalUEVersion);

                AssetCount++;
                if (!TypesToCopy.Contains(type))
                {
                    PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                    continue;
                }

                var relativePath = Path.GetRelativePath(ContentDir, file);
                var newPath = Path.Combine(OutputDir, relativePath);

                PrintOutput(newPath, "GetCookedAssets");

                Directory.CreateDirectory(Path.GetDirectoryName(newPath) ?? string.Empty);
                if (copy) File.Copy(file, newPath, true);
                else File.Move(file, newPath, true);

                if (File.Exists(uexpFile))
                {
                    if (copy) File.Copy(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                    else File.Move(uexpFile, Path.ChangeExtension(newPath, "uexp"), true);
                }

                if (!File.Exists(ubulkFile)) continue;
                if (copy) File.Copy(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
                else File.Move(ubulkFile, Path.ChangeExtension(newPath, "ubulk"), true);
            }
        }
        
        public void SerializeAssets()
        {
            var files = Directory.GetFiles(ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                UAsset asset = new UAsset(file, GlobalUEVersion, true);
                AssetCount++;

                if (SkipSerialization.Contains(asset.assetType))
                {
                    PrintOutput("Skipped serialization on " + file, "SerializeAssets");
                    continue;
                }

                PrintOutput(file, "SerializeAssets");

                if (asset.assetType != EAssetType.Uncategorized)
                {
                    switch (asset.assetType)
                    {
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
                }
                else
                {
                    var aType = GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset);
                    if (SimpleAssets.Contains(aType)) SerializeSimpleAsset();
                }
            }
        }

        public void PrintOutput(string output, string type = "debug")
        {
            Console.WriteLine(output);
            var filename = type == "debug" ? "debug" : "output";
            using var sw = File.AppendText(Path.Combine(JSONDir, filename + "_log.txt"));
            sw.WriteLine($"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}");
        }

        public bool FindExternalProperty(UAsset asset, FName propname, out FProperty property)
        {
            var export = asset.GetClassExport();
            if (export != null)
            {
                foreach (var prop in export.LoadedProperties)
                    if (prop.Name == propname)
                    {
                        property = prop;
                        return true;
                    }

                property = new FObjectProperty();
                Console.WriteLine("No " + propname.ToName() + "in ClassExport");
                return false;
            }
            else
            {
                property = new FObjectProperty();
                Console.WriteLine("No ClassExport");
                return false;
            }
        }

        public bool FindPropertyData(FPackageIndex export, string propname, out PropertyData prop, UAsset asset)
        {
            prop = null;
            if (export.IsExport() && export.ToExport(asset) is NormalExport exp)
                foreach (var property in exp.Data)
                    if (property.Name.ToName() == propname)
                    {
                        prop = property;
                        return true;
                    }

            return false;
        }

        public bool FindPropertyData(Export export, string propname, out PropertyData prop)
        {
            prop = null;
            if (export is NormalExport exp)
                foreach (var property in exp.Data)
                    if (property.Name.ToName() == propname)
                    {
                        prop = property;
                        return true;
                    }

            return false;
        }

        public bool FindPropertyData(List<PropertyData> list, string propname, out PropertyData prop)
        {
            prop = null;
            foreach (var property in list)
                if (property.Name.ToName() == propname)
                {
                    prop = property;
                    return true;
                }

            return false;
        }

        public bool FindPropertyData(List<PropertyData> list, string propname, out PropertyData[] props)
        {
            props = null;
            List<PropertyData> temp = new();
            foreach (var property in list)
                if (property.Name.ToName() == propname)
                    temp.Add(property);
            if (temp.Count > 0)
            {
                props = temp.ToArray();
                return true;
            }

            return false;
        }

        public void FixIndexes(Dictionary<int, int> dict, UAsset asset)
        {
            var index = 0;
            dict.Add(0, -1);

            for (var i = 1; i <= asset.Imports.Count; i++)
            {
                //string importname = asset.Imports[i - 1].ObjectName.Value.Value;
                dict.Add(-i, index);
                index++;
            }

            for (var i = 1; i <= asset.Exports.Count; i++)
                if (asset.Exports[i - 1] is FunctionExport)
                {
                    /*if (asset.Exports[i - 1].ObjectName.ToName().StartsWith("ExecuteUbergraph_")) {
                        dict.Add(i, index);
                        index++;
                    }*/
                }
                else
                {
                    dict.Add(i, index);
                    index++;
                }
        }

        public int GetClassIndex(UAsset asset)
        {
            for (var i = 1; i <= asset.Exports.Count; i++)
                if (asset.Exports[i - 1] is ClassExport)
                    return i;
            return 0;
        }

        public int Index(int index, Dictionary<int, int> dict)
        {
            if (dict.TryGetValue(index, out var validindex))
                return validindex;
            else //Console.WriteLine("Non valid Import index : "+index);
                return -1;
        }
        
        public string GetName(int index, UAsset asset)
        {
            if (index > 0)
                return asset.Exports[index - 1].ObjectName.ToName();
            else if (index < 0)
                return asset.Imports[-index - 1].ObjectName.ToName();
            else
                return "";
        }

        public string GetFullName(int index, UAsset asset, bool alt = false)
        {
            if (index > 0)
            {
                if (asset.Exports[index - 1].OuterIndex.Index != 0)
                {
                    var parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index, asset);
                    return parent + "." + asset.Exports[index - 1].ObjectName.ToName();
                }

                return asset.Exports[index - 1].ObjectName.ToName();
            }

            if (index < 0)
            {
                if (asset.Imports[-index - 1].OuterIndex.Index != 0)
                {
                    var parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index, asset);
                    return parent + "." + asset.Imports[-index - 1].ObjectName.ToName();
                }

                return asset.Imports[-index - 1].ObjectName.ToName();
            }
            return "";
        }

        public string GetParentName(int index, UAsset asset)
        {
            if (index > 0)
            {
                if (asset.Exports[index - 1].OuterIndex.Index != 0)
                {
                    var parent = GetFullName(asset.Exports[index - 1].OuterIndex.Index, asset);
                    return parent;
                }

                return "";
            }

            if (index < 0)
            {
                if (asset.Imports[-index - 1].OuterIndex.Index != 0)
                {
                    var parent = GetFullName(asset.Imports[-index - 1].OuterIndex.Index, asset);
                    return parent;
                }

                return "";
            }

            return "";
        }

        public bool FindProperty(int index, FName propname, out FProperty property, UAsset asset, List<string> importVariables)
        {
            if (index < 0)
            {
                //Console.WriteLine(index+" , "+GetFullName(index) + " , " + propname.ToName());

                var klass = asset.Imports[-index - 1].ClassName.ToName();
                var owner = GetName(index, asset);
                var parent = GetParentName(index, asset);
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


                importVariables.Add(asset.Imports[-index - 1].ClassName.ToName() + " " + GetFullName(index, asset) + " " +
                                    propname.ToName());

                property = new FObjectProperty();
                return false;
            }

            var export = asset.Exports[index - 1];
            if (export is StructExport)
                foreach (var prop in (export as StructExport).LoadedProperties)
                    if (prop.Name == propname)
                    {
                        property = prop;
                        return true;
                    }

            property = new FObjectProperty();
            return false;
        }

        public bool CheckDuplications(ref List<PropertyData> Data)
        {
            for (var i = 0; i < Data.Count; i++)
                if (i != 0)
                {
                    if (Data[i].DuplicationIndex > 0)
                    {
                        if (Data[i].DuplicationIndex != Data[i - 1].DuplicationIndex + 1 &&
                            Data[i].Name.ToName() == Data[i - 1].Name.ToName())
                        {
                            Console.WriteLine("Missing property with lower duplication index  Name : " +
                                              Data[i].Name.ToName() + " Type : " + Data[i].PropertyType.ToName() +
                                              " StructType : " + (Data[i] as StructPropertyData).StructType.ToName());

                            return false;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (Data[i].DuplicationIndex > 0)
                    {
                        Console.WriteLine(" i=0  Missing property with lower duplication index  Name : " +
                                          Data[i].Name.ToName() + " Type : " + Data[i].PropertyType.ToName() +
                                          " StructType : " + (Data[i] as StructPropertyData).StructType.ToName());
                        return false;
                    }
                }

            return true;
        }
        
        public string GetAssetType(string file, UE4Version version)
        {
            var name = Path.GetFileNameWithoutExtension(file).ToLower();
            UAsset asset = new UAsset(file, version, true);
            if (asset.Exports.Count == 1)
            {
                return GetFullName(asset.Exports[0].ClassIndex.Index, asset);
            }

            List<Export> exportnames = new();
            List<Export> isasset = new();
            foreach (var exp in asset.Exports)
            {
                if (exp.ObjectName.ToName().ToLower() == name + "_c") exportnames.Add(exp);
                if (exp.bIsAsset) isasset.Add(exp);
            }

            if (exportnames.Count == 0)
                foreach (var exp in asset.Exports)
                    if (exp.ObjectName.ToName().ToLower() == name)
                        exportnames.Add(exp);

            if (exportnames.Count == 1)
            {
                return GetFullName(exportnames[0].ClassIndex.Index, asset);
            }

            if (isasset.Count == 1)
            {
                return GetFullName(isasset[0].ClassIndex.Index, asset);
            }

            Console.WriteLine("Couldn't identify asset type : " + file);
            return "null";
        }

        public JObject GuidToJson(Guid Value)
        {
            var res = new JObject();
            var guid = Value.ToUnsignedInts();
            res.Add(new JProperty("A", (int)guid[0]));
            res.Add(new JProperty("B", (int)guid[1]));
            res.Add(new JProperty("C", (int)guid[2]));
            res.Add(new JProperty("D", (int)guid[3]));
            return res;
        }
    }
}