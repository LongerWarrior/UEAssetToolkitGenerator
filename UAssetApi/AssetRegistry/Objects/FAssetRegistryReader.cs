namespace UAssetAPI.AssetRegistry;

public class FAssetRegistryReader : FAssetRegistryArchive
{
    private const uint AssetRegistryNumberedNameBit = 0x80000000u; // int32 max
    public readonly FStore Tags;

    public FAssetRegistryReader(AssetBinaryReader reader, FAssetRegistryVersionType version, UE4Version engineVersion = UE4Version.UNKNOWN) : base(reader, version, engineVersion)
    {
        NameMap = FNameEntrySerialized.LoadNameBatch(reader);
        Tags = new FStore(this);
    }

    public override FName ReadFName() {
        var index = ReadUInt32();
        var number = 0u;
        if ((index & AssetRegistryNumberedNameBit) > 0u) {
            index -= AssetRegistryNumberedNameBit;
            number = ReadUInt32();
        }
        if (index >= NameMap.Length) {
            throw new Exception($"FName could not be read, requested index {index}, name map size {NameMap.Length}");
        }
        return new FName(NameMap[index].Name, (int)number);
    }

    public override void SerializeTagsAndBundles(FAssetData assetData)
    {
        var size = ReadUInt64();
        var ret = new Dictionary<FName, string>();
        var mapHandle = FPartialMapHandle.MakeFullHandle(Tags, size);
        foreach (var m in mapHandle.GetEnumerable())
        {
            ret[m.Key] = FValueHandle.GetString(Tags, m.Value);
        }

        assetData.TagsAndValues = ret;
        assetData.TaggedAssetBundles = new FAssetBundleData(this);
    }
}