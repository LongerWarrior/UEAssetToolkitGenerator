using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FGPUVertFloat : FSkelMeshVertexBase
{
    private const int MAX_SKELETAL_UV_SETS_UE4 = 4;
    public FMeshUVFloat[] UV;

    public FGPUVertFloat()
    {
        UV = Array.Empty<FMeshUVFloat>();
    }

    public FGPUVertFloat(AssetBinaryReader reader, bool bExtraBoneInfluences, int numSkelUVSets) : this()
    {
        SerializeForGPU(reader, bExtraBoneInfluences);

        UV = new FMeshUVFloat[MAX_SKELETAL_UV_SETS_UE4];
        for (var i = 0; i < numSkelUVSets; i++)
        {
            UV[i] = new FMeshUVFloat(reader);
        }
    }
}