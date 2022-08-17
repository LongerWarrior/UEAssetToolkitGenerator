namespace UAssetAPI.StructTypes.StaticMesh;

public static class DistanceField
{
    public const int NumMips = 3;
}

public struct FSparseDistanceFieldMip
{
    public FIntVector IndirectionDimensions;
    public int NumDistanceFieldBricks;
    public FVector VolumeToVirtualUVScale;
    public FVector VolumeToVirtualUVAdd;
    public FVector2D DistanceFieldToVolumeScaleBias;
    public uint BulkOffset;
    public uint BulkSize;

    public FSparseDistanceFieldMip(AssetBinaryReader reader) {
        IndirectionDimensions = new FIntVector(reader);
        NumDistanceFieldBricks = reader.ReadInt32();
        VolumeToVirtualUVScale = reader.ReadVector();
        VolumeToVirtualUVAdd = reader.ReadVector();
        DistanceFieldToVolumeScaleBias = new FVector2D(reader);
        BulkOffset = reader.ReadUInt32();
        BulkSize = reader.ReadUInt32();
    }
}

public class FDistanceFieldVolumeData
{
    public ushort[] DistanceFieldVolume; // LegacyIndices
    public FIntVector Size;
    public FBox LocalBoundingBox;
    public bool bMeshWasClosed;
    public bool bBuiltAsIfTwoSided;
    public bool bMeshWasPlane;

    public byte[] CompressedDistanceFieldVolume;
    public FVector2D DistanceMinMax;

    public FDistanceFieldVolumeData(AssetBinaryReader reader)
    {
        if (reader.Ver >= UE4Version.VER_UE4_16)
        {
            var len = reader.ReadInt32();
            CompressedDistanceFieldVolume = reader.ReadBytes(len);
            Size = new FIntVector(reader);
            LocalBoundingBox = new FBox(reader);
            DistanceMinMax = new FVector2D(reader);
            bMeshWasClosed = reader.ReadIntBoolean();
            bBuiltAsIfTwoSided = reader.ReadIntBoolean();
            bMeshWasPlane = reader.ReadIntBoolean();
            DistanceFieldVolume = new ushort[0];
        }
        else
        {
            DistanceFieldVolume = reader.ReadArray(() => reader.ReadUInt16());
            Size = new FIntVector(reader);
            LocalBoundingBox = new FBox(reader);
            bMeshWasClosed = reader.ReadIntBoolean();
            bBuiltAsIfTwoSided =reader.Ver >= UE4Version.VER_UE4_RENAME_CROUCHMOVESCHARACTERDOWN && reader.ReadIntBoolean();
            bMeshWasPlane = reader.Ver >= UE4Version.VER_UE4_DEPRECATE_UMG_STYLE_OVERRIDES && reader.ReadIntBoolean();
            CompressedDistanceFieldVolume = new byte[0];
            DistanceMinMax = new FVector2D(0f, 0f);
        }
    }
}

public class FDistanceFieldVolumeData5
{
    /** Local space bounding box of the distance field volume. */
    public FBox LocalSpaceMeshBounds;

    /** Whether most of the triangles in the mesh used a two-sided material. */
    public bool bMostlyTwoSided;

    public FSparseDistanceFieldMip[] Mips;

    // Lowest resolution mip is always loaded so we always have something
    public byte[] AlwaysLoadedMip;

    // Remaining mips are streamed
    public FByteBulkData StreamableMips;

    public FDistanceFieldVolumeData5(AssetBinaryReader reader)
    {
        LocalSpaceMeshBounds = new FBox(reader);
        bMostlyTwoSided = reader.ReadIntBoolean();
        Mips = new FSparseDistanceFieldMip[DistanceField.NumMips];
        for (int i = 0; i < DistanceField.NumMips; i++) {
            Mips[i] = new FSparseDistanceFieldMip(reader);
        }
        var len = reader.ReadInt32();
        AlwaysLoadedMip = reader.ReadBytes(len);
        StreamableMips = new FByteBulkData(reader);
    }
}