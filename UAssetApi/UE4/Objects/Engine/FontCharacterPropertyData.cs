using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using UAssetAPI.PropertyTypes;

namespace UAssetAPI.StructTypes
{

    public struct FFontCharacter {
        public int StartU;
        public int StartV;
        public int USize;
        public int VSize;
        public byte TextureIndex;
        public int VerticalOffset;

        public FFontCharacter(AssetBinaryReader reader) {
            StartU = reader.ReadInt32();
            StartV = reader.ReadInt32();
            USize = reader.ReadInt32();
            VSize = reader.ReadInt32();
            TextureIndex = reader.ReadByte();
            VerticalOffset = reader.ReadInt32();
        }

        public void Write(AssetBinaryWriter writer) {
            writer.Write(StartU);
            writer.Write(StartV);
            writer.Write(USize);
            writer.Write(VSize);
            writer.Write(TextureIndex);
            writer.Write(VerticalOffset);
        }

        public JObject ToJson() {
            JObject value = new JObject();
            value.Add("StartU", StartU);
            value.Add("StartV", StartV);
            value.Add("USize", USize);
            value.Add("VSize", VSize);
            value.Add("TextureIndex", TextureIndex);
            value.Add("VerticalOffset", VerticalOffset);
            return value;
        }
    }


    public class FontCharacterPropertyData : PropertyData<FFontCharacter>
    {


        public FontCharacterPropertyData(FName name) : base(name)
        {

        }

        public FontCharacterPropertyData()
        {

        }

        private static readonly FName CurrentPropertyType = new FName("FontCharacter");
        public override bool HasCustomStructSerialization { get { return true; } }
        public override FName PropertyType { get { return CurrentPropertyType; } }

        public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
        {
            if (includeHeader)
            {
                PropertyGuid = reader.ReadPropertyGuid();
            }
            Value = new FFontCharacter(reader);

        }

        public override int Write(AssetBinaryWriter writer, bool includeHeader) {
            if (includeHeader) {
                writer.WritePropertyGuid(PropertyGuid);
            }

            int here = (int)writer.BaseStream.Position;

            Value.Write(writer);
 
            return (int)writer.BaseStream.Position - here;
        }
        public override JToken ToJson() => Value.ToJson();
    }
}
