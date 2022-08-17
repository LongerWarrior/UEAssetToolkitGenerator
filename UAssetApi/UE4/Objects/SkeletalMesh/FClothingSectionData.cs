namespace UAssetAPI.StructTypes.SkeletalMesh;

public struct FClothingSectionData
{
    public readonly Guid AssetGuid;
    public readonly int AssetLodIndex;

    public FClothingSectionData(AssetBinaryReader reader) {
        AssetGuid = new Guid(reader.ReadBytes(16));
        AssetLodIndex = reader.ReadInt32();
    }
}