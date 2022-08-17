namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FMultisizeIndexContainer
{
    public ushort[] Indices16;
    public uint[] Indices32;

    public FMultisizeIndexContainer()
    {
        Indices16 = Array.Empty<ushort>();
        Indices32 = Array.Empty<uint>();
    }
    
    public FMultisizeIndexContainer(AssetBinaryReader reader) : this()
    {
        if (reader.Ver < UE4Version.VER_UE4_KEEP_SKEL_MESH_INDEX_DATA)
        {
            reader.BaseStream.Position += 4; //var bOldNeedsCPUAccess = Ar.ReadBoolean();
        }
        
        var dataSize = reader.ReadByte();
        if (dataSize == 0x02)
        {
            var size = reader.ReadInt32();
            var len = reader.ReadInt32();
            Indices16 = new ushort[len];
            for (int i = 0; i < len; i++) {
                Indices16[i] = reader.ReadUInt16();
            }
        }
        else
        {
            var size = reader.ReadInt32();
            var len = reader.ReadInt32();
            Indices32 = new uint[len];
            for (int i = 0; i < len; i++) {
                Indices32[i] = reader.ReadUInt32();
            }
        }
    }
}