﻿namespace UAssetAPI;

/*public class UAnimBoneCompressionCodec_ACL : UAnimBoneCompressionCodec_ACLBase
{
    public FPackageIndex SafetyFallbackCodec; // UAnimBoneCompressionCodec

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        SafetyFallbackCodec = GetOrDefault<FPackageIndex>(nameof(SafetyFallbackCodec));
    }

    public override UAnimBoneCompressionCodec? GetCodec(string ddcHandle)
    {
        var thisHandle = GetCodecDDCHandle();
        UAnimBoneCompressionCodec? codecMatch = thisHandle == ddcHandle ? this : null;

        if (codecMatch == null && SafetyFallbackCodec.TryLoad<UAnimBoneCompressionCodec>(out var safetyFallbackCodec))
        {
            codecMatch = safetyFallbackCodec.GetCodec(ddcHandle);
        }

        return codecMatch;
    }
}*/