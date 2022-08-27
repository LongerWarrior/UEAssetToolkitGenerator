namespace UAssetAPI;

public class FontExport : NormalExport
{
    public Dictionary<ushort, ushort> CharRemap;

    public FontExport(Export super) : base(super)
    {

    }


    public FontExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        int num = reader.ReadInt32();

        CharRemap = new Dictionary<ushort, ushort>(num);
        for (int i = 0; i < num; ++i) {
            CharRemap[reader.ReadUInt16()] = reader.ReadUInt16();
        }
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(CharRemap.Count);
        foreach (KeyValuePair<ushort,ushort> entry in CharRemap)
        {
            writer.Write(entry.Key);
            writer.Write(entry.Value);
        }
    }
}
