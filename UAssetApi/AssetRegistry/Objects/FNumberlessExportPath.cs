namespace UAssetAPI.AssetRegistry;

public class FNumberlessExportPath
{
    public readonly uint ClassPackage;
    public readonly uint ClassObject;
    public readonly uint Object;
    public readonly uint Package;
    public readonly FNameEntrySerialized[] Names;

    public FNumberlessExportPath(FAssetRegistryArchive ARreader)
    {
        //if (ARreader.Version >= FAssetRegistryVersionType.ClassPaths)
        //{
        //    ClassPackage = ARreader.ReadUInt32();
        //    ClassObject = ARreader.ReadUInt32();
        //}
        //else
        //{
        //    ClassObject = ARreader.ReadUInt32();
        //}
        ClassObject = ARreader.ReadUInt32();
        Object = ARreader.ReadUInt32();
        Package = ARreader.ReadUInt32();
        Names = ARreader.NameMap;
    }

    public override string ToString()
    {
        return new FAssetRegistryExportPath(Names[ClassObject], Names[Object], Names[Package]).ToString();
    }
}