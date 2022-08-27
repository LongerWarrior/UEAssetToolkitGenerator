namespace UAssetAPI.StructTypes;

/// <summary>
/// <see cref="FloatPropertyData"/> (<see cref="float"/>) property with per-platform overrides.
/// </summary>
public class PerPlatformFloatPropertyData : PropertyData<float>
{
    public bool bCooked;
    public PerPlatformFloatPropertyData(FName name) : base(name)
    {

    }

    public PerPlatformFloatPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("PerPlatformFloat");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        bCooked = reader.ReadIntBoolean();
        Value = reader.ReadSingle();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        writer.Write(bCooked ? 1 : 0);
        writer.Write(Value);

        return sizeof(int) + sizeof(float);
    }

    public override JToken ToJson() {
        JObject res = new JObject();
        res.Add("Default", Value);
        return res;
    }
}