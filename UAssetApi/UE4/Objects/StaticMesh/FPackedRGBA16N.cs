namespace UAssetAPI.StructTypes.StaticMesh;

public class FPackedRGBA16N {
    public readonly uint X;
    public readonly uint Y;
    public readonly uint Z;
    public readonly uint W;

    public FPackedRGBA16N(AssetBinaryReader reader) {
        X = reader.ReadUInt16();
        Y = reader.ReadUInt16();
        Z = reader.ReadUInt16();
        W = reader.ReadUInt16();

        if (reader.Ver >= UE4Version.VER_UE4_20) {
            X ^= 0x8000;
            Y ^= 0x8000;
            Z ^= 0x8000;
            W ^= 0x8000;
        }
    }

    public static explicit operator FVector(FPackedRGBA16N packedRGBA16N) {
        var X = (packedRGBA16N.X - (float)32767.5) / (float)32767.5;
        var Y = (packedRGBA16N.Y - (float)32767.5) / (float)32767.5;
        var Z = (packedRGBA16N.Z - (float)32767.5) / (float)32767.5;

        return new FVector(X, Y, Z);
    }

    public static explicit operator FVector4(FPackedRGBA16N packedRGBA16N) {
        var X = (packedRGBA16N.X - (float)32767.5) / (float)32767.5;
        var Y = (packedRGBA16N.Y - (float)32767.5) / (float)32767.5;
        var Z = (packedRGBA16N.Z - (float)32767.5) / (float)32767.5;
        var W = (packedRGBA16N.W - (float)32767.5) / (float)32767.5;

        return new FVector4(X, Y, Z, W);
    }

    public static explicit operator FPackedNormal(FPackedRGBA16N packedRGBA16N) {
        return new((FVector)packedRGBA16N);
    }
}

