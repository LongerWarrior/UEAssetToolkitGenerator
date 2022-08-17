using System.Drawing;
using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSoftVertex : FSkelMeshVertexBase
{
    private const int MAX_SKELETAL_UV_SETS_UE4 = 4;

    public FMeshUVFloat[] UV;
    public Color Color;

    public FSoftVertex(AssetBinaryReader reader, bool isRigid = false)
    {
        SerializeForEditor(reader);

        UV = new FMeshUVFloat[MAX_SKELETAL_UV_SETS_UE4];
        for (var i = 0; i < UV.Length; i++)
            UV[i] = new FMeshUVFloat(reader);

        Color = Color.FromArgb(reader.ReadInt32());
        if (!isRigid)
        {
            Infs = new FSkinWeightInfo(reader, reader.Ver >=  UE4Version.VER_UE4_SUPPORT_8_BONE_INFLUENCES_SKELETAL_MESHES);
        }
        else
        {
            Infs = new FSkinWeightInfo();
            Infs.BoneIndex[0] = reader.ReadByte();
            Infs.BoneWeight[0] = 255;
        }
    }
}

public class FRigidVertex : FSoftVertex
{
    public FRigidVertex(AssetBinaryReader reader) : base(reader, true) { }
}