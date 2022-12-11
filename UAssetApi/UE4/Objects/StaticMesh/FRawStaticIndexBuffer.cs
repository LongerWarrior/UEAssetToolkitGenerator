namespace UAssetAPI.StructTypes.StaticMesh;

public class FRawStaticIndexBuffer
{
    public ushort[] Indices16; // LegacyIndices
    public uint[] Indices32;

    public FRawStaticIndexBuffer()
    {
        Indices16 = Array.Empty<ushort>();
        Indices32 = Array.Empty<uint>();
    }

    public FRawStaticIndexBuffer(AssetBinaryReader reader) : this()
    {
        if (reader.Ver < UE4Version.VER_UE4_SUPPORT_32BIT_STATIC_MESH_INDICES)
        {
            Indices16 = reader.ReadBulkArray(()=> reader.ReadUInt16());
        }
        else
        {
            var is32bit = reader.ReadIntBoolean();

            var size = reader.ReadInt32();
            var len = reader.ReadInt32();

            if (is32bit)
            {
                var count = (int)len / 4;
                Indices32 = new uint[count];
                for (int i = 0; i < count; i++) {
                    Indices32[i] = reader.ReadUInt32();
                }
            }
            else
            {
                var count = (int)len / 2;
                Indices16 = new ushort[count];
                for (int i = 0; i < count; i++) {
                    Indices16[i] = reader.ReadUInt16();
                }
            }

            if (reader["RawIndexBuffer.HasShouldExpandTo32Bit"]) {
                var bShouldExpandTo32Bit = reader.ReadIntBoolean();
            }

        }
    }
    
    public int Length
    {
        get
        {
            if (Indices32.Length > 0)
                return Indices32.Length;
            if (Indices16.Length > 0)
                return Indices16.Length;
            return -1;
        }
    }
    
    public int this[int i]
    {
        get
        {
            if (Indices32.Length > 0)
                return (int)Indices32[i];
            if (Indices16.Length > 0)
                return Indices16[i];
            throw new IndexOutOfRangeException();
        }
    }
}
