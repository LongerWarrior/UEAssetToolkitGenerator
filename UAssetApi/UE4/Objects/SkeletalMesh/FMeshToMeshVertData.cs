namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FMeshToMeshVertData
{
    public readonly FVector4 PositionBaryCoordsAndDist;
    public readonly FVector4 NormalBaryCoordsAndDist;
    public readonly FVector4 TangentBaryCoordsAndDist;
    public readonly short[] SourceMeshVertIndices;
    public readonly float Weight;
    public readonly uint Padding;

    public FMeshToMeshVertData(AssetBinaryReader reader)
    {
        PositionBaryCoordsAndDist = new FVector4(reader);
        NormalBaryCoordsAndDist = new FVector4(reader);
        TangentBaryCoordsAndDist = new FVector4(reader);
        SourceMeshVertIndices = new short[4];
        for (int i = 0; i < 4; i++) {
            SourceMeshVertIndices[i] = reader.ReadInt16();
        }

        if (reader.Asset.GetCustomVersion<FReleaseObjectVersion>() < FReleaseObjectVersion.WeightFMeshToMeshVertData)
        {
            // Old version had "uint32 Padding[2]"
            var discard = reader.ReadUInt32();
            Padding = reader.ReadUInt32();
        }
        else
        {
            // New version has "float Weight and "uint32 Padding"
            Weight = reader.ReadSingle();
            Padding = reader.ReadUInt32();
        }
    }
}