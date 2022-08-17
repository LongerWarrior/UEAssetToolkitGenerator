namespace UAssetAPI.StructTypes;

public class MovieSceneSequenceInstanceDataPtrPropertyData : PropertyData<FPackageIndex> {

    public MovieSceneSequenceInstanceDataPtrPropertyData(FName name) : base(name) {

    }

    public MovieSceneSequenceInstanceDataPtrPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("MovieSceneSequenceInstanceDataPtr");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }
        Value = reader.XFER_OBJECT_POINTER();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }
        int here = (int)writer.BaseStream.Position;
        writer.XFER_OBJECT_POINTER(Value);
        return (int)writer.BaseStream.Position - here;
    }

    public override JToken ToJson() {
        return Value.ToJson();
    }
}
