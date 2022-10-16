using UAssetApi.ExportTypes;

namespace UAssetAPI;

public class UAnimCurveCompressionSettings : NormalExport
{
    public FPackageIndex Codec; // UAnimCurveCompressionCodec

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
        Codec = GetOrDefault<FPackageIndex>(nameof(Codec));
    }

    public UAnimCurveCompressionCodec? GetCodec(string path) => Codec?.Load<UAnimCurveCompressionCodec>()?.GetCodec(path);
}
