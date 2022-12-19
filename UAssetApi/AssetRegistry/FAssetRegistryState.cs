using Serilog;
namespace UAssetAPI.AssetRegistry;
public class FAssetRegistryState {

    protected static readonly ILogger ARlog = Log.ForContext("Context", "AR");
    //public FAssetDataOld[] PreallocatedAssetDataBuffersOld;
    public FAssetData[] PreallocatedAssetDataBuffers;
    public FDependsNode[] PreallocatedDependsNodeDataBuffers;
    public FAssetPackageData[] PreallocatedPackageDataBuffers;

    public FAssetRegistryState(AssetBinaryReader reader, UE4Version engineVersion = UE4Version.UNKNOWN) {
        FAssetRegistryVersion.TrySerializeVersion(reader, out var version);
        switch (version) {
            case < FAssetRegistryVersionType.RemovedMD5Hash:
                //var ARreaderold = new FOldArReader(reader, version, engineVersion);
                //PreallocatedAssetDataBuffersOld = ARreaderold.ReadArray(() => new FAssetDataOld(ARreaderold));
                throw new Exception($"Cannot read registry state before '{version}'");
                break;
            case < FAssetRegistryVersionType.FixedTags: {
                    var nameTableReader = new FNameTableArchiveReader(reader, version, engineVersion);        
                    Load(nameTableReader);
                    nameTableReader.Dispose();
                    break;
                }
            default: {
                    var ARreader = new FAssetRegistryReader(reader, version, engineVersion);
                    Load(ARreader);
                    ARreader.Dispose();
                    break;
                }
        }
        ARlog.Information($"[Scan AR]: Succefully read Asset Registry");
        reader.Dispose();
    }

    public FAssetRegistryState(string path, UE4Version engineVersion = UE4Version.UNKNOWN) : this(new AssetBinaryReader(PathToStream(path), engineVersion), engineVersion) {

    }
    public static MemoryStream PathToStream(string path) {
        using (FileStream origStream = File.Open(path, FileMode.Open)) {
            MemoryStream completeStream = new MemoryStream();
            origStream.CopyTo(completeStream);
            completeStream.Seek(0, SeekOrigin.Begin);
            return completeStream;
        }

    }

    public static FileStream PathToStreamTest(string path) {
        return File.Open(path, FileMode.Open);
    }


    private void Load(FAssetRegistryArchive ARreader) {
        PreallocatedAssetDataBuffers = ARreader.ReadArray(() => new FAssetData(ARreader));

        if (ARreader.Version < FAssetRegistryVersionType.AddedDependencyFlags) {
            var localNumDependsNodes = ARreader.ReadInt32();
            PreallocatedDependsNodeDataBuffers = new FDependsNode[localNumDependsNodes];
            for (var i = 0; i < localNumDependsNodes; i++) {
                PreallocatedDependsNodeDataBuffers[i] = new FDependsNode(i);
            }
            if (localNumDependsNodes > 0) {
                LoadDependencies_BeforeFlags(ARreader);
            }
        } else {
            var dependencySectionSize = ARreader.ReadInt64();
            var dependencySectionEnd = ARreader.BaseStream.Position + dependencySectionSize;
            var localNumDependsNodes = ARreader.ReadInt32();
            PreallocatedDependsNodeDataBuffers = new FDependsNode[localNumDependsNodes];
            for (var i = 0; i < localNumDependsNodes; i++) {
                PreallocatedDependsNodeDataBuffers[i] = new FDependsNode(i);
            }
            if (localNumDependsNodes > 0) {
                LoadDependencies(ARreader);
            }
            ARreader.BaseStream.Position = dependencySectionEnd;
        }

        PreallocatedPackageDataBuffers = ARreader.ReadArray(() => new FAssetPackageData(ARreader));
    }

    private void LoadDependencies_BeforeFlags(FAssetRegistryArchive Ar) {
        foreach (var dependsNode in PreallocatedDependsNodeDataBuffers) {
            dependsNode.SerializeLoad_BeforeFlags(Ar, PreallocatedDependsNodeDataBuffers);
        }
    }

    private void LoadDependencies(FAssetRegistryArchive Ar) {
        foreach (var dependsNode in PreallocatedDependsNodeDataBuffers) {
            dependsNode.SerializeLoad(Ar, PreallocatedDependsNodeDataBuffers);
        }
    }
}
