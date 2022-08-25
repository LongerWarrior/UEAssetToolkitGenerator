namespace UAssetAPI.AssetRegistry;

public class FMD5Hash
{
    public readonly byte[]? Hash;
    
    public FMD5Hash(AssetBinaryReader reader)
    {
        Hash = reader.ReadUInt32() != 0 ? reader.ReadBytes(16) : null;
    }
}