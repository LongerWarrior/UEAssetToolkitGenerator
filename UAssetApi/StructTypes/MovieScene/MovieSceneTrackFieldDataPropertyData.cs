namespace UAssetAPI.StructTypes;

public class MovieSceneTrackFieldDataPropertyData : PropertyData<FMovieSceneTrackFieldData> {

    public MovieSceneTrackFieldDataPropertyData(FName name) : base(name) {

    }

    public MovieSceneTrackFieldDataPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("MovieSceneTrackFieldData");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        Value = new FMovieSceneTrackFieldData().Read(reader);


    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }
        int here = (int)writer.BaseStream.Position;
        Value.Write(writer);

        return (int)writer.BaseStream.Position - here;
    }

    public override JToken ToJson() {
        return Value.ToJson();
    }
}