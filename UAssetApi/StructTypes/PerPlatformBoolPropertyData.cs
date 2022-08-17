namespace UAssetAPI.StructTypes;

/// <summary>
/// <see cref="BoolPropertyData"/> (<see cref="bool"/>) property with per-platform overrides.
/// </summary>
public class PerPlatformBoolPropertyData : PropertyData<bool>
{
    public bool bCooked; 
    public PerPlatformBoolPropertyData(FName name) : base(name)
    {

    }

    public PerPlatformBoolPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("PerPlatformBool");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        bCooked = reader.ReadInt32()!=0;
        Value = reader.ReadInt32()!=0;
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        writer.Write(bCooked?1:0);
        writer.Write(Value?1:0);

        return 2*sizeof(int);
    }


    public override JToken ToJson() {
        JObject res = new JObject();
        res.Add("Default", Value);
        return res;
    }
}