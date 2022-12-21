namespace UAssetAPI.AssetRegistry;

public abstract class FAssetRegistryArchive : AssetBinaryReader
{
    //protected readonly FArchive baseArchive;
    public FAssetRegistryVersionType Version;
    public FNameEntrySerialized[] NameMap;

    public abstract void SerializeTagsAndBundles(FAssetData assetData);
    //public virtual void SerializeTagsAndBundles(FAssetDataOld assetData) { }

    public FAssetRegistryArchive(AssetBinaryReader reader, FAssetRegistryVersionType version, UE4Version engineVersion = UE4Version.UNKNOWN) : base(reader.BaseStream, engineVersion)
    {
        //baseArchive = Ar;
        Version = version;
    }


}