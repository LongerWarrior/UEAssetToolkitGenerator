namespace UAssetAPI;

public class BlendSpaceBaseExport : NormalExport
{
    public Guid SkeletonGuid;

    public BlendSpaceBaseExport(Export super) : base(super)
    {

    }


    public BlendSpaceBaseExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        if (reader.Ver >= UE4Version.VER_UE4_SKELETON_GUID_SERIALIZATION) {
            SkeletonGuid = new Guid(reader.ReadBytes(16));
        }
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);

        if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_SKELETON_GUID_SERIALIZATION) {
            writer.Write(SkeletonGuid.ToByteArray());
        }
    }
}
