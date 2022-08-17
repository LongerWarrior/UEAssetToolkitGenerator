namespace UAssetAPI;

/// <summary>
/// A vector in 4-D space composed of components (X, Y, Z, W) with floating point precision.
/// </summary>
public class FVector4
{
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

    /// <summary>Vector's W-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float W;

    public FVector4(float x, float y, float z,float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public FVector4(float x) : this(x, x, x, x) { }
    public FVector4()
    {

    }
    public FVector4(AssetBinaryReader reader) {
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
        W = reader.ReadSingle();
    }

    public FVector4(FVector v, float w = 1f) : this(v.X, v.Y, v.Z, w) { }

    public FVector4(LinearColor color) : this(color.R, color.G, color.B, color.A) { }

    public static explicit operator FVector(FVector4 v) => new FVector(v.X, v.Y, v.Z);

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        res.Add(new JProperty("Z", Z));
        res.Add(new JProperty("W", W));
        return res;
    }

    
}
