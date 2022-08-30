using System.Text;

namespace UAssetAPI.AssetRegistry;

public class FAssetRegistryExportPath
{
    public readonly FName Class;
    public readonly FName Object;
    public readonly FName Package;

    public FAssetRegistryExportPath(FAssetRegistryArchive Ar)
    {
        //Class = Ar.Version >= FAssetRegistryVersionType.ClassPaths ? new FTopLevelAssetPath(Ar).AssetName : Ar.ReadFName();
        Class = Ar.ReadFName();
        Object = Ar.ReadFName();
        Package = Ar.ReadFName();
    }

    public FAssetRegistryExportPath(FNameEntrySerialized classs, FNameEntrySerialized objectt, FNameEntrySerialized package)
    {
        Class = new FName(classs.Name);
        Object = new FName(objectt.Name);
        Package = new FName(package.Name);
    }

    public override string ToString() {
        var sb = new StringBuilder();
        if (!Class.IsNone)
            sb.Append(Class.ToName() + "'");
        sb.Append(Package.ToName());
        if (!Object.IsNone)
            sb.Append('.' + Object.ToName());
        if (!Class.IsNone)
            sb.Append("'");
        return sb.ToString();
    }
}