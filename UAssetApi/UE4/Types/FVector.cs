namespace UAssetAPI;

/// <summary>
/// A vector in 3-D space composed of components (X, Y, Z) with floating point precision.
/// </summary>
public class FVector {
    /// <summary>Vector's X-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float X;

    /// <summary>Vector's Y-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Y;

    /// <summary>Vector's Z-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Z;

    public FVector(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }
    public FVector(float scale) {
        X = scale;
        Y = scale;
        Z = scale;
    }
    public FVector() {

    }
    public FVector(AssetBinaryReader reader) {
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
    }

    public FVector Scale(float scale) {
        return new FVector(X * scale, Y * scale, Z * scale);
    }

    public void Scale(FVector scale) {
        X *= scale.X;
        Y *= scale.Y;
        Z *= scale.Z;
    }
    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        res.Add(new JProperty("Z", Z));
        return res;
    }

    public static FVector operator ^(FVector a, FVector b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );

    public static float operator |(FVector a, FVector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static FVector operator +(FVector a, FVector b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static FVector operator +(FVector a, float bias) => new(a.X + bias, a.Y + bias, a.Z + bias);
    public static FVector operator -(FVector a, FVector b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static FVector operator -(FVector a, float bias) => new(a.X - bias, a.Y - bias, a.Z - bias);
    public static FVector operator *(FVector a, FVector b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static FVector operator *(FVector a, float scale) => new(a.X * scale, a.Y * scale, a.Z * scale);
    public static FVector operator *(float scale, FVector a) => a * scale;
    public static FVector operator /(FVector a, FVector b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static FVector operator /(FVector a, float scale) {
        var rScale = 1f / scale;
        return new FVector(a.X * rScale, a.Y * rScale, a.Z * rScale);
    }
    public static FVector operator /(float scale, FVector a) => a / scale;
    public static bool operator ==(FVector a, FVector b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(FVector a, FVector b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
}

