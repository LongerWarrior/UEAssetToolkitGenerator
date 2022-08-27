namespace UAssetAPI.StructTypes.SkeletalMesh;

public enum ESkinVertexColorChannel : byte
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Alpha = 3,
    None = Alpha
}


public class FSkelMeshSection
{
    public short MaterialIndex;
    public int BaseIndex;
    public int NumTriangles;
    public bool bRecomputeTangent;
    public ESkinVertexColorChannel RecomputeTangentsVertexMaskChannel;
    public bool bCastShadow;
    public bool bVisibleInRayTracing;
    [Obsolete]
    public bool bLegacyClothingSection;
    [Obsolete]
    public short CorrespondClothSectionIndex;
    public uint BaseVertexIndex;
    public FSoftVertex[] SoftVertices;
    public FMeshToMeshVertData[][] ClothMappingDataLODs;
    public ushort[] BoneMap;
    public int NumVertices;
    public int MaxBoneInfluences;
    public bool bUse16BitBoneIndex;
    public short CorrespondClothAssetIndex;
    public FClothingSectionData ClothingData;
    public Dictionary<int, int[]> OverlappingVertices;
    public bool bDisabled;
    public int GenerateUpToLodIndex;
    public int OriginalDataSectionIndex;
    public int ChunkedParentSectionIndex;

    public bool HasClothData => ClothMappingDataLODs.Any(data => data.Length > 0);

    public FSkelMeshSection()
    {
        RecomputeTangentsVertexMaskChannel = ESkinVertexColorChannel.None;
        bCastShadow = true;
        bVisibleInRayTracing = true;
        CorrespondClothSectionIndex = -1;
        SoftVertices = Array.Empty<FSoftVertex>();
        MaxBoneInfluences = 4;
        GenerateUpToLodIndex = -1;
        OriginalDataSectionIndex = -1;
        ChunkedParentSectionIndex = -1;
    }

