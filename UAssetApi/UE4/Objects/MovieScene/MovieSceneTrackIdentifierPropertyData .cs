using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{
    public class MovieSceneTrackIdentifierPropertyData : PropertyData<FMovieSceneTrackIdentifier> {
       

        public MovieSceneTrackIdentifierPropertyData(FName name) : base(name)
        {

        }

        public MovieSceneTrackIdentifierPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("MovieSceneTrackIdentifier");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                PropertyGuid = reader.ReadPropertyGuid();
            }

            Value = new FMovieSceneTrackIdentifier(reader.ReadUInt32());

        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.WritePropertyGuid(PropertyGuid);
            }

            writer.Write(Value.Value);

            return sizeof(uint);
        }


        public override JToken ToJson() => Value.ToJson();
    }

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

    public class MovieSceneEvaluationKeyPropertyData : PropertyData<FMovieSceneEvaluationKey> {


        public MovieSceneEvaluationKeyPropertyData(FName name) : base(name) {

        }

        public MovieSceneEvaluationKeyPropertyData() {

        }

        private static readonly FName CurrentPropertyType = new FName("MovieSceneEvaluationKey");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
            if (includeHeader) {
                PropertyGuid = reader.ReadPropertyGuid();
            }

            Value = new FMovieSceneEvaluationKey(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());

        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader) {
            if (includeHeader) {
                writer.WritePropertyGuid(PropertyGuid);
            }

            writer.Write(Value.SequenceID.Value);
            writer.Write(Value.TrackIdentifier.Value);
            writer.Write(Value.SectionIndex);

            return 3*sizeof(uint);
        }
        public override JToken ToJson() => Value.ToJson();
    }

}