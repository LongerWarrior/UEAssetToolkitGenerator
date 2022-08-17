namespace UAssetAPI.StructTypes;

/// <summary>
/// A linear, 32-bit/component floating point RGBA color.
/// </summary>
public class LinearColor : ICloneable
{
    [JsonProperty]
    public float R;
    [JsonProperty]
    public float G;
    [JsonProperty]
    public float B;
    [JsonProperty]
    public float A;

    public LinearColor()
    {

    }

    public LinearColor(float R, float G, float B, float A)
    {
        this.R = R;
        this.G = G;
        this.B = B;
        this.A = A;
    }

    public object Clone()
    {
        return new LinearColor(this.R, this.G, this.B, this.A);
    }

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("R", R));
        res.Add(new JProperty("G", G));
        res.Add(new JProperty("B", B));
        res.Add(new JProperty("A", A));
        return res;
    }
}
