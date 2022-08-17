namespace UAssetAPI;

/// <summary>
/// A vector in 3-D space composed of components (X, Y, Z) with floating point precision.
/// </summary>
public class FVector2D
{
    /// <summary>Vector's X-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float X;

    /// <summary>Vector's Y-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Y;


    public FVector2D(float x, float y)
    {
        X = x;
        Y = y;
    }
    public FVector2D(float scale) {
        X = scale;
        Y = scale;
    }
    public FVector2D()
    {

    }

    public FVector2D(AssetBinaryReader reader) {
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
    }

    public FVector2D Scale(float scale) {
        return new FVector2D(X * scale, Y * scale);
    }

    public void Scale(FVector2D scale) {
        X *= scale.X;
        Y *= scale.Y;
    }
    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        return res;
    }
}
