namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkinWeightInfo
{
    private const int NUM_INFLUENCES_UE4 = 4;
    private const int MAX_TOTAL_INFLUENCES_UE4 = 8;

    [JsonConverter(typeof(FSkinWeightInfoConverter))]
    public readonly byte[] BoneIndex;
    [JsonConverter(typeof(FSkinWeightInfoConverter))]
    public readonly byte[] BoneWeight;

    public FSkinWeightInfo()
    {
        BoneIndex = new byte[NUM_INFLUENCES_UE4];
        BoneWeight = new byte[NUM_INFLUENCES_UE4];
    }

    public FSkinWeightInfo(AssetBinaryReader reader, bool bExtraBoneInfluences) : this()
    {
        var numSkelInfluences = bExtraBoneInfluences ? MAX_TOTAL_INFLUENCES_UE4 : NUM_INFLUENCES_UE4;
        if (numSkelInfluences <= BoneIndex.Length)
        {
            for (var i = 0; i < numSkelInfluences; i++)
                BoneIndex[i] = reader.ReadByte();
            for (var i = 0; i < numSkelInfluences; i++)
                BoneWeight[i] = reader.ReadByte();
        }
        else
        {
            var boneIndex2 = new byte[MAX_TOTAL_INFLUENCES_UE4];
            var boneWeight2 = new byte[MAX_TOTAL_INFLUENCES_UE4];
            for (var i = 0; i < numSkelInfluences; i++)
                boneIndex2[i] = reader.ReadByte();
            for (var i = 0; i < numSkelInfluences; i++)
                boneWeight2[i] = reader.ReadByte();

            // copy influences to vertex
            for (var i = 0; i < NUM_INFLUENCES_UE4; i++)
            {
                BoneIndex[i] = boneIndex2[i];
                BoneWeight[i] = boneWeight2[i];
            }
        }
    }

    public int ConvertToInt(byte[] value) => BitConverter.ToInt32(value, 0);
    public long ConvertToLong(byte[] value) => BitConverter.ToInt64(value, 0);
}