using System;
using System.Collections.Generic;
using UAssetAPI;
using static CookedAssetSerializer.Serializers;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;


namespace CookedAssetSerializer {

    public static class Globals {

        //public static string ContentDir = @"C:\Games\DRGNEW\FSD\Content";
        //public static string JsonDir = @"C:\Games\DRGNEW\FSD\Json";
        public static string ContentDir = @"D:\SteamLibrary\steamapps\common\Trepang2 Demo\CPPFPS\Content\Paks\CPPFPS-WindowsNoEditor\CPPFPS\Content";
        public static string JsonDir = @"D:\SteamLibrary\steamapps\common\Trepang2 Demo\CPPFPS\Content\Paks\CPPFPS-WindowsNoEditor\CPPFPS\Json";
        public static UE4Version globalUE = UE4Version.VER_UE4_27;

        //public static string ContentDir = @"D:\GR\Ghostrunner\Content";
        //public static string JsonDir = @"D:\GR\Ghostrunner\Json";
        //public static string ContentDir = @"C:\Users\LongerWarrior\Downloads";
        //public static string JsonDir = @"C:\Users\LongerWarrior\Downloads";
        //public static UE4Version globalUE = UE4Version.VER_UE4_26;
        public static bool refreshassets = true;

        public static List<EAssetType> skipserialization = new List<EAssetType> {
             //EAssetType.Blueprint,
             //EAssetType.WidgetBlueprint,
             EAssetType.AnimBlueprint,
             EAssetType.BlendSpaceBase,
             EAssetType.AnimSequence,
             EAssetType.SkeletalMesh,
             EAssetType.Skeleton,
             EAssetType.AnimMontage,
             EAssetType.FileMediaSource,
        };

        public static List<string> circulardependency = new List<string> {
            "/Script/Engine.SoundClass",
            "/Script/Engine.SoundSubmix",
            "/Script/Engine.EndpointSubmix",
        };

        public static List<string> simpleassets = new List<string> {
            //"/Script/FSD.ResourceData",
            "/Script/Engine.TextureLightProfile",
            "/Script/Engine.SoundAttenuation",
            "/Script/Engine.SoundWave",
            "/Script/AIModule.BehaviorTree",
            "/Script/Engine.CameraAnim",
            "/Script/Engine.SoundConcurrency",
            "/Script/Engine.ReverbEffect",
            "/Script/AudioMixer.SubmixEffectReverbPreset",
            "/Script/Engine.SoundClass",
            "/Script/Engine.SoundMix",
            "/Script/Engine.SoundSubmix",
            "/Script/AIModule.BlackboardData",
            "/Script/CPPFPS.TrepangPhysMat",
            "/Script/SaveExtension.SavePreset",
            "/Script/Engine.TextureRenderTargetCube",
            "/Script/Landscape.LandscapeLayerInfoObject",
            "/Script/Engine.HLODProxy",
            "/Script/Engine.SoundEffectSourcePresetChain",
            "/Script/Synthesis.SourceEffectMidSideSpreaderPreset",
            "/Script/Synthesis.SourceEffectPannerPreset",
            "/Script/Synthesis.SourceEffectBitCrusherPreset",
            "/Script/Synthesis.SourceEffectChorusPreset",
            "/Script/Synthesis.SourceEffectDynamicsProcessorPreset",
            "/Script/Synthesis.SourceEffectEnvelopeFollowerPreset",
            "/Script/Synthesis.SourceEffectEQPreset",
            "/Script/Synthesis.SourceEffectFilterPreset",
            "/Script/Synthesis.SourceEffectFoldbackDistortionPreset",
            "/Script/Synthesis.SourceEffectPhaserPreset",
            "/Script/Synthesis.SourceEffectRingModulationPreset",
            "/Script/Synthesis.SourceEffectSimpleDelayPreset",
            "/Script/Synthesis.SourceEffectWaveShaperPreset",
            "/Script/SlateCore.SlateWidgetStyleAsset",
            "/Script/AIModule.EnvQuery",
            "/Script/Engine.VectorFieldStatic",
            "/Script/ApexDestruction.DestructibleMesh",
            "/Script/Engine.TextureRenderTarget2D",
            "/Script/Foliage.FoliageType_InstancedStaticMesh",
            "/Script/Engine.RuntimeVirtualTexture",
        };

        public static List<string> typestocopy = new List<string> {
            //"/Script/FSD.ResourceData",
        };
    }

    class Program  {

		static void Main(string[] args) {

            //ScanAssetTypes(@"D:\SteamLibrary\steamapps\common\Trepang2 Demo\CPPFPS\Content\Paks\CPPFPS-WindowsNoEditor\CPPFPS\Content", globalUE);
            //MoveAssets(@"C:\Games\DRGNEW\FSD\Content", @"C:\Games\DRGNEW\FSD\Cooked",typestocopy,globalUE);
            SerializeAssets(@"D:\SteamLibrary\steamapps\common\Trepang2 Demo\CPPFPS\Content\Paks\CPPFPS-WindowsNoEditor\CPPFPS\Content");

            //asset = new UAsset(@"C:\Users\LongerWarrior\Downloads\p_ex001_AttackData.uasset", UE4Version.VER_UE4_27,false);
            //SerializeDataTable();
        }


    }
}

