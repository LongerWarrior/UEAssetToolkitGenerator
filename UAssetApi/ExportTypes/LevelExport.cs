namespace UAssetAPI;

public class LevelExport : NormalExport {
    public List<FPackageIndex> Actors;
    public FURL URL;
    public FPackageIndex Model;
    public List<FPackageIndex> ModelComponents;

    public LevelExport(Export super) : base(super) {

    }

    public LevelExport(UAsset asset, byte[] extras) : base(asset, extras) {

    }

    public LevelExport() {

    }
    public override void Read(AssetBinaryReader reader, int nextStarting) {
        base.Read(reader, nextStarting);

        reader.ReadInt32();
        int numIndexEntries = reader.ReadInt32();

        Actors = new List<FPackageIndex>();
        for (int i = 0; i < numIndexEntries; i++) {
            Actors.Add(reader.XFERPTR());
        }
        URL = new FURL(reader);

        Model = reader.XFERPTR();

        numIndexEntries = reader.ReadInt32();
        ModelComponents = new List<FPackageIndex>();
        for (int i = 0; i < numIndexEntries; i++) {
            ModelComponents.Add(reader.XFERPTR());
        }

        //reader.ReadByte();
    }

    public override void Write(AssetBinaryWriter writer) {
        base.Write(writer);

        writer.Write((int)0);

    }
}
