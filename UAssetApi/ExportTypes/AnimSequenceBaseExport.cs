using UAssetAPI;

namespace UAssetApi.ExportTypes;

public class AnimSequenceBaseExport : AnimExport
{
    public float SequenceLength;
    public float RateScale;

    public AnimSequenceBaseExport(Export super) : base(super) { }
    
    public AnimSequenceBaseExport() { }
    
    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        SequenceLength = this["SequenceLength"] is FloatPropertyData slength ? slength.Value : default;
        RateScale = this["RateScale"] is FloatPropertyData srate ? srate.Value : 1.0f;
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);
    }
}