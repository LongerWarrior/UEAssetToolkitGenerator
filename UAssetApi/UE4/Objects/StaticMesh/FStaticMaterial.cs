namespace UAssetAPI.StructTypes.StaticMesh;

public class FStaticMaterial
{
    //public ResolvedObject? MaterialInterface; // UMaterialInterface
    public FPackageIndex MaterialInterface; // UMaterialInterface
    public FName MaterialSlotName;
    //public FName ImportedMaterialSlotName;
    public FMeshUVChannelInfo? UVChannelData;

    public FStaticMaterial(AssetBinaryReader reader)
    {
        MaterialInterface = reader.XFERPTR();// new FPackageIndex(Ar).ResolvedObject;
        MaterialSlotName = reader.ReadFName();
        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() >= FRenderingObjectVersion.TextureStreamingMeshUVChannelData)
            UVChannelData = new FMeshUVChannelInfo(reader);
    }
}
