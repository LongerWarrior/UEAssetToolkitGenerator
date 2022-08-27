namespace UAssetAPI.StructTypes;

public class MovieSceneSequenceIDPropertyData : PropertyData<FMovieSceneSequenceID> {


    public MovieSceneSequenceIDPropertyData(FName name) : base(name) {

    }

    public MovieSceneSequenceIDPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("MovieSceneSequenceID");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        Value = new FMovieSceneSequenceID(reader.ReadUInt32());

    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }

        writer.Write(Value.Value);

        return sizeof(uint);
    }

    public override JToken ToJson() => Value.ToJson();
}
