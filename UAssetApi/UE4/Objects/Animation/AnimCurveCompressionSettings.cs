namespace UAssetAPI;

/*public class UAnimCurveCompressionSettings : UObjectProperty
{
    public FPackageIndex Codec; // UAnimCurveCompressionCodec

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        Codec = GetOrDefault<FPackageIndex>(nameof(Codec));
    }
    
    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
        
        // Skeleton = GetOrDefault<FPackageIndex>(nameof(Skeleton));
        Skeleton = reader.XFER_OBJECT_POINTER();
        
        if (reader.Ver >= UE4Version.VER_UE4_SKELETON_GUID_SERIALIZATION)
        {
            SkeletonGuid = new Guid(reader.ReadBytes(16));
        }
    }

    public UAnimCurveCompressionCodec? GetCodec(string path) => Codec?.Load<UAnimCurveCompressionCodec>()?.GetCodec(path);
}*/
