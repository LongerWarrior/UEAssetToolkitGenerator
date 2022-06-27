using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;


namespace CookedAssetSerializer {
    public static class Globals {
        public static string CONTENT_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\cyubeVR-WindowsNoEditor\cyubeVR\Content";
        public static string JSON_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON";
        public static string OUTPUT_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON\Output";
        public static UE4Version GLOBAL_UE_VERSION = UE4Version.VER_UE4_27;
        public static bool REFRESH_ASSETS = true;

        public static List<EAssetType> SKIP_SERIALIZATION = new() {
            EAssetType.BlendSpaceBase,
            EAssetType.AnimSequence,
            EAssetType.SkeletalMesh,
            EAssetType.Skeleton,
            EAssetType.AnimMontage,
            EAssetType.FileMediaSource,
            EAssetType.StaticMesh,
        };

        public static List<string> CIRCULAR_DEPENDENCY = new() {
            "/Script/Engine.SoundClass",
            "/Script/Engine.SoundSubmix",
            "/Script/Engine.EndpointSubmix"
        };

        public static List<string> SIMPLE_ASSETS = new() {
            "/Script/Engine.SoundClass",
            "/Script/Engine.TextureRenderTarget2D",
            "/Script/Engine.MaterialFunction",
            "/Script/Engine.SoundMix",
            "/Script/Engine.SoundConcurrency",
            "/Script/Engine.ForceFeedbackEffect",
            "/Script/Engine.SoundAttenuation",
            "/Script/Foliage.FoliageType_InstancedStaticMesh",
        };

        public static List<string> TYPES_TO_COPY = new() {
            "/Script/Engine.ParticleSystem",
            "/Script/Engine.StaticMesh",
            "/Script/Engine.SoundWave",
            "/Script/Engine.SkeletalMesh",
            "/Script/Engine.Skeleton",
            "/Script/Engine.AnimSequence",
            "/Script/Engine.BlendSpace1D",
            "/Script/Engine.PhysicsAsset",
            "/Script/Engine.VectorFieldStatic",
            "/Script/Engine.SubUVAnimation",
            "/Script/Engine.AimOffsetBlendSpace",
        };
    }

    internal static class Program {
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        private static void Main(string[] args) {
            // ScanAssetTypes();
            // MoveAssets();
            SerializeAssets();
        }
    }
}
