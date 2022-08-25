namespace UAssetAPI;

public readonly struct FTopLevelAssetPath : IUStruct
{
    public readonly FName PackageName;
    public readonly FName AssetName;

    public FTopLevelAssetPath(AssetBinaryReader reader)
    {
        PackageName = reader.ReadFName();
        AssetName = reader.ReadFName();
    }

    public FTopLevelAssetPath(FName packageName, FName assetName)
    {
        PackageName = packageName;
        AssetName = assetName;
    }

    public override string ToString()
    {
        return $"{PackageName}.{AssetName}";
    }
}