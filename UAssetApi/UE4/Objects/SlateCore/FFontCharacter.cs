namespace UAssetAPI.StructTypes;

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
