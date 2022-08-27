namespace UAssetAPI;

public class FileMediaSourceExport : NormalExport
{
    public FName PlayerName;

    public FileMediaSourceExport(Export super) : base(super)
    {

    }


    public FileMediaSourceExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        PlayerName = reader.ReadFName();
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);

        writer.Write(PlayerName);
    }
}