    public FSkelMeshSection(AssetBinaryReader reader) : this()
    {
        var stripDataFlags = new FStripDataFlags(reader);
        var skelMeshVer = reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>();

        MaterialIndex = reader.ReadInt16();

        if (skelMeshVer < FSkeletalMeshCustomVersion.CombineSectionWithChunk)
        {
            var dummyChunkIndex = reader.ReadInt16();
        }

        if (!stripDataFlags.IsDataStrippedForServer())
        {
            BaseIndex = reader.ReadInt32();
            NumTriangles = reader.ReadInt32();
        }

        if (skelMeshVer < FSkeletalMeshCustomVersion.RemoveTriangleSorting)
        {
            var dummyTriangleSorting = reader.ReadByte(); // TEnumAsByte<ETriangleSortOption>
        }

        if (reader.Ver >=  UE4Version.VER_UE4_APEX_CLOTH)
        {
            if (skelMeshVer < FSkeletalMeshCustomVersion.DeprecateSectionDisabledFlag)
            {
                bLegacyClothingSection = reader.ReadIntBoolean();
            }

            if (skelMeshVer < FSkeletalMeshCustomVersion.RemoveDuplicatedClothingSections)
            {
                CorrespondClothSectionIndex = reader.ReadInt16();
            }
        }

        if (reader.Ver >= UE4Version.VER_UE4_APEX_CLOTH_LOD && skelMeshVer < FSkeletalMeshCustomVersion.RemoveEnableClothLOD)
        {
            var dummyEnableClothLOD = reader.ReadByte();
        }

        if (reader.Asset.GetCustomVersion<FRecomputeTangentCustomVersion>() >= FRecomputeTangentCustomVersion.RuntimeRecomputeTangent)
        {
            bRecomputeTangent = reader.ReadIntBoolean();
        }

        RecomputeTangentsVertexMaskChannel = reader.Asset.GetCustomVersion<FRecomputeTangentCustomVersion>() >= FRecomputeTangentCustomVersion.RecomputeTangentVertexColorMask ? (ESkinVertexColorChannel)reader.ReadByte() : ESkinVertexColorChannel.None;
        bCastShadow = reader.Asset.GetCustomVersion<FEditorObjectVersion>() < FEditorObjectVersion.RefactorMeshEditorMaterials || reader.ReadIntBoolean();
        bVisibleInRayTracing = true;//FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.Type.SkelMeshSectionVisibleInRayTracingFlagAdded || reader.ReadIntBoolean();

        if (skelMeshVer >= FSkeletalMeshCustomVersion.CombineSectionWithChunk)
        {
            if (!stripDataFlags.IsDataStrippedForServer())
            {
                BaseVertexIndex = reader.ReadUInt32();
            }

            if (!stripDataFlags.IsEditorDataStripped() && !reader.Asset.PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly))
            {
                if (skelMeshVer < FSkeletalMeshCustomVersion.CombineSoftAndRigidVerts)
                {
                    var legacyRigidVertices = reader.ReadArray(() => new FRigidVertex(reader));
                }

                SoftVertices = reader.ReadArray(() => new FSoftVertex(reader));
            }

            if (reader.Asset.GetCustomVersion<FAnimObjectVersion>() >= FAnimObjectVersion.IncreaseBoneIndexLimitPerChunk)
            {
                bUse16BitBoneIndex = reader.ReadIntBoolean();
            }

            BoneMap = reader.ReadArray(() => reader.ReadUInt16()); ;

            if (skelMeshVer >= FSkeletalMeshCustomVersion.SaveNumVertices)
            {
                NumVertices = reader.ReadInt32();
            }

            if (skelMeshVer < FSkeletalMeshCustomVersion.CombineSoftAndRigidVerts)
            {
                var dummyNumRigidVerts = reader.ReadInt32();
                var dummyNumSoftVerts = reader.ReadInt32();

                if (dummyNumRigidVerts + dummyNumSoftVerts != SoftVertices.Length)
                {
                    //Log.Error("Legacy NumSoftVerts + NumRigidVerts != SoftVertices.Num()");
                    Console.WriteLine("Legacy NumSoftVerts + NumRigidVerts != SoftVertices.Num()");
                }
            }

            MaxBoneInfluences = reader.ReadInt32();
            ClothMappingDataLODs = new[] { reader.ReadArray(() => new FMeshToMeshVertData(reader)) };//FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.AddClothMappingLODBias ? new[] { Ar.ReadArray(() => new FMeshToMeshVertData(Ar)) } : Ar.ReadArray(() => Ar.ReadArray(() => new FMeshToMeshVertData(Ar)));

            if (skelMeshVer < FSkeletalMeshCustomVersion.RemoveDuplicatedClothingSections)
            {
                var dummyPhysicalMeshVertices = reader.ReadArray(() => new FVector(reader));
                var dummyPhysicalMeshNormals = reader.ReadArray(() => new FVector(reader));
            }

            CorrespondClothAssetIndex = reader.ReadInt16();

            if (skelMeshVer < FSkeletalMeshCustomVersion.NewClothingSystemAdded)
            {
                var dummyClothAssetSubmeshIndex = reader.ReadInt32();
            }
            else
            {
                // UE4.16+
                ClothingData = new FClothingSectionData(reader);
            }

            if (reader.Asset.GetCustomVersion<FOverlappingVerticesCustomVersion>() >= FOverlappingVerticesCustomVersion.DetectOVerlappingVertices)
            {
                var size = reader.ReadInt32();
                OverlappingVertices = new Dictionary<int, int[]>(size);
                for (var i = 0; i < size; i++)
                {
                    OverlappingVertices[reader.ReadInt32()] = reader.ReadArray(() => reader.ReadInt32());
                }
            }

            if (reader.Asset.GetCustomVersion<FReleaseObjectVersion>() >= FReleaseObjectVersion.AddSkeletalMeshSectionDisable)
            {
                bDisabled = reader.ReadIntBoolean();
            }

            if (reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>() >= FSkeletalMeshCustomVersion.SectionIgnoreByReduceAdded)
            {
                GenerateUpToLodIndex = reader.ReadInt32();
            }
            else
            {
                GenerateUpToLodIndex = -1;
            }

            if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.SkeletalMeshBuildRefactor)
            {
                OriginalDataSectionIndex = reader.ReadInt32();
                ChunkedParentSectionIndex = reader.ReadInt32();
            }
            else
            {
                OriginalDataSectionIndex = -1;
                ChunkedParentSectionIndex = -1;
            }
        }
    }

    // Reference: FArchive& operator<<(FArchive& Ar, FSkelMeshRenderSection& S)
    public void SerializeRenderItem(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader);

        MaterialIndex = reader.ReadInt16();
        BaseIndex = reader.ReadInt32();
        NumTriangles = reader.ReadInt32();
        bRecomputeTangent = reader.ReadIntBoolean();
        RecomputeTangentsVertexMaskChannel = reader.Asset.GetCustomVersion<FRecomputeTangentCustomVersion>() >= FRecomputeTangentCustomVersion.RecomputeTangentVertexColorMask ? (ESkinVertexColorChannel)reader.ReadByte() : ESkinVertexColorChannel.None;
        bCastShadow = reader.Asset.GetCustomVersion<FEditorObjectVersion>() < FEditorObjectVersion.RefactorMeshEditorMaterials || reader.ReadIntBoolean();
        bVisibleInRayTracing = true;//FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.SkelMeshSectionVisibleInRayTracingFlagAdded || reader.ReadIntBoolean();
        BaseVertexIndex = reader.ReadUInt32();
        ClothMappingDataLODs = new[] { reader.ReadArray(() => new FMeshToMeshVertData(reader)) };//reader.ReadArray(() => reader.ReadArray(() => new FMeshToMeshVertData(reader)));//FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.AddClothMappingLODBias ? new[] { Ar.ReadArray(() => new FMeshToMeshVertData(Ar)) } : Ar.ReadArray(() => Ar.ReadArray(() => new FMeshToMeshVertData(Ar)));
        BoneMap = reader.ReadArray(() => reader.ReadUInt16());
        NumVertices = reader.ReadInt32();
        MaxBoneInfluences = reader.ReadInt32();
        CorrespondClothAssetIndex = reader.ReadInt16();
        ClothingData = new FClothingSectionData(reader);

        if (reader.Ver < UE4Version.VER_UE4_23 || !stripDataFlags.IsClassDataStripped(1)) // DuplicatedVertices, introduced in UE4.23
        {
            reader.SkipFixedArray(4); // DupVertData
            reader.SkipFixedArray(8); // DupVertIndexData
        }

        if (reader.Asset.GetCustomVersion<FReleaseObjectVersion>() >= FReleaseObjectVersion.AddSkeletalMeshSectionDisable)
        {
            bDisabled = reader.ReadIntBoolean();
        }

    }
}