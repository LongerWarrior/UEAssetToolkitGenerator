namespace UAssetAPI;

public class CurveBaseExport : NormalExport
{

    public CurveBaseExport(Export super) : base(super)
    {

    }


    public CurveBaseExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);
    }
}
