namespace UAssetAPI.AssetRegistry;

public class FAssetPackageData
{
    public readonly FName PackageName;
    public readonly Guid PackageGuid;
    public readonly FMD5Hash? CookedHash;
    //public readonly FName[]? ImportedClasses;
    public readonly long DiskSize;
    //public readonly FPackageFileVersion FileVersionUE;
    //public readonly int FileVersionLicenseeUE = -1;
    //public readonly FCustomVersion[]? CustomVersions;
    //public readonly uint Flags;

    public FAssetPackageData(FAssetRegistryArchive ARreader)
    {
        PackageName = ARreader.ReadFName();
        DiskSize = ARreader.ReadInt64();
        PackageGuid = new Guid(ARreader.ReadBytes(16));
        if (ARreader.Version >= FAssetRegistryVersionType.AddedCookedMD5Hash)
        {
            CookedHash = new FMD5Hash(ARreader);
        }
        //if (ARreader.Version >= FAssetRegistryVersionType.AddedChunkHashes)
        //{
        //    // TMap<FIoChunkId, FIoHash> ChunkHashes;
        //    ARreader.BaseStream.Position += ARreader.ReadInt32() * (12 + 20);
        //}
        //if (ARreader.Version >= FAssetRegistryVersionType.WorkspaceDomain)
        //{
        //    if (ARreader.Version >= FAssetRegistryVersionType.PackageFileSummaryVersionChange)
        //    {
        //       // FileVersionUE = ARreader.Read<FPackageFileVersion>();
        //    }
        //    else
        //    {
        //        var ue4Version = ARreader.ReadInt32();
        //       // FileVersionUE = FPackageFileVersion.CreateUE4Version(ue4Version);
        //    }

        //    FileVersionLicenseeUE = ARreader.ReadInt32();
        //    Flags = ARreader.ReadUInt32();
        //    //CustomVersions = ARreader.ReadArray<FCustomVersion>();
        //}
        //if (ARreader.Version >= FAssetRegistryVersionType.PackageImportedClasses)
        //{
        //    ImportedClasses = ARreader.ReadArray(ARreader.ReadFName);
        //}
    }
}
