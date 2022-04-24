using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace UAssetAPI
{

    public struct FReferencePose {
        public FName PoseName;
        public FTransform[] ReferencePose;

        public FReferencePose(FName poseName, FTransform[] referencePose) {
            PoseName = poseName;
            ReferencePose = referencePose;
        }

        public void Read(AssetBinaryReader reader) {
            PoseName = reader.ReadFName();

            int poselength = reader.ReadInt32();
            if (poselength > 0) {
                List<FTransform> poselist = new List<FTransform>();
                poselist.Add(new FTransform(reader.ReadQuat(), reader.ReadVector(), reader.ReadVector()));
                ReferencePose = poselist.ToArray();
            } else {
                ReferencePose = null;
            }
        }

        public JToken ToJson() {
            JObject res = new JObject();
            res.Add("PoseName", PoseName.ToName());

            if (ReferencePose != null) {
                JArray poses = new JArray();
                foreach (FTransform trans in ReferencePose) {
                    poses.Add(trans.ToString());
                }
                res.Add("ReferencePose", poses);
            } else {
                res.Add("ReferencePose", new JArray());
            }
            return res;
        }
    }

    public struct FAnimCurveType {
        public bool bMaterial;
        public bool bMorphtarget;

        public FAnimCurveType(bool bMaterial, bool bMorphtarget) {
            this.bMaterial = bMaterial;
            this.bMorphtarget = bMorphtarget;
        }
    }

    public struct FCurveMetaData {
        public FAnimCurveType Type;
        public FName[] LinkedBones;
        public byte MaxLOD;

        public void Read(AssetBinaryReader reader) {
            Type = new FAnimCurveType(reader.ReadInt32()!=0, reader.ReadInt32() != 0);
            int length = reader.ReadInt32();
            List<FName> bones = new List<FName>(length);
            if (length > 0) {
                for (var i = 0; i < length; i++) {
                    bones[i] = reader.ReadFName();
                }
            }
            MaxLOD = reader.ReadByte();
        }

    }

    public class FSmartNameMapping {
        public Dictionary<FName, Guid> GuidMap;
        public Dictionary<ushort, FName> UidMap;
        public Dictionary<FName, FCurveMetaData> CurveMetaDataMap;

        public void Read(AssetBinaryReader reader) {
            int length = reader.ReadInt32();
            if (length > 0) {
                CurveMetaDataMap = new Dictionary<FName, FCurveMetaData>(length);
                for (var i = 0; i < length; i++) {
                    FName name = reader.ReadFName();
                    FCurveMetaData curve = new FCurveMetaData();
                    curve.Read(reader);
                    CurveMetaDataMap[name] = curve;
                }
            } 
        }


    }

    public struct FBoneNode  {
        public EBoneTranslationRetargetingMode TranslationRetargetingMode;

        public FBoneNode(EBoneTranslationRetargetingMode translationRetargetingMode) {
            TranslationRetargetingMode = translationRetargetingMode;
        }
    }

    public struct FMeshBoneInfo {
        public FName Name;
        public int ParentIndex;

        public FMeshBoneInfo(FName name, int parentIndex) {
            Name = name;
            ParentIndex = parentIndex;
        }
    }
    public struct FReferenceSkeleton {
        public FMeshBoneInfo[] FinalRefBoneInfo;
        public FTransform[] FinalRefBonePose;
        public Dictionary<FName, int> FinalNameToIndexMap;

        public void Read(AssetBinaryReader reader) {
            int infolength = reader.ReadInt32();
            if (infolength > 0) {
                List<FMeshBoneInfo> infolist = new List<FMeshBoneInfo>();
                for (int i = 0; i < infolength; i++) {
                    infolist.Add(new FMeshBoneInfo(reader.ReadFName(), reader.ReadInt32()));
                }
                FinalRefBoneInfo = infolist.ToArray();
            } else {
                FinalRefBoneInfo = null;
            }

            int poselength = reader.ReadInt32();
            if (poselength > 0) {
                List<FTransform> poselist = new List<FTransform>();
                for (int i = 0; i < poselength; i++) {
                    poselist.Add(new FTransform(reader.ReadQuat(), reader.ReadVector(), reader.ReadVector()));
                }
                FinalRefBonePose = poselist.ToArray();
            } else {
                FinalRefBonePose = null;
            }

            int num = reader.ReadInt32();
            FinalNameToIndexMap = new Dictionary<FName, int>(num);
            for (var i = 0; i < num; ++i) {
                FinalNameToIndexMap[reader.ReadFName()] = reader.ReadInt32();
            }

            if (FinalRefBonePose != null) {
                AdjustBoneScales(FinalRefBonePose);
            }
        }

        public void AdjustBoneScales(FTransform[] transforms) {
            if (FinalRefBoneInfo.Length != transforms.Length)
                return;

            for (int boneIndex = 0; boneIndex < transforms.Length; boneIndex++) {
                var scale = GetBoneScale(transforms, boneIndex);
                transforms[boneIndex].Translation.Scale(scale);
            }
        }

        public FVector GetBoneScale(FTransform[] transforms, int boneIndex) {
            var scale = new FVector(1.0f);

            // Get the parent bone, ignore scale of the current one
            boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex;
            while (boneIndex >= 0) {
                var boneScale = transforms[boneIndex].Scale3D;
                // Accumulate the scale
                scale.Scale(boneScale);
                // Get the bone's parent
                boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex;
            }

            return scale;
        }

        public JToken ToJson() {
            JArray res = new JArray();
            foreach (FMeshBoneInfo bone in FinalRefBoneInfo) {
                JObject jbone = new JObject();

                jbone.Add("Name",bone.Name.ToName());
                jbone.Add("ParentIndex",bone.ParentIndex);
                FinalNameToIndexMap.TryGetValue(bone.Name, out int index);
                jbone.Add("Index",index);
                jbone.Add("Pose",FinalRefBonePose[index].ToString());
                res.Add(jbone);
            }

            return new JObject(new JProperty("Bones",res));
        }
    }


    public class SkeletonExport : NormalExport {
        //public FBoneNode[] BoneTree;
        public FReferenceSkeleton ReferenceSkeleton;
        public Guid Guid;
        //public Guid VirtualBoneGuid;
        public Dictionary<FName, FReferencePose> AnimRetargetSources;
        public Dictionary<FName, FSmartNameMapping> NameMappings;
        public FName[] ExistingMarkerNames;

        //static Dictionary<string, byte> BoneEnumToByte = new Dictionary<string, byte>()
        //{
        //    { "Animation", 0 },
        //    { "Skeleton", 1  },
        //    { "AnimationScaled", 2  },
        //    { "AnimationRelative", 3  },
        //    { "OrientAndScale", 4 },
        //};
        public SkeletonExport(Export super) : base(super) {

        }

        public SkeletonExport() {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting) {
            base.Read(reader, nextStarting);

            int idk = reader.ReadInt32();

            ReferenceSkeleton = new FReferenceSkeleton();
            ReferenceSkeleton.Read(reader);

            int numOfRetargetSources = reader.ReadInt32();
            AnimRetargetSources = new Dictionary<FName, FReferencePose>(numOfRetargetSources);
            for (var i = 0; i < numOfRetargetSources; i++) {
                FName name = reader.ReadFName();
                FReferencePose pose = new FReferencePose();
                pose.Read(reader);

                if (pose.ReferencePose != null) ReferenceSkeleton.AdjustBoneScales(pose.ReferencePose);
                AnimRetargetSources[reader.ReadFName()] = pose;
            }

            Guid = new Guid(reader.ReadBytes(16));

            int mapLength = reader.ReadInt32();
            NameMappings = new Dictionary<FName, FSmartNameMapping>(mapLength);
            for (var i = 0; i < mapLength; i++) {
                FName name = reader.ReadFName();
                FSmartNameMapping mapping = new FSmartNameMapping();
                mapping.Read(reader);

                NameMappings[name] = mapping;
            }

            byte GlobalStripFlags = reader.ReadByte();
            byte ClassStripFlags = reader.ReadByte();

            if (!((GlobalStripFlags & 1) != 0)) {
                //ExistingMarkerNames = Ar.ReadArray(Ar.ReadFName);
                throw new NotImplementedException("ExistingMarkerNames not implemented");
            }

        }

        public override void Write(AssetBinaryWriter writer) {
            base.Write(writer);

            writer.Write((int)0);

            //writer.Write(CollisionDisableTable.Count);
            //foreach (KeyValuePair<FRigidBodyIndexPair, bool> entry in CollisionDisableTable) {
            //    writer.Write(entry.Key.Indices[0]);
            //    writer.Write(entry.Key.Indices[1]);
            //    writer.Write(entry.Value ? 1 : 0);
            //}
        }
    }
}
