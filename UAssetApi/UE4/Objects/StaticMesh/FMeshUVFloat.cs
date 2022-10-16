using static System.BitConverter;

namespace UAssetAPI.StructTypes.StaticMesh;

public struct FMeshUVFloat : IUStruct {
    public float U;
    public float V;

    public FMeshUVFloat(float u, float v) {
        U = u;
        V = v;
    }

    public FMeshUVFloat(AssetBinaryReader reader) {
        U = reader.ReadSingle();
        V = reader.ReadSingle();
    }

    public static float HalfToFloat(ushort fp16) {
        const uint shiftedExp = 0x7c00 << 13;       // exponent mask after shift
        var magic = Int32BitsToSingle(113 << 23);

        var fp32 = (fp16 & 0x7fff) << 13;           // exponent/mantissa bits
        var exp = shiftedExp & fp32;                // just the exponent
        fp32 += (127 - 15) << 23;                   // exponent adjust

        // handle exponent special cases
        if (exp == shiftedExp)                      // Inf/NaN?
        {
            fp32 += (128 - 16) << 23;               // extra exp adjust
        } else if (exp == 0)                            // Zero/Denormal?
          {
            fp32 += 1 << 23;                        // extra exp adjust
            fp32 = SingleToInt32Bits(Int32BitsToSingle(fp32) - magic); // renormalize
        }

        fp32 |= (fp16 & 0x8000) << 16;              // sign bit
        var halfToFloat = Int32BitsToSingle(fp32);
        return halfToFloat;
    }

    //public void Serialize(FArchiveWriter Ar) {
    //    Ar.Write(U);
    //    Ar.Write(V);
    //}

    //public static implicit operator Vector2(FMeshUVFloat uv) => new(uv.U, uv.V);
    public static explicit operator FMeshUVFloat(FMeshUVHalf uvHalf) => new(HalfToFloat(uvHalf.U), HalfToFloat(uvHalf.V));
}

