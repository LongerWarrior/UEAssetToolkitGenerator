using System.Drawing;

namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkeletalMeshVertexColorBuffer
{
    [JsonConverter(typeof(ColorJsonConverter))]
    public readonly Color[] Data;

    public FSkeletalMeshVertexColorBuffer()
    {
        Data = Array.Empty<Color>();
    }
    
    public FSkeletalMeshVertexColorBuffer(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader,reader.Ver >= UE4Version.VER_UE4_STATIC_SKELETAL_MESH_SERIALIZATION_FIX);
        Data = !stripDataFlags.IsDataStrippedForServer() ? reader.ReadBulkArray(() => Color.FromArgb(reader.ReadInt32())) : new Color[0];
    }

    public FSkeletalMeshVertexColorBuffer(Color[] data)
    {
        Data = data;
    }
}
