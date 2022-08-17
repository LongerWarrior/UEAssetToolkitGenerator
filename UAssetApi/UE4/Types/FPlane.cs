using static System.MathF;


namespace UAssetAPI;

/// <summary>
/// A vector in 3-D space composed of components (X, Y, Z) with floating point precision.
/// </summary>
public class FPlane
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

    public FPlane(float x, float y, float z, float w)
    {
        
        Y = y;
        Z = z;
        W = w;
    }

    public FPlane()
    {

    }

    public FPlane(AssetBinaryReader reader) {
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
        Z = reader.ReadSingle();
        W = reader.ReadSingle();
    }
    public FPlane(FVector @base, FVector normal) {
        X = @base.X;
        Y = @base.Y;
        Z = @base.Z;
        W = @base | normal;
    }
    public float PlaneDot(FVector p) => X * p.X + Y * p.Y + Z * p.Z - W;

    public bool Equals(FPlane v, float tolerance = 1e-4f) => Abs(X - v.X) <= tolerance && Abs(Y - v.Y) <= tolerance && Abs(Z - v.Z) <= tolerance && Abs(W - v.W) <= tolerance;

    public override bool Equals(object? obj) => obj is FPlane other && Equals(other, 0f);

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        res.Add(new JProperty("Z", Z));
        res.Add(new JProperty("W", W));
        return res;
    }
}
