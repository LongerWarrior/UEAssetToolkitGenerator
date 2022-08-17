namespace UAssetAPI;

public struct FDeprecatedSerializedPackedNormal {
    public uint Data;

    public FDeprecatedSerializedPackedNormal(AssetBinaryReader reader) {
        Data = reader.ReadUInt32();
    }

    public static FVector4 VectorMultiplyAdd(FVector4 vec1, FVector4 vec2, FVector4 vec3) =>
        new(vec1.X * vec2.X + vec3.X, vec1.Y * vec2.Y + vec3.Y, vec1.Z * vec2.Z + vec3.Z, vec1.W * vec2.W + vec3.W);

    public static explicit operator FVector4(FDeprecatedSerializedPackedNormal packed) {
        var vectorToUnpack = new FVector4(packed.Data & 0xFF, (packed.Data >> 8) & 0xFF, (packed.Data >> 16) & 0xFF, (packed.Data >> 24) & 0xFF);
        return VectorMultiplyAdd(vectorToUnpack, new FVector4(1.0f / 127.5f), new FVector4(-1.0f));
    }

    public static explicit operator FVector(FDeprecatedSerializedPackedNormal packed) => (FVector)(FVector4)packed;
}