namespace UAssetAPI.StructTypes;

public class MovieSceneSegmentPropertyData : PropertyData<FMovieSceneSegment>
{
    public MovieSceneSegmentPropertyData(FName name) : base(name)
    {

    }

    public MovieSceneSegmentPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("MovieSceneSegment");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }
        Value = new FMovieSceneSegment();

        Value.Range = new FFrameNumberRange(reader);
        Value.ID = new FMovieSceneSegmentIdentifier(reader.ReadInt32());
        Value.bAllowEmpty = reader.ReadIntBoolean();
        int length = reader.ReadInt32();


        List<PropertyData>[] items = new List<PropertyData>[length];
        for (int i = 0; i < length; i++) {
            List<PropertyData> resultingList = new List<PropertyData>();
            PropertyData data = null;
            while ((data = MainSerializer.Read(reader, true)) != null) {
                resultingList.Add(data);
            }
            items[i] = resultingList;
        }
        Value.Impls = items;


        //Value.Impls = new StructPropertyData[length];
        //for (int i = 0; i< length;i++) {
        //    Value.Impls[i] = new StructPropertyData(new FName("Impls"), new FName("SectionEvaluationData"));
        //    Value.Impls[i].Read(reader, false, 1);
        //}
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int here = (int)writer.BaseStream.Position;
        Value.Range.Write(writer);
        writer.Write(Value.ID.IdentifierIndex);
        writer.Write(Value.bAllowEmpty ? 1 : 0);
        for (int i = 0; i < Value.Impls.Length; i++) {


            if (Value.Impls[i] != null) {
                foreach (var t in Value.Impls[i]) {
                    MainSerializer.Write(t, writer, true);
                }
            }
            writer.Write(new FName("None"));
            //Value.Impls[i].Write(writer, false);
        }

        return (int)writer.BaseStream.Position - here;
    }

    public override JToken ToJson() {
        return Value.ToJson();
    }

}