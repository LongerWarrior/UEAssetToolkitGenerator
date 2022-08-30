namespace UAssetAPI.AssetRegistry;

public class FAssetBundleEntry
{
    public readonly FName BundleName;
    public readonly FSoftObjectPath[] BundleAssets;

    public FAssetBundleEntry(FAssetRegistryArchive ARreader)
    {
        BundleName = ARreader.ReadFName();
        BundleAssets = ARreader.ReadArray(() => new FSoftObjectPath(ARreader.ReadFName(), ARreader.ReadFString()));
    }
}