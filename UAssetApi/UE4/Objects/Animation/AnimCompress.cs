using UAssetAPI;

namespace UAssetApi.UE4.Objects.Animation;

public class UAnimCompress : UAnimBoneCompressionCodec
{
    public override ICompressedAnimData AllocateAnimData() => new FUECompressedAnimData();
}