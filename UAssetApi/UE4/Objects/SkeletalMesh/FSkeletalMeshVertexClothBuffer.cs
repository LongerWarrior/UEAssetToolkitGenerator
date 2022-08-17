namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkeletalMeshVertexClothBuffer
{
    public readonly ulong[]? ClothIndexMapping;

    public FSkeletalMeshVertexClothBuffer(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader, reader.Ver >= UE4Version.VER_UE4_STATIC_SKELETAL_MESH_SERIALIZATION_FIX);
        if (stripDataFlags.IsDataStrippedForServer()) return;

        reader.SkipBulkArrayData();
        if (reader.Asset.GetCustomVersion < FSkeletalMeshCustomVersion>() >= FSkeletalMeshCustomVersion.CompactClothVertexBuffer)
        {
            ClothIndexMapping = reader.ReadArray(() => reader.ReadUInt64());
            //if (FUE5ReleaseStreamObjectVersion.Get(Ar) >= FUE5ReleaseStreamObjectVersion.Type.AddClothMappingLODBias)
            //{
            //    Ar.Position += ClothIndexMapping.Length * 4;
            //}
        }
    }
}