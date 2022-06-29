using System.Collections.Generic;
using UAssetAPI;

namespace CookedAssetSerializer {
    internal static class Program {
        private static void Main(string[] args) {
            const string CONTENT_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\cyubeVR-WindowsNoEditor\cyubeVR\Content";
            const string JSON_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON";
            const string OUTPUT_DIR = @"F:\CyubeVR Modding\_Tools\UnrealPacker4.27\JSON\Output";
            const UE4Version GLOBAL_UE_VERSION = UE4Version.VER_UE4_27;
            const bool REFRESH_ASSETS = true;

            List<EAssetType> skipSerialization = new() {
                EAssetType.BlendSpaceBase,
                EAssetType.AnimSequence,
                EAssetType.SkeletalMesh,
                EAssetType.Skeleton,
                EAssetType.AnimMontage,
                EAssetType.FileMediaSource,
                EAssetType.StaticMesh,
            };

            List<string> circularDependency = new() {
                "/Script/Engine.SoundClass",
                "/Script/Engine.SoundSubmix",
                "/Script/Engine.EndpointSubmix"
            };

            List<string> simpleAssets = new() {
                "/Script/Engine.SoundClass",
                "/Script/Engine.TextureRenderTarget2D",
                "/Script/Engine.MaterialFunction",
                "/Script/Engine.SoundMix",
                "/Script/Engine.SoundConcurrency",
                "/Script/Engine.ForceFeedbackEffect",
                "/Script/Engine.SoundAttenuation",
                "/Script/Foliage.FoliageType_InstancedStaticMesh",
            };

            List<string> typesToCopy = new() {
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

            Globals settings = new Globals(CONTENT_DIR, JSON_DIR, OUTPUT_DIR, GLOBAL_UE_VERSION, REFRESH_ASSETS,
                skipSerialization, circularDependency, simpleAssets, typesToCopy);
            settings.ScanAssetTypes();
            settings.GetCookedAssets();
            settings.SerializeAssets();
        }
    }
}
