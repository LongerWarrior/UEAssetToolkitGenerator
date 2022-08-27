namespace UAssetAPI.StructTypes.SkeletalMesh;

public struct FSkinWeightOverrideInfo
{
    public readonly uint InfluencesOffset;
    public readonly byte NumInfluences;

    public FSkinWeightOverrideInfo(AssetBinaryReader reader) {
        InfluencesOffset = reader.ReadUInt32();
        NumInfluences = reader.ReadByte();
    }
}