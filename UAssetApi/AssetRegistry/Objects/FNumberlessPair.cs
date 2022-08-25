namespace UAssetAPI.AssetRegistry;

public class FNumberlessPair
{
    public readonly uint Key;
    public readonly FValueId Value;

    public FNumberlessPair(FAssetRegistryReader ARreader)
    {
        Key = ARreader.ReadUInt32();
        Value = new FValueId(ARreader);
    }
}