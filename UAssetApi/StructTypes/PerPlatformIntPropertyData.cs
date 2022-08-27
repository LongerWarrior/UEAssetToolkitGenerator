namespace UAssetAPI.StructTypes;

/// <summary>
/// <see cref="IntPropertyData"/> (<see cref="int"/>) property with per-platform overrides.
/// </summary>
public class PerPlatformIntPropertyData : PropertyData<int>
{
    public bool bCooked;
    public PerPlatformIntPropertyData(FName name) : base(name)
    {

    }

    public PerPlatformIntPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("PerPlatformInt");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        bCooked = reader.ReadIntBoolean();
        Value = reader.ReadInt32();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        writer.Write(bCooked ? 1 : 0);
        writer.Write(Value);
        return sizeof(int)*2;
    }
    public override JToken ToJson() {
        JObject res = new JObject();
        res.Add("Default", Value);
        return res;
    }
}