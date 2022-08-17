namespace UAssetAPI;

/// <summary>
/// A vector in 3-D space composed of components (X, Y, Z).
/// </summary>
public class FIntVector
{
    /// <summary>Vector's X-component.</summary>
    [JsonProperty]
    public int X;

    /// <summary>Vector's Y-component.</summary>
    [JsonProperty]
    public int Y;

    /// <summary>Vector's Z-component.</summary>
    [JsonProperty]
    public int Z;

    public FIntVector(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public FIntVector()
    {

    }
    public FIntVector(AssetBinaryReader reader) {
        X = reader.ReadInt32();
        Y = reader.ReadInt32();
        Z = reader.ReadInt32();
    }

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        res.Add(new JProperty("Z", Z));
        return res;
    }

}
