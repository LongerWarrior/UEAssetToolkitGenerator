using System;
using System.Collections.Generic;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace UAssetAPI
{
    public struct FBoxSphereBounds {
        /** Holds the origin of the bounding box and sphere. */
        public FVector Origin;
        /** Holds the extent of the bounding box. */
        public FVector BoxExtent;
        /** Holds the radius of the bounding sphere. */
        public float SphereRadius;

        public FBoxSphereBounds(FVector origin, FVector boxExtent, float sphereRadius) {
            Origin = origin;
            BoxExtent = boxExtent;
            SphereRadius = sphereRadius;
        }
    }

    public struct FMeshUVChannelInfo {
        public bool bInitialized;
        public bool bOverrideDensities;
        public float[] LocalUVDensities;
        const int TEXSTREAM_MAX_NUM_UVCHANNELS = 4;

        public FMeshUVChannelInfo(AssetBinaryReader reader) {
            bInitialized = reader.ReadInt32()!=0;
            bOverrideDensities = reader.ReadInt32()!=0;
            LocalUVDensities = new float[TEXSTREAM_MAX_NUM_UVCHANNELS];
            for (int i = 0; i < TEXSTREAM_MAX_NUM_UVCHANNELS; i++) {
                LocalUVDensities[i] = reader.ReadSingle();
            }
        }
    }

    public struct FSkeletalMaterial {
        //public ResolvedObject Material; // UMaterialInterface
        public FPackageIndex Material; // UMaterialInterface
        public FName MaterialSlotName;
        public FName ImportedMaterialSlotName;
        public FMeshUVChannelInfo UVChannelData;

        public void Read(AssetBinaryReader reader) {

            Material = reader.XFER_OBJECT_POINTER();
            MaterialSlotName = reader.ReadFName();

            if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.RefactorMeshEditorMaterials) {
                var bSerializeImportedMaterialSlotName = !reader.Asset.PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly);
                if (reader.Asset.GetCustomVersion<FCoreObjectVersion>() >= FCoreObjectVersion.SkeletalMaterialEditorDataStripping) {
                    bSerializeImportedMaterialSlotName = reader.ReadInt32() != 0;
                }

                if (bSerializeImportedMaterialSlotName) {
                    ImportedMaterialSlotName = reader.ReadFName();
                }
               
            } else {
                throw new NotImplementedException("FEditorObjectVersion lower than"+FEditorObjectVersion.RefactorMeshEditorMaterials);
            }
            if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() >= FRenderingObjectVersion.TextureStreamingMeshUVChannelData)
                UVChannelData = new FMeshUVChannelInfo(reader);
        }


    }





    public struct FStaticLODModel {
        //public FSkelMeshSection[] Sections;
        //public FMultisizeIndexContainer Indices;
        //public short[] ActiveBoneIndices;
        //public FSkelMeshChunk[] Chunks;
        //public int Size;
        //public int NumVertices;
        //public short[] RequiredBones;
        //public FIntBulkData RawPointIndices;
        //public int[] MeshToImportVertexMap;
        //public int MaxImportVertex;
        //public int NumTexCoords;
        //public FSkeletalMeshVertexBuffer VertexBufferGPUSkin;
        //public FSkeletalMeshVertexColorBuffer ColorVertexBuffer;
        //public FMultisizeIndexContainer AdjacencyIndexBuffer;
        //public FSkeletalMeshVertexClothBuffer ClothVertexBuffer;
        //public bool SkipLod => Indices == null || Indices.Indices16.Length < 1 && Indices.Indices32.Length < 1;
    }

        /// <summary>
        /// Export for a skeletal mesh asset.
        /// </summary>
        public class SkeletalMeshExport : NormalExport
    {
        public FBoxSphereBounds ImportedBounds;
        public FSkeletalMaterial[] Materials;
        public FReferenceSkeleton ReferenceSkeleton;
        public FStaticLODModel[] LODModels;
        public bool bHasVertexColors;
        public byte NumVertexColorChannels;
        public SkeletalMeshExport(Export super) : base(super)
        {

        }

        public SkeletalMeshExport()
        {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting)
        {
            base.Read(reader, nextStarting);
            reader.ReadInt32();
            
            byte GlobalStripFlags = reader.ReadByte();
            byte ClassStripFlags = reader.ReadByte();

            ImportedBounds = new FBoxSphereBounds(reader.ReadVector(), reader.ReadVector(), reader.ReadSingle());
            int length = reader.ReadInt32();
            Materials = new FSkeletalMaterial[length];
            for (int i = 0; i < length; i++) {
                Materials[i].Read(reader);
            }
            ReferenceSkeleton = new FReferenceSkeleton();
            ReferenceSkeleton.Read(reader);
            




            Console.WriteLine();
            //if (!((GlobalStripFlags & 2) != 0)) {
            //    int length = reader.ReadInt32();
            //    MorphLODModels = new FMorphTargetLODModel[length];
            //    for (int i = 0; i < length; i++) {
            //        MorphLODModels[i].Read(reader);
            //    }


            //} else {
            //    throw new NotImplementedException("ExistingMarkerNames not implemented");
            //}



            //CollisionDisableTable = new Dictionary<FRigidBodyIndexPair, bool>();

            //int numEntries = reader.ReadInt32();
            //for (int i = 0; i < numEntries; i++)
            //{
            //    CollisionDisableTable.Add(new FRigidBodyIndexPair(reader.ReadInt32(), reader.ReadInt32()), reader.ReadInt32() != 0);
            //}
        }

        public override void Write(AssetBinaryWriter writer) {
            base.Write(writer);

            writer.Write((int)0);

        }
    }
}
