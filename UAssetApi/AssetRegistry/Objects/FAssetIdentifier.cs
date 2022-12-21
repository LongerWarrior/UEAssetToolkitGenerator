namespace UAssetAPI.AssetRegistry;

public class FAssetIdentifier
{
    public readonly FName PackageName;
    public readonly FName PrimaryAssetType;
    public readonly FName ObjectName;
    public readonly FName ValueName;

    public FAssetIdentifier(AssetBinaryReader reader)
    {
        var fieldBits = reader.ReadByte();
        if ((fieldBits & (1 << 0)) != 0)
        {
            PackageName = reader.ReadFName();
        }
        if ((fieldBits & (1 << 1)) != 0)
        {
            PrimaryAssetType = reader.ReadFName();
        }
        if ((fieldBits & (1 << 2)) != 0)
        {
            ObjectName = reader.ReadFName();
        }
        if ((fieldBits & (1 << 3)) != 0)
        {
            ValueName = reader.ReadFName();
        }
    }
}