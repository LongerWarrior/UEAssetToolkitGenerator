namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FRuntimeSkinWeightProfileData
{
    public readonly Dictionary<uint, uint> VertexIndexToInfluenceOffset;
    public readonly FSkinWeightOverrideInfo[] OverridesInfo;
    public readonly ushort[] Weights;
    public readonly byte[] BoneIDs;
    public readonly byte[] BoneWeights;
    public readonly byte NumWeightsPerVertex;

    public FRuntimeSkinWeightProfileData(AssetBinaryReader reader)
    {
        if (reader.Ver < UE4Version.VER_UE4_SKINWEIGHT_PROFILE_DATA_LAYOUT_CHANGES)
        {
            OverridesInfo = reader.ReadArray(() => new FSkinWeightOverrideInfo(reader));
            Weights = reader.ReadArray(() => reader.ReadUInt16());
        }
        else
        {
            // UE4.26+
            BoneIDs = reader.ReadBytes(reader.ReadInt32());
            BoneWeights = reader.ReadBytes(reader.ReadInt32());
            NumWeightsPerVertex = reader.ReadByte();
        }
        
        var length = reader.ReadInt32();
        VertexIndexToInfluenceOffset = new Dictionary<uint, uint>();
        for (var i = 0; i < length; i++)
        {
            VertexIndexToInfluenceOffset[reader.ReadUInt32()] = reader.ReadUInt32();
        }
    }
}