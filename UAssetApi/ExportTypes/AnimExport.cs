namespace UAssetAPI;

public class AnimExport : NormalExport
{
    public FPackageIndex Skeleton; // USkeleton
    public Guid SkeletonGuid;

    public AnimExport(Export super) : base(super) { }
    
    public AnimExport() { }
    
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
    
    public override void Write(AssetBinaryWriter writer) {
        base.Write(writer);
        writer.Write(0);
        
        if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_SKELETON_GUID_SERIALIZATION) 
        {
            writer.Write(SkeletonGuid.ToByteArray());
        }
    }
}