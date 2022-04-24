using System;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.Serializers;
using System.IO;
using System.Collections.Generic;

namespace CookedAssetSerializer {

	class Program  {

		static void Main(string[] args) {

            var typestocopy = new List<string> {
                "/Script/Engine.SoundWave",
                "/Script/Engine.SoundCue",
                "/Script/Engine.ParticleSystem",
                "/Script/Niagara.NiagaraSystem",
                "/Script/Engine.SoundClass",
                "/Script/Engine.SoundAttenuation",
                "/Script/Engine.SoundMix",
                "/Script/AIModule.BehaviorTree",
                "/Script/Engine.MaterialFunction",
                "/Script/Engine.SoundConcurrency",
                "/Script/AIModule.BlackboardData",
                "/Script/Engine.SoundSubmix",
                "/Script/Engine.ForceFeedbackEffect",
                "/Script/Engine.ReverbEffect",
                "/Script/Niagara.NiagaraScript",
                "/Script/AudioMixer.SubmixEffectSubmixEQPreset",
                "/Script/Engine.Rig",
                "/Script/Engine.EndpointSubmix",
                "/Script/AnimationSharing.AnimationSharingSetup",
                "/Script/AudioMixer.SubmixEffectDynamicsProcessorPreset",
                "/Script/AudioMixer.SubmixEffectReverbPreset",
                "/Script/Engine.AnimComposite",
                "/Script/Engine.ForceFeedbackAttenuation",
                "/Script/Engine.VectorFieldStatic",
                "/Script/Niagara.NiagaraEffectType",
                "/Script/Niagara.NiagaraParameterCollection"

                //"/Script/FSD.DialogDataAsset",
                //"/Script/FSD.ItemSkin",
                //"/Script/FSD.AmmoDrivenWeaponUpgrade",
                //"/Script/FSD.DamageUpgrade",
                //"/Script/FSD.FSDPhysicalMaterial",
                //"/Script/FSD.OverclockUpgrade",
                //"/Script/FSD.ArmorMaterialVanityItem",
                //"/Script/FSD.HeadVanityItem",
                //"/Script/FSD.TerrainMaterial",
                //"/Script/FSD.RoomGenerator",
                //"/Script/FSD.PickaxePart",
                //"/Script/FSDEngine.StaticMeshCarver",
                //"/Script/FSD.MissionStat",
                //"/Script/FSD.ItemUpgradeCategory",
                //"/Script/FSD.FSDAchievement",
                //"/Script/FSD.EnemyDescriptor",
                //"/Script/FSD.CombinedUpgrade",
                //"/Script/FSD.MinersManualData",
                //"/Script/FSD.ItemID",
                //"/Script/FSD.ItemData",
                //"/Script/FSD.PawnAffliction",
                //"/Script/FSD.EnemyMinersManualData",
                //"/Script/FSD.EnemyID",
                //"/Script/FSD.BeardVanityItem",
                //"/Script/FSD.MilestoneAsset",
                //"/Script/FSD.HitscanBaseUpgrade",
                //"/Script/FSD.PawnStat",
                //"/Script/FSD.ArmorVanityItem",
                //"/Script/FSD.DebrisMesh",
                //"/Script/FSD.FloodFillSettings",
                //"/Script/FSD.ArmorStatUpgrade",
                //"/Script/FSD.ProjectileUpgrade",
                //"/Script/FSD.InventoryItemUpgrade",
                //"/Script/FSD.UseAnimationSetting",
                //"/Script/FSD.MoustacheVanityItem",
                //"/Script/FSD.DrinkableDataAsset",
                //"/Script/FSD.VictoryPose",
                //"/Script/FSD.FloatPerkAsset",
                //"/Script/FSD.DebrisPositioning",
                //"/Script/FSD.CoilgunUpgrade",
                //"/Script/FSD.OverclockBank",
                //"/Script/FSD.ItemRefundList",
                //"/Script/FSD.DetailNoise",
                //"/Script/FSD.TerrainType",
                //"/Script/FSD.CrossbowUpgrade",
                //"/Script/FSD.LockOnWeaponUpgrade",
                //"/Script/FSD.DamageClass",
                //"/Script/FSD.STLMeshCarver",
                //"/Script/FSD.ChargedWeaponUpgrade",
                //"/Script/FSD.SideburnsVanityItem",
                //"/Script/FSD.IconGenerationCameraKey",
                //"/Script/FSD.CryoSprayUpgrade",
                //"/Script/FSD.ArmorUpgrade",
                //"/Script/FSD.SchematicCategory",
                //"/Script/FSD.MissionStatCategory",
                //"/Script/FSD.ItemSkinSet",
                //"/Script/FSD.HUDVisibilityGroup",
                //"/Script/FSD.EnemyGroupDescriptor",
                //"/Script/FSD.BeardColorVanityItem",
                //"/Script/FSD.VanityTattoo",
                //"/Script/FSD.PlayerAfflictionOverlay",
                //"/Script/FSD.GemResourceData",
                //"/Script/FSD.CommunityGoal",
                //"/Script/FSD.ProjectileLauncherBaseUpgrade",
                //"/Script/FSD.ItemCharacterAnimationSet",
                //"/Script/FSD.FlatDamageUpgrade",
                //"/Script/FSD.FakeMoverSettings",
                //"/Script/FSD.MissionWarning",
                //"/Script/FSD.ItemSkinSchematicCollection",
                //"/Script/FSD.DebrisSet",
                //"/Script/FSD.CommnuityRewardBundle",
                //"/Script/FSD.TunnelSetting",
                //"/Script/FSD.SentryGunUpgrade",
                //"/Script/FSD.PushSatusEffectDamageBonusUpgrade",
                //"/Script/FSD.LineCutterProjectileUpgrade",
                //"/Script/FSD.DamageImpulse",
                //"/Script/FSD.DamageConversionUpgrade",
                //"/Script/FSD.BoltActionRifleUpgrade",
                //"/Script/FSD.Biome",
                //"/Script/FSD.AutoCannonUpgrade",
                //"/Script/FSD.TriggeredStatusEffectUpgrade",
                //"/Script/FSD.SkinColorVanityItem",
                //"/Script/FSD.FlameThrowerUpgrade",
                //"/Script/FSD.DifficultySetting",
                //"/Script/FSD.DamageTag",
                //"/Script/FSD.TunnelSegmentSetting",
                //"/Script/FSD.SpecialEvent",
                //"/Script/FSD.ShieldGeneratorUpgrade",
                //"/Script/FSD.MissionTemplate",
                //"/Script/FSD.GameDLC",
                //"/Script/FSD.GameActivityMissionType",
                //"/Script/FSD.DoubleDrillUpgrade",
                //"/Script/FSD.CollectableResourceData",
                //"/Script/FSD.CaveInfluencer",
                //"/Script/FSD.UseConditionSet",
                //"/Script/FSD.StatTemporaryBuff",
                //"/Script/FSD.Schematic",
                //"/Script/FSD.PickaxeUpgrade",
                //"/Script/FSD.MissionMutator",
                //"/Script/FSD.MicrowavegunUpgrade",
                //"/Script/FSD.FlareUpgrade",
                //"/Script/FSD.EyeBrowsVanityItem",
                //"/Script/FSD.CategoryID",
                //"/Script/FSD.CarvedResourceData",
                //"/Script/FSD.CarvedResourceCreator",
                //"/Script/FSD.BoscoUpgrade",
                //"/Script/FSD.WeaponSwitchProjectileUpgrade",
                //"/Script/FSD.VanitySchematicBank",
                //"/Script/FSD.SeasonChallenge",
                //"/Script/FSD.HeavyParticleCannonUpgrade",
                //"/Script/FSD.GooGunProjectileUpgrade",
                //"/Script/FSD.FSDEvent",
                //"/Script/FSD.ZiplineGunUpgrade",
                //"/Script/FSD.VeinResourceData",
                //"/Script/FSD.InventoryList",
                //"/Script/FSD.EnemyRarityMutator",
                //"/Script/FSD.CritterDescriptor",
                //"/Script/FSDEngine.TerrainMaterialCore",
                //"/Script/FSD.SchematicRarity",
                //"/Script/FSD.SchematicPricingTier",
                //"/Script/FSD.ResourceMutator",
                //"/Script/FSD.PushDynamicStatusEffectDamageBonusUpgrade",
                //"/Script/FSD.PlayerCharacterID",
                //"/Script/FSD.PlayerCharacterData",
                //"/Script/FSD.DetPackUpgrade",
                //"/Script/FSD.ConditionalDamageModifierUpgrade",
                //"/Script/FSD.ChargedProjectileUpgrade",
                //"/Script/FSD.CharacterShoutsData",
                //"/Script/FSD.CapacityUpgrade",
                //"/Script/FSD.WeaponHitCounterUpgrade",
                //"/Script/FSD.TargetStateDamageBonusUpgrade",
                //"/Script/FSD.StatusDamageBonusUpgrade",
                //"/Script/FSD.RandomDamageUpgrade",
                //"/Script/FSD.PlanetZone",
                //"/Script/FSD.MusicLibrary",
                //"/Script/FSD.MicroMissileLauncherUpgrade",
                //"/Script/FSD.GrapplingHookUpgrade",
                //"/Script/FSD.GatlingGunUpgrade",
                //"/Script/FSD.FlyingFormationData",
                //"/Script/FSD.EventRewardType",
                //"/Script/FSD.CommunityGoalFaction",
                //"/Script/FSD.CommnuityRewardSetup",
                //"/Script/FSD.CharacterVanityItems",
                //"/Script/FSD.StatusEffectExclusiveKey",
                //"/Script/FSD.SingleUsableUpgrade",
                //"/Script/FSD.SawedOffShotgunUpgrade",
                //"/Script/FSD.RoomGeneratorGroup",
                //"/Script/FSD.MusicCategory",
                //"/Script/FSD.MultiHitscanUpgrade",
                //"/Script/FSD.MissionDuration",
                //"/Script/FSD.MissionComplexity",
                //"/Script/FSD.GooGunUpgrade",
                //"/Script/FSD.FlaregunProjectileUpgrade",
                //"/Script/FSD.ElectricalSMGUpgrade",
                //"/Script/FSD.CommunityGoalCategory",
                //"/Script/FSD.CapsuleHitscanUpgrade",
                //"/Script/FSD.BeltDrivenWeaponUpgrade",
                //"/Script/FSD.AmberEventEnemyPool",
                //"/Script/FSD.VeinMutator",
                //"/Script/FSD.UseConditionCollection",
                //"/Script/FSD.TunnelParameters",
                //"/Script/FSD.TargetSpecificDamageBonusUpgrade",
                //"/Script/FSD.StickyFlameStatusEffectUpgrade",
                //"/Script/FSD.StatusAndStateDamageBonusUpgrade",
                //"/Script/FSD.ShowroomCameraKey",
                //"/Script/FSD.RareCritterDescriptor",
                //"/Script/FSD.PlayerDamageTakenMutator",
                //"/Script/FSD.PlatformGunUpgrade",
                //"/Script/FSD.PlatformExclusiveDLC",
                //"/Script/FSD.OptionalBloodPhysicalMaterial",
                //"/Script/FSD.ModifyDynamicStatusEffectDamageBonusUpgrade",
                //"/Script/FSD.LineCutterUpgrade",
                //"/Script/FSD.LimbDismembermentList",
                //"/Script/FSD.ItemAquisitionSource",
                //"/Script/FSD.GroundFormationData",
                //"/Script/FSD.GrenadeAnimationSet",
                //"/Script/FSD.EnemyPlaySoundKey",
                //"/Script/FSD.DifficultyMutator",
                //"/Script/FSD.DeepDiveTemplate",
                //"/Script/FSD.CryoSprayProjectileUpgrade",
                //"/Script/FSD.CraftingMaterialMutator",
                //"/Script/FSD.CooldownUpgrade",
                //"/Script/FSD.BurstWeaponUpgrade",
                //"/Script/FSD.BoscoProjectileAbillity",
                //"/Script/FSD.BoolUserSettingAsset",
                //"/Script/FSD.AutoShotgunUpgrade",
                //"/Script/FSD.AssaultRifleUpgrade",
                //"/Script/FSD.AndDLC",
                //"/Script/FSD.AddComponentUpgrade",
                //"/Script/FSD.WeaponChargeProgressDamageBonus",
                //"/Script/FSD.VictoryPoseSettings",
                //"/Script/FSD.VanitySettings",
                //"/Script/FSD.VanityEventSourceDataAsset",
                //"/Script/FSD.UpgradeSettings",
                //"/Script/FSD.TreeOfVanity",
                //"/Script/FSD.TreasureSettings",
                //"/Script/FSD.TerrainMaterialsCollection",
                //"/Script/FSD.StatusEffectSettings",
                //"/Script/FSD.SpecialEventSettings",
                //"/Script/FSD.SpawnSettings",
                //"/Script/FSD.ShowroomSettings",
                //"/Script/FSD.SentryGunTypeUpgrade",
                //"/Script/FSD.SeasonTokenReward",
                //"/Script/FSD.SeasonSettings",
                //"/Script/FSD.SeasonEventData",
                //"/Script/FSD.Season",
                //"/Script/FSD.SeamlessTravelEventKey",
                //"/Script/FSD.SchematicSettings",
                //"/Script/FSD.SchematicBank",
                //"/Script/FSD.RoomDecorationObject",
                //"/Script/FSD.RewardMutator",
                //"/Script/FSD.ResourceVeinMutator",
                //"/Script/FSD.ResourceData",
                //"/Script/FSD.ReflectionHitscanUpgrade",
                //"/Script/FSD.PromotionRewardsSettings",
                //"/Script/FSD.ProceduralSettings",
                //"/Script/FSD.PlayerShieldsMutator",
                //"/Script/FSD.PlasmaCarbineUpgrade",
                //"/Script/FSD.PlanetZoneSetup",
                //"/Script/FSD.PickaxeSettings",
                //"/Script/FSD.NotDLC",
                //"/Script/FSD.NoOxygenMutator",
                //"/Script/FSD.NewsTextLists",
                //"/Script/FSD.NewsTextHeadlines",
                //"/Script/FSD.MissionNameBank",
                //"/Script/FSD.MinersManual",
                //"/Script/FSD.LockCountSTEBonusUpgrade",
                //"/Script/FSD.LegacySettings",
                //"/Script/FSD.KeyBindingSettings",
                //"/Script/FSD.ItemSkinSettings",
                //"/Script/FSD.ItemSettings",
                //"/Script/FSD.InfestedEnemiesMutator",
                //"/Script/FSD.GlobalMissionSetup",
                //"/Script/FSD.GatlingHotShellsBonusUpgrade",
                //"/Script/FSD.GameDLCSettings",
                //"/Script/FSD.GameActivitySettings",
                //"/Script/FSD.GameActivityAssignmentType",
                //"/Script/FSD.FSDTutorialSettings",
                //"/Script/FSD.FSDTagSettings",
                //"/Script/FSD.FSDEventCollection",
                //"/Script/FSD.ForginSettings",
                //"/Script/FSD.ForceStationaryEncounterMutator",
                //"/Script/FSD.ExplosiveEnemiesMutator",
                //"/Script/FSD.EnemySettings",
                //"/Script/FSD.EnemyDetonationSetting",
                //"/Script/FSD.EncounterSettings",
                //"/Script/FSD.EncounterOverrideMutator",
                //"/Script/FSD.EliteEnemiesMutator",
                //"/Script/FSD.EffectSettings",
                //"/Script/FSD.DynamicIconSettings",
                //"/Script/FSD.DualMachinePistolsUpgrade",
                //"/Script/FSD.DrinkSettings",
                //"/Script/FSD.DeepDiveSettings",
                //"/Script/FSD.DebrisCarved",
                //"/Script/FSD.DanceSettings",
                //"/Script/FSD.DamageTagBonusUpgrade",
                //"/Script/FSD.DamageSettings",
                //"/Script/FSD.DailyDealSettings",
                //"/Script/FSD.CreditsResourceData",
                //"/Script/FSD.CommunityGoalSettings",
                //"/Script/FSD.CommunicationMutator",
                //"/Script/FSD.CharacterSettings",
                //"/Script/FSD.BuildRestriction",
                //"/Script/FSD.AlwaysLockedDLC",
                //"/Script/FSD.AfflictionSettings",
                //"/Script/FSD.AchievementList"

            };

            //MoveAssets(@"C:\Games\DRGNEW\FSD\Content", @"C:\Games\DRGNEW\FSD\Cooked",typestocopy,UE4Version.VER_UE4_27);


            string[] files = Directory.GetFiles(@"C:\Games\DRGNEW\FSD\Content", "*.uasset", SearchOption.AllDirectories);
			foreach (string file in files) {

				asset = new UAsset(file, UE4Version.VER_UE4_27, true);

                if (asset.assetType != EAssetType.Uncategorized) {
                    asset = new UAsset(file, UE4Version.VER_UE4_27, false);

                    switch (asset.assetType) {
                        case EAssetType.Blueprint:
                        case EAssetType.WidgetBlueprint:
                        case EAssetType.AnimBlueprint:
                            SerializeBPAsset();
                            break;
                        case EAssetType.DataTable:
                            SerializeDataTable();
                            break;
                        case EAssetType.StringTable:
                            SerializeStringTable();
                            break;
                        case EAssetType.UserDefinedEnum:
                            SerializeUserDefinedEnum();
                            break;
                        //case EAssetType.UserDefinedStruct:
                        //    break;
                        case EAssetType.Texture2D:
                            SerializeTexture();
                            break;
                        case EAssetType.Material:
                            SerializeMaterial();
                            break;
                        //case EAssetType.Font:
                        //    break;
                        case EAssetType.FontFace:
                            SerializeFontFace();
                            break;
                        case EAssetType.BlendSpaceBase:
                            SerializeBlendSpace();
                            break;
                        case EAssetType.CurveBase:
                            SerializeCurveBase();
                            break;
                        //case EAssetType.AnimSequence:
                        //    break;
                        //case EAssetType.SkeletalMesh:
                        //    break;
                        //case EAssetType.Skeleton:
                        //    break;
                        //case EAssetType.StaticMesh:
                        //    break;
                        //case EAssetType.AnimMontage:
                        //    break;
                        //case EAssetType.CameraAnim:
                        //    break;
                        //case EAssetType.LandscapeGrassType:
                        //    break;
                        case EAssetType.MaterialInstanceConstant:
                            SerializeMaterialInstanceConstant();
                            break;
                        case EAssetType.MaterialParameterCollection:
                            SerializeMaterialParameterCollection();
                            break;
                        //case EAssetType.MediaPlayer:
                        //    break;
                        //case EAssetType.MediaTexture:
                        //    break;
                        //case EAssetType.FileMediaSource:
                        //    break;
                        case EAssetType.PhycialMaterial:
                            SerializePhysicalMaterial();
                            break;
                        //case EAssetType.SubsurfaceProfile:
                        //    break;
                        default:

                            break;
                    }
                }

            }



            //ScanFiles(Directory.GetFiles(@"C:\Games\DRG\FSD\Content", "*.uasset", SearchOption.AllDirectories));
            //ScanAssetTypes(Directory.GetFiles(@"C:\Games\DRGNEW\FSD\Content", "*.uasset", SearchOption.AllDirectories), UE4Version.VER_UE4_27);
            //ScanAssetTypes(Directory.GetFiles(@"C:\Games\GRLatest\Ghostrunner\Content", "*.uasset", SearchOption.AllDirectories), UE4Version.VER_UE4_26);

            //var asset = new UAsset(@"C:\Users\LongerWarrior\Documents\Unreal Projects\4.27\FSD-Template-main\WindowsNoEditor\FSD\Content\Test\Test.uasset", UE4Version.VER_UE4_27, false);
            //var asset = new UAsset(@"C:\Games\DRGNEW\FSD\Content\Audio\SFX\WeaponsNTools\WPN_CryoSpray\CryoSprayFireCore_Cue.uasset", UE4Version.VER_UE4_27, false);

             Console.WriteLine();


        }


    }
}

