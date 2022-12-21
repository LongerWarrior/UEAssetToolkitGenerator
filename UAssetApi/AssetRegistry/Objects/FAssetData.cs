namespace UAssetAPI.AssetRegistry;

public class FAssetData
{
    public readonly FName ObjectPath;
    public readonly FName PackageName;
    public readonly FName PackagePath;
    public readonly FName AssetName;
    public readonly FName AssetClass;
    public IDictionary<FName, string> TagsAndValues;
    public FAssetBundleData TaggedAssetBundles;
    public readonly int[] ChunkIDs;
    public readonly uint PackageFlags;

    public FAssetData(FAssetRegistryArchive reader)
    {
        ObjectPath = reader.ReadFName();
        PackagePath = reader.ReadFName();
        //AssetClass = reader.Version >= FAssetRegistryVersionType.ClassPaths ? new FTopLevelAssetPath(reader).AssetName : reader.ReadFName();
        AssetClass = reader.ReadFName();
        PackageName = reader.ReadFName();
        AssetName = reader.ReadFName();

        reader.SerializeTagsAndBundles(this);

        ChunkIDs = reader.ReadArray(()=> reader.ReadInt32());
        PackageFlags = reader.ReadUInt32();
        //reader.BaseStream.Position += 8;
    }
}

//public class FAssetDataOld  {
//    public readonly FString ObjectPath;
//    public readonly FString PackageName;
//    public readonly FString PackagePath;
//    public readonly FString AssetName;
//    public readonly FString AssetClass;
//    public readonly FString GroupNames;
//    public IDictionary<FName, string> TagsAndValues;
//    public FAssetBundleData TaggedAssetBundles;
//    public readonly int[] ChunkIDs;
//    public readonly uint PackageFlags;

//    public FAssetDataOld(FAssetRegistryArchive reader) {
//        ObjectPath = reader.ReadFString();
//        PackagePath = reader.ReadFString();
//        AssetClass = reader.ReadFString();
//        GroupNames = reader.ReadFString();
//        PackageName = reader.ReadFString();
//        AssetName = reader.ReadFString();

//        reader.SerializeTagsAndBundles(this);

//        ChunkIDs = reader.ReadArray(() => reader.ReadInt32());
//        if (reader.Ver >= UE4Version.VER_UE4_COOKED_ASSETS_IN_EDITOR_SUPPORT)
//            PackageFlags = reader.ReadUInt32();
        
//    }
//}