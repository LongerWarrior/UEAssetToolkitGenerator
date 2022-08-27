using System.Text;

namespace UAssetAPI.StructTypes;

public class FUniqueNetId {
    public FName Type;
    public FString Contents;

    public FUniqueNetId(FName type, FString contents) {
        Type = type;
        Contents = contents;
    }

    public JObject ToJson() {
        JObject value = new JObject();
        value.Add("Type", Type.ToName());
        value.Add("Contents", Contents.ToString());
        return value;
    }
}


public class UniqueNetIdReplPropertyData : PropertyData<FUniqueNetId>
{


    public UniqueNetIdReplPropertyData(FName name) : base(name)
    {

    }

    public UniqueNetIdReplPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("UniqueNetIdRepl");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }
        int size = reader.ReadInt32();
        if (size > 0) {
            Value = new FUniqueNetId(reader.ReadFName(), reader.ReadFString());
        } else {
            Value = null;
        }
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int here = (int)writer.BaseStream.Position;

        if (Value != null) {

            int length = 3 * sizeof(int);
            if (Value.Contents != null) {
                length+= Value.Contents.Encoding is UnicodeEncoding ? (Value.Contents.Value.Length+1)*2 : (Value.Contents.Value.Length+1);
            }
            writer.Write(Value.Type);
            writer.Write(Value.Contents);

        } else {
            writer.Write(0);
        }           
        return (int)writer.BaseStream.Position - here;
    }
    public override JToken ToJson() => Value.ToJson();
}
