namespace UAssetAPI;

public class WorldExport : NormalExport
{
    public FPackageIndex PersistentLevel;
    public FPackageIndex[] ExtraReferencedObjects;
    public FPackageIndex[] StreamingLevels;

    public WorldExport(Export super) : base(super)
    {

    }


    public WorldExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        int num = reader.ReadInt32();

        PersistentLevel = reader.XFER_OBJECT_POINTER();
        int length = reader.ReadInt32();
        ExtraReferencedObjects = new FPackageIndex[length];
        for (int i = 0; i < length; i++) {
            ExtraReferencedObjects[i] = reader.XFER_OBJECT_POINTER();
        }
        length = reader.ReadInt32();
        StreamingLevels = new FPackageIndex[length];
        for (int i = 0; i < length; i++) {
            StreamingLevels[i] = reader.XFER_OBJECT_POINTER();
        }
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);
        writer.XFER_OBJECT_POINTER(PersistentLevel);
        writer.Write(ExtraReferencedObjects.Length);
        foreach (FPackageIndex index in ExtraReferencedObjects) {
            writer.XFER_OBJECT_POINTER(index);
        }
        writer.Write(StreamingLevels.Length);
        foreach (FPackageIndex index in StreamingLevels) {
            writer.XFER_OBJECT_POINTER(index);
        }
    }
}
