namespace UAssetAPI.AssetRegistry;

public class FNumberedPair
{
    public readonly FName Key;
    public readonly FValueId Value;

    public FNumberedPair(FAssetRegistryReader ARreader)
    {
        Key = ARreader.ReadFName();
        Value = new FValueId(ARreader);
    }

    public FNumberedPair(FName key, FValueId value)
    {
        Key = key;
        Value = value;
    }
}