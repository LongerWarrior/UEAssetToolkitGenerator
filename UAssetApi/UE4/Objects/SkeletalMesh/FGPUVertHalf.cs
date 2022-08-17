using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FGPUVertHalf : FSkelMeshVertexBase
{
    private const int MAX_SKELETAL_UV_SETS_UE4 = 4;
    public readonly FMeshUVHalf[] UV;

    public FGPUVertHalf()
    {
        UV = Array.Empty<FMeshUVHalf>();
    }

    public FGPUVertHalf(AssetBinaryReader reader, bool bExtraBoneInfluences, int numSkelUVSets) : this()
    {
        SerializeForGPU(reader, bExtraBoneInfluences);

        UV = new FMeshUVHalf[MAX_SKELETAL_UV_SETS_UE4];
        for (var i = 0; i < numSkelUVSets; i++)
        {
            UV[i] = new FMeshUVHalf(reader);
        }
    }
}