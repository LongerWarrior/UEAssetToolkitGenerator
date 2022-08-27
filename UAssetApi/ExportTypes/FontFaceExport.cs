namespace UAssetAPI;

public class FFontFaceData {
    public byte[] Data;

    public FFontFaceData(AssetBinaryReader reader) {

        int length = reader.ReadInt32();
        Data = reader.ReadBytes(length);
    }
}

public class FontFaceExport : NormalExport
{
    public bool bSaveInlineData;
    public FFontFaceData FontFaceData;
    public FontFaceExport(Export super) : base(super)
    {

    }

    public FontFaceExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        int num = reader.ReadInt32();
        bSaveInlineData = reader.ReadIntBoolean();
        if (bSaveInlineData) {
            FontFaceData = new FFontFaceData(reader);
        }

    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);
                    
        writer.Write(0);
        writer.Write(bSaveInlineData ? 1 : 0);
        if (bSaveInlineData && FontFaceData!=null) {
            writer.Write(FontFaceData.Data.Length);
            writer.Write(FontFaceData.Data);
        }

    }
}
