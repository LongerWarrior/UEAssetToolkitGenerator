namespace UAssetAPI.StructTypes.SkeletalMesh;
public class FSkelMeshChunk
{
    public readonly int BaseVertexIndex;
    public readonly FRigidVertex[] RigidVertices;
    public readonly FSoftVertex[] SoftVertices;
    public readonly ushort[] BoneMap;
    public readonly int NumRigidVertices;
    public readonly int NumSoftVertices;
    public readonly int MaxBoneInfluences;
    public readonly bool HasClothData;

    public FSkelMeshChunk(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader);

        if (!stripDataFlags.IsDataStrippedForServer())
            BaseVertexIndex = reader.ReadInt32();

        if (!stripDataFlags.IsEditorDataStripped())
        {
            RigidVertices = reader.ReadArray(() => new FRigidVertex(reader));
            SoftVertices = reader.ReadArray(() => new FSoftVertex(reader));
        }

        BoneMap = reader.ReadArray(() => reader.ReadUInt16());
        NumRigidVertices = reader.ReadInt32();
        NumSoftVertices = reader.ReadInt32();
        MaxBoneInfluences = reader.ReadInt32();
        HasClothData = false;

        if (reader.Ver >=  UE4Version.VER_UE4_APEX_CLOTH)
        {
            // Physics data, drop
            var clothMappingData = reader.ReadArray(() => new FMeshToMeshVertData(reader));

            reader.ReadArray(() => new FVector(reader)); // PhysicalMeshVertices
            reader.ReadArray(() => new FVector(reader)); // PhysicalMeshNormals
            reader.BaseStream.Position += 4; // CorrespondClothAssetIndex, ClothAssetSubmeshIndex
            HasClothData = clothMappingData.Length > 0;
        }
    }
}