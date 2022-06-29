using System.Collections.Generic;
using UAssetAPI;

namespace CookedAssetSerializer {
    public struct Globals {
        public static string CONTENT_DIR;
        public static string JSON_DIR;
        public static string OUTPUT_DIR;
        public static UE4Version GLOBAL_UE_VERSION;
        public static bool REFRESH_ASSETS;
        public static List<EAssetType> SKIP_SERIALIZATION;
        public static List<string> CIRCULAR_DEPENDENCY;
        public static List<string> SIMPLE_ASSETS;
        public static List<string> TYPES_TO_COPY;

        public Globals(string contentDir, string jsonDir, string outputDir, UE4Version ueVersion, bool refreshAssets,
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
