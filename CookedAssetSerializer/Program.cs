using System;
using System.Collections.Generic;
using UAssetAPI;
using static CookedAssetSerializer.Serializers;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Globals;


namespace CookedAssetSerializer {

    public static class Globals {

        public static string ContentDir = @"C:\Games\DRGNEW\FSD\Content";
        public static string JsonDir = @"C:\Games\DRGNEW\FSD\Json";
        public static UE4Version globalUE = UE4Version.VER_UE4_27;
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
        };

        public static List<string> typestocopy = new List<string> {
            //"/Script/FSD.ResourceData",
        };
    }

    class Program  {

		static void Main(string[] args) {

            //ScanAssetTypes(@"C:\Games\DRGNEW\FSD\Content", globalUE);
            //MoveAssets(@"C:\Games\DRGNEW\FSD\Content", @"C:\Games\DRGNEW\FSD\Cooked",typestocopy,globalUE);
            SerializeAssets(@"C:\Games\DRGNEW\FSD\Content");

        }


    }
}

