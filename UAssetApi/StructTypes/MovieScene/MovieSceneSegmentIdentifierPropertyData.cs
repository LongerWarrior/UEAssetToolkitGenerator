namespace UAssetAPI.StructTypes;

public class MovieSceneSegmentIdentifierPropertyData : PropertyData<FMovieSceneSegmentIdentifier> {
    public MovieSceneSegmentIdentifierPropertyData(FName name) : base(name) {

    }

    public MovieSceneSegmentIdentifierPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("MovieSceneSegmentIdentifier");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }
        Value = new FMovieSceneSegmentIdentifier(reader.ReadInt32());

    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int here = (int)writer.BaseStream.Position;
        writer.Write(Value.IdentifierIndex);

        return (int)writer.BaseStream.Position - here;
    }

    public override JToken ToJson() {
        return Value.ToJson();
    }

}
