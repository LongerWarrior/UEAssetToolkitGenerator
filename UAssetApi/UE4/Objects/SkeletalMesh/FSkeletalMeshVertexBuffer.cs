namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkeletalMeshVertexBuffer
{
    public int NumTexCoords;
    public FVector MeshExtension;
    public FVector MeshOrigin;
    public bool bUseFullPrecisionUVs;
    public bool bExtraBoneInfluences;
    public FGPUVertHalf[] VertsHalf;
    public FGPUVertFloat[] VertsFloat;

    public FSkeletalMeshVertexBuffer()
    {
        VertsHalf = Array.Empty<FGPUVertHalf>();
        VertsFloat = Array.Empty<FGPUVertFloat>();
    }

    public FSkeletalMeshVertexBuffer(AssetBinaryReader reader) : this()
    {
        var stripDataFlags = new FStripDataFlags(reader, reader.Ver >= UE4Version.VER_UE4_STATIC_SKELETAL_MESH_SERIALIZATION_FIX);

        NumTexCoords = reader.ReadInt32(); ;
        bUseFullPrecisionUVs = reader.ReadIntBoolean();

        if (reader.Ver >= UE4Version.VER_UE4_SUPPORT_GPUSKINNING_8_BONE_INFLUENCES &&
            reader.Asset.GetCustomVersion <FSkeletalMeshCustomVersion>() < FSkeletalMeshCustomVersion.UseSeparateSkinWeightBuffer)
        {
            bExtraBoneInfluences = reader.ReadIntBoolean();
        }

        MeshExtension = reader.ReadVector();
        MeshOrigin = reader.ReadVector();

        if (!bUseFullPrecisionUVs)
            VertsHalf = reader.ReadBulkArray(() => new FGPUVertHalf(reader, bExtraBoneInfluences, NumTexCoords));
        else
            VertsFloat = reader.ReadBulkArray(() => new FGPUVertFloat(reader, bExtraBoneInfluences, NumTexCoords));
    }

    public int GetVertexCount()
    {
        if (VertsHalf.Length > 0) return VertsHalf.Length;
        if (VertsFloat.Length > 0) return VertsFloat.Length;
        return 0;
    }
}