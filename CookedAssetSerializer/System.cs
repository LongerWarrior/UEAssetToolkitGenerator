using System;
using System.Collections.Generic;
using System.IO;
using CookedAssetSerializer.AssetTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Serializers;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer
{
    public class System
    {
        private readonly Settings Settings;

        [JsonProperty] public int SelectedIndex;

        private int AssetTotal;
        private int AssetCount;

        public System(Settings settings, int selectedIndex)
        {
            Settings = settings;
            SelectedIndex = selectedIndex;
        }

        public int GetAssetTotal()
        {
            return AssetTotal;
        }

        public int GetAssetCount()
        {
            return AssetCount;
        }
        
        public int GetSelectedIndex()
        {
            return SelectedIndex;
        }
        
        public Settings GetSettings()
        {
            return Settings;
        }

        public void ClearLists()
        {
            Settings.CircularDependency.Clear();
            Settings.SimpleAssets.Clear();
            Settings.TypesToCopy.Clear();
        }

        public void ScanAssetTypes(string typeToFind = "")
        {
            Dictionary<string, List<string>> types = new();
            List<string> allTypes = new();

            var files = Directory.GetFiles(Settings.ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                var type = GetAssetType(file, Settings.GlobalUEVersion);
                var path = "/" + Path.GetRelativePath(Settings.ContentDir, file).Replace("\\", "/");

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

            File.WriteAllText(Settings.JSONDir + "\\AssetTypes.json", jTypes.ToString());
            File.WriteAllText(Settings.JSONDir + "\\AllTypes.txt", string.Join("\n", allTypes));
        }
        
        public void GetCookedAssets(bool copy = true)
        {
            var files = Directory.GetFiles(Settings.ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                var uexpFile = Path.ChangeExtension(file, "uexp");
                var ubulkFile = Path.ChangeExtension(file, "ubulk");
                var type = GetAssetType(file, Settings.GlobalUEVersion);

                AssetCount++;
                if (!Settings.TypesToCopy.Contains(type))
                {
                    PrintOutput("Skipped operation on " + file, "GetCookedAssets");
                    continue;
                }

                var relativePath = Path.GetRelativePath(Settings.ContentDir, file);
                var newPath = Path.Combine(Settings.OutputDir, relativePath);

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
            var files = Directory.GetFiles(Settings.ContentDir, "*.uasset", SearchOption.AllDirectories);

            AssetTotal = files.Length;
            AssetCount = 0;
            foreach (var file in files)
            {
                UAsset asset = new UAsset(file, Settings.GlobalUEVersion, true);
                AssetCount++;

                if (Settings.SkipSerialization.Contains(asset.assetType))
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
                            var bps = new BlueprintSerializer(Settings);
                            bps.SerializeAsset(false);
                            break;
                        case EAssetType.DataTable:
                            new DataTableSerializer(Settings);
                            break;
                        case EAssetType.StringTable:
                            SerializeStringTable();
                            break;
                        case EAssetType.UserDefinedStruct:
                            SerializeUserDefinedStruct();
                            break;
                        case EAssetType.BlendSpaceBase:
                            var bsb = new BlendSpaceSerializer(Settings);
                            bsb.SerializeAsset();
                            break;
                        case EAssetType.AnimMontage:
                        case EAssetType.CameraAnim:
                        case EAssetType.LandscapeGrassType:
                        case EAssetType.MediaPlayer:
                        case EAssetType.MediaTexture:
                        case EAssetType.SubsurfaceProfile:
                            var sas = new SimpleAssetSerializer<NormalExport>(Settings);
                            sas.SerializeAsset(null, null, null, false, false);
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
                            new SoundCueSerializer(Settings);
                            break;
                        case EAssetType.Font:
                            SerializeFont();
                            break;
                        case EAssetType.FontFace:
                            SerializeFontFace();
                            break;
                        case EAssetType.CurveBase:
                            new CurveBaseSerializer(Settings);
                            break;
                        case EAssetType.Texture2D:
                            SerializeTexture();
                            break;
                        case EAssetType.SkeletalMesh:
                            break;
                        case EAssetType.FileMediaSource:
                            new FileMediaSourceSerializer(Settings);
                            break;
                        case EAssetType.StaticMesh:
                            SerializeStaticMesh();
                            break;
                    }
                }
                else
                {
                    var aType = GetFullName(asset.Exports[asset.mainExport - 1].ClassIndex.Index, asset);
                    if (!Settings.SimpleAssets.Contains(aType)) continue;
                    var sas = new SimpleAssetSerializer<NormalExport>(Settings);
                    sas.SerializeAsset();
                }
            }
        }

        public void PrintOutput(string output, string type = "debug")
        {
            Console.WriteLine(output);
            var filename = type == "debug" ? "debug" : "output";
            using var sw = File.AppendText(Path.Combine(Settings.JSONDir, filename + "_log.txt"));
            sw.WriteLine($"[{type}] {DateTime.Now:HH:mm:ss}: {AssetCount}/{AssetTotal} {output}");
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
                {
                    if (exp.ObjectName.ToName().ToLower() == name)
                        exportnames.Add(exp);
                }

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
    }
}