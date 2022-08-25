namespace UAssetAPI.AssetRegistry;

public class FAssetBundleData
{
    public readonly FAssetBundleEntry[] Bundles;

    public FAssetBundleData()
    {
        Bundles = Array.Empty<FAssetBundleEntry>();
    }
    
    public FAssetBundleData(FAssetRegistryArchive ARreader)
    {
        Bundles = ARreader.ReadArray(() => new FAssetBundleEntry(ARreader));
    }
}