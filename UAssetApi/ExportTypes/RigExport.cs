namespace UAssetAPI;


public class RigExport : NormalExport
{
    public FReferenceSkeleton ReferenceSkeleton;

    public RigExport(Export super) : base(super)
    {

    }


    public RigExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);


        int idk = reader.ReadInt32();

        ReferenceSkeleton = new FReferenceSkeleton();
        ReferenceSkeleton.Read(reader);
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);

    }
}
