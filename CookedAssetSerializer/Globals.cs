using System.Collections.Generic;
using Newtonsoft.Json;
using UAssetAPI;

namespace CookedAssetSerializer
{
    public struct Globals
    {
        [JsonProperty] public string ContentDir;
        [JsonProperty] public string JSONDir;
        [JsonProperty] public string OutputDir;
        [JsonProperty] public UE4Version GlobalUEVersion;
        [JsonProperty] public bool RefreshAssets;
        [JsonProperty] public List<EAssetType> SkipSerialization;
        [JsonProperty] public List<string> CircularDependency;
        [JsonProperty] public List<string> SimpleAssets;
        [JsonProperty] public List<string> TypesToCopy;
        [JsonProperty] public int SelectedIndex;

        private readonly Utils utils;

        public Globals(string contentDir, string jsonDir, 
            string outputDir, UE4Version ueVersion, 
            int selectedIndex, bool refreshAssets,
            List<EAssetType> skipSerialization, List<string> circularDeps, 
            List<string> simpleAssets, List<string> cookedAssets)
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
            SelectedIndex = selectedIndex;
            
            utils = new Utils(contentDir, jsonDir, outputDir, ueVersion, refreshAssets, 
                skipSerialization, circularDeps, simpleAssets, cookedAssets);
        }

        public string GetContentDir()
        {
            return ContentDir;
        }

        public string GetJSONDir()
        {
            return JSONDir;
        }

        public string GetOutputDir()
        {
            return OutputDir;
        }

        public UE4Version GetUEVersion()
        {
            return GlobalUEVersion;
        }

        public int GetSelectedIndex()
        {
            return SelectedIndex;
        }

        public bool GetRefreshAssets()
        {
            return RefreshAssets;
        }

        public List<EAssetType> GetSkipSerialization()
        {
            return SkipSerialization;
        }

        public List<string> GetCircularDependency()
        {
            return CircularDependency;
        }

        public List<string> GetSimpleAssets()
        {
            return SimpleAssets;
        }

        public List<string> GetTypesToCopy()
        {
            return TypesToCopy;
        }

        public int GetAssetTotal()
        {
            return utils.GetAssetTotal();
        }

        public int GetAssetCount()
        {
            return utils.GetAssetCount();
        }

        public void ClearLists()
        {
            CircularDependency.Clear();
            SimpleAssets.Clear();
            TypesToCopy.Clear();
        }

        public void ScanAssetTypes()
        {
            utils.ScanAssetTypes();
        }

        public void GetCookedAssets()
        {
            utils.GetCookedAssets();
        }

        public void SerializeAssets()
        {
            utils.SerializeAssets();
        }
    }
}