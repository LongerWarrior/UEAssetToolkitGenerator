using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{

    public struct FInt32RangeBound {
        public ERangeBoundTypes Type; // 0x00(0x01)
        public int Value; // 0x04(0x04)

        public JObject ToJson() {
            JObject value = new JObject();
            value.Add("Type", Type.ToString());
            value.Add("Value", Value);
            return value;
        }
    }

    public class MovieSceneFrameRangePropertyData : PropertyData
    {
        [JsonProperty]
        public FInt32RangeBound LowerBound;
        [JsonProperty]
        public FInt32RangeBound UpperBound;

        public MovieSceneFrameRangePropertyData(FName name) : base(name)
        {

        }

        public MovieSceneFrameRangePropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("MovieSceneFrameRange");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                PropertyGuid = reader.ReadPropertyGuid();
            }

            LowerBound.Type = (ERangeBoundTypes)reader.ReadSByte();
            LowerBound.Value = reader.ReadInt32();

            UpperBound.Type = (ERangeBoundTypes)reader.ReadSByte();
            UpperBound.Value = reader.ReadInt32();
        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader)
        {
            if (includeHeader)
            {
                writer.WritePropertyGuid(PropertyGuid);
            }

            writer.Write((sbyte)LowerBound.Type);
            writer.Write(LowerBound.Value);
            writer.Write((sbyte)UpperBound.Type);
            writer.Write(UpperBound.Value);

            return sizeof(float) * 2 + sizeof(sbyte) * 2;
        }


        public override JToken ToJson() {

            JObject res = new JObject();
            res.Add("LowerBound", LowerBound.ToJson());
            res.Add("UpperBound", UpperBound.ToJson());
            return res;

        }

    }
}
