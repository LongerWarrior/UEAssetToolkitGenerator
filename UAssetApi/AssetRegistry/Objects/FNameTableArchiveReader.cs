namespace UAssetAPI.AssetRegistry;

public class FNameTableArchiveReader : FAssetRegistryArchive
{
    public FNameTableArchiveReader(AssetBinaryReader reader, FAssetRegistryVersionType version, UE4Version engineVersion = UE4Version.UNKNOWN) : base(reader, version, engineVersion)
    {
        var nameOffset = reader.ReadInt64();
        if (nameOffset > reader.BaseStream.Length)
            throw new ArgumentOutOfRangeException(nameof(nameOffset), "AssetRegistry is corrupted");

        if (nameOffset > 0)
        {
            var originalOffset = reader.BaseStream.Position;
            reader.BaseStream.Position = nameOffset;

            var nameCount = reader.ReadInt32();
            if (nameCount < 0)
                throw new ArgumentOutOfRangeException(nameof(nameCount), "AssetRegistry is corrupted");

            var maxReservation = (reader.BaseStream.Length - reader.BaseStream.Position) / sizeof(int);
            NameMap = new FNameEntrySerialized[Math.Min(nameCount, maxReservation)];
            reader.ReadArray(NameMap, () => new FNameEntrySerialized(reader));

            reader.BaseStream.Position = originalOffset;
        }
        else
        {
            NameMap = Array.Empty<FNameEntrySerialized>();
        }
    }

    public override FName ReadFName() {
        var nameIndex = ReadInt32();
        var number = ReadInt32();
        if (nameIndex < 0 || nameIndex >= NameMap.Length) {
            throw new Exception($"FName could not be read, requested index {nameIndex}, name map size {NameMap.Length}");
        }
        return new FName(NameMap[nameIndex].Name, number);
    }

    public override void SerializeTagsAndBundles(FAssetData assetData)
    {
        var size = this.ReadInt32();
        var ret = new Dictionary<FName, string>();
        for (var i = 0; i < size; i++)
        {
            ret[ReadFName()] = ReadFString().ToString();
        }
        assetData.TagsAndValues = ret;
        assetData.TaggedAssetBundles = new FAssetBundleData();
    }
    
}

//public class FOldArReader : FAssetRegistryArchive {
//    public FOldArReader(AssetBinaryReader reader, FAssetRegistryVersionType version, UE4Version engineVersion = UE4Version.UNKNOWN) : base(reader, version, engineVersion) {

//    }

//    public override FName ReadFName() {
//        var nameIndex = ReadInt32();
//        var number = ReadInt32();
//        if (nameIndex < 0 || nameIndex >= NameMap.Length) {
//            throw new Exception($"FName could not be read, requested index {nameIndex}, name map size {NameMap.Length}");
//        }
//        return new FName(NameMap[nameIndex].Name, number);
//    }

//    public override void SerializeTagsAndBundles(FAssetData assetData) {
//        var size = this.ReadInt32();
//        var ret = new Dictionary<FName, string>();
//        for (var i = 0; i < size; i++) {
//            ret[ReadFName()] = ReadFString().ToString();
//        }
//        assetData.TagsAndValues = ret;
//        assetData.TaggedAssetBundles = new FAssetBundleData();
//    }

//    public override void SerializeTagsAndBundles(FAssetDataOld assetData) {
//        var size = this.ReadInt32();
//        var ret = new Dictionary<FName, string>();
//        for (var i = 0; i < size; i++) {
//            ret[ReadFName()] = ReadFString().ToString();
//        }
//        assetData.TagsAndValues = ret;
//        assetData.TaggedAssetBundles = new FAssetBundleData();
//    }

//}