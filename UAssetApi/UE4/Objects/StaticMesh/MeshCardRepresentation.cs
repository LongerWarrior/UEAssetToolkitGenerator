namespace UAssetAPI.StructTypes.StaticMesh;

public struct FLumenCardBuildData
{
    public FLumenCardOBB OBB;
    public byte LODLevel;
    public byte AxisAlignedDirectionIndex;

    public FLumenCardBuildData(AssetBinaryReader reader) {
        OBB = new FLumenCardOBB(reader);
        LODLevel = reader.ReadByte();
        AxisAlignedDirectionIndex = reader.ReadByte();
    }
}

public struct FLumenCardOBB
{
    public FVector Origin, AxisX, AxisY, AxisZ, Extent;

    public FLumenCardOBB(AssetBinaryReader reader) {
        Origin = reader.ReadVector();
        AxisX = reader.ReadVector();
        AxisY = reader.ReadVector();
        AxisZ = reader.ReadVector();
        Extent = reader.ReadVector();
    }
}

public class FCardRepresentationData
{
    public FBox Bounds;
    public int MaxLodLevel;
    public FLumenCardBuildData[] CardBuildData;

    public FCardRepresentationData(AssetBinaryReader reader)
    {
        Bounds = new FBox(reader);
        MaxLodLevel = reader.ReadInt32();
        var len = reader.ReadInt32();
        CardBuildData = new FLumenCardBuildData[len];
        for (int i = 0; i < len; i++) {
            CardBuildData[i] = new FLumenCardBuildData(reader);
        }
    }
}