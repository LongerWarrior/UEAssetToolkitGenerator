namespace UAssetAPI.StructTypes;

public class FrameNumberPropertyData : PropertyData<FFrameNumber>
{


    public FrameNumberPropertyData(FName name) : base(name)
    {

    }

    public FrameNumberPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("FrameNumber");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }
        Value = new FFrameNumber(reader.ReadInt32());

    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        writer.Write(Value.Value);
        return sizeof(int);
    }
    public override JToken ToJson() => Value.ToJson();
}
