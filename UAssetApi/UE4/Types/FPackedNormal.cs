namespace UAssetAPI;

public class FPackedNormal {
    public uint Data;
    public float X => (Data & 0xFF) / (float)127.5 - 1;
    public float Y => ((Data >> 8) & 0xFF) / (float)127.5 - 1;
    public float Z => ((Data >> 16) & 0xFF) / (float)127.5 - 1;
    public float W => ((Data >> 24) & 0xFF) / (float)127.5 - 1;

    public FPackedNormal(AssetBinaryReader reader) {
        Data = reader.ReadUInt32();
        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() >= FRenderingObjectVersion.IncreaseNormalPrecision)
            Data ^= 0x80808080;
    }

    public FPackedNormal(uint data) {
        Data = data;
    }

    public FPackedNormal(FVector vector) {
        Data = (uint)((int)(vector.X + 1 * 127.5) + (int)(vector.Y + 1 * 127.5) << 8 + (int)(vector.Z + 1 * 127.5) << 16);
    }

    public FPackedNormal(FVector4 vector)// TODO: is this broken?
    {
        Data = (uint)((int)(vector.X + 1 * 127.5) + (int)(vector.Y + 1 * 127.5) << 8 + (int)(vector.Z + 1 * 127.5) << 16 + (int)(vector.W + 1 * 127.5) << 24);
    }

    public void SetW(float value) {
        Data = (Data & 0xFFFFFF) | (uint)((int)Math.Round(value * 127.0f) << 24);
    }

    public float GetW() {
        return (byte)(Data >> 24) / 127.0f;
    }

    public static explicit operator FVector(FPackedNormal packedNormal) => new(packedNormal.X, packedNormal.Y, packedNormal.Z);
    public static implicit operator FVector4(FPackedNormal packedNormal) => new(packedNormal.X, packedNormal.Y, packedNormal.Z, packedNormal.W);
    //public static explicit operator Vector3(FPackedNormal packedNormal) => new(packedNormal.X, packedNormal.Y, packedNormal.Z);
    //public static implicit operator Vector4(FPackedNormal packedNormal) => new(packedNormal.X, packedNormal.Y, packedNormal.Z, packedNormal.W);

    public static bool operator ==(FPackedNormal a, FPackedNormal b) => a.Data == b.Data && a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(FPackedNormal a, FPackedNormal b) => a.Data != b.Data || a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
}