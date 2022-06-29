using System.Collections.Generic;
using Newtonsoft.Json;
using UAssetAPI;

namespace CookedAssetSerializer {
    public struct Globals {
        [JsonProperty]
        public static string CONTENT_DIR;
        [JsonProperty]
        public static string JSON_DIR;
        [JsonProperty]
        public static string OUTPUT_DIR;
        [JsonProperty]
        public static UE4Version GLOBAL_UE_VERSION;
        [JsonProperty]
        public static bool REFRESH_ASSETS;
        [JsonProperty]
        public static List<EAssetType> SKIP_SERIALIZATION;
        [JsonProperty]
        public static List<string> CIRCULAR_DEPENDENCY;
        [JsonProperty]
        public static List<string> SIMPLE_ASSETS;
        [JsonProperty]
        public static List<string> TYPES_TO_COPY;
        [JsonProperty]
        public static int SELECTED_INDEX;

        public Globals(string contentDir, string jsonDir, string outputDir, UE4Version ueVersion, int selectedIndex, bool refreshAssets,
            List<EAssetType> skipSerialization, List<string> circularDeps, List<string> simpleAssets, List<string> cookedAssets) {
            CONTENT_DIR = contentDir;
            JSON_DIR = jsonDir;
            OUTPUT_DIR = outputDir;
            GLOBAL_UE_VERSION = ueVersion;
            REFRESH_ASSETS = refreshAssets;
            SKIP_SERIALIZATION = skipSerialization;
            CIRCULAR_DEPENDENCY = circularDeps;
            SIMPLE_ASSETS = simpleAssets;
            TYPES_TO_COPY = cookedAssets;
            SELECTED_INDEX = selectedIndex;
        }

        public string GetContentDir() {
            return CONTENT_DIR;
        }

        public string GetJSONDir() {
            return JSON_DIR;
        }

        public string GetOutputDir() {
            return OUTPUT_DIR;
        }

        public UE4Version GetUEVersion() {
            return GLOBAL_UE_VERSION;
        }

        public int GetSelectedIndex() {
            return SELECTED_INDEX;
        }

        public bool GetRefreshAssets() {
            return REFRESH_ASSETS;
        }

        public List<EAssetType> GetSkipSerialization() {
            return SKIP_SERIALIZATION;
        }

        public List<string> GetCircularDependency() {
            return CIRCULAR_DEPENDENCY;
        }

        public List<string> GetSimpleAssets() {
            return SIMPLE_ASSETS;
        }

        public List<string> GetTypesToCopy() {
            return TYPES_TO_COPY;
        }

        public void ClearLists() {
            CIRCULAR_DEPENDENCY.Clear();
            SIMPLE_ASSETS.Clear();
            TYPES_TO_COPY.Clear();
        }

        public void ScanAssetTypes() {
            Utils.ScanAssetTypes();
        }

        public void GetCookedAssets() {
            Utils.GetCookedAssets();
        }

        public void SerializeAssets() {
            Utils.SerializeAssets();
        }
    }
}
