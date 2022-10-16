namespace UAssetAPI;

public class AnimSequenceBaseExport : NormalExport
{
    public float SequenceLength;
    public float RateScale = 1.0f;

    public AnimSequenceBaseExport(Export super) : base(super) { }
    
    public AnimSequenceBaseExport() { }
    
    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        SequenceLength = reader.ReadSingle();
        RateScale = reader.ReadSingle();
    }
}