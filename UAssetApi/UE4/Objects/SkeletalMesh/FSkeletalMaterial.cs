namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkeletalMaterial
{
    public FPackageIndex? Material; // UMaterialInterface
    public FName MaterialSlotName;
    public FName? ImportedMaterialSlotName;
    public FMeshUVChannelInfo? UVChannelData;

    public FSkeletalMaterial(AssetBinaryReader reader)
    {
        Material = reader.XFERPTR();
        if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.RefactorMeshEditorMaterials)
        {
            MaterialSlotName = reader.ReadFName();
            var bSerializeImportedMaterialSlotName = !reader.Asset.PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly);
            if (reader.Asset.GetCustomVersion<FCoreObjectVersion>() >= FCoreObjectVersion.SkeletalMaterialEditorDataStripping)
            {
                bSerializeImportedMaterialSlotName = reader.ReadIntBoolean();
            }

            if (bSerializeImportedMaterialSlotName)
            {
                ImportedMaterialSlotName = reader.ReadFName();
            }
        }
        else
        {
            if (reader.Ver >=  UE4Version.VER_UE4_MOVE_SKELETALMESH_SHADOWCASTING)
                reader.BaseStream.Position += 4;

            if (reader.Asset.GetCustomVersion<FRecomputeTangentCustomVersion>() >= FRecomputeTangentCustomVersion.RuntimeRecomputeTangent)
            {
                var bRecomputeTangent = reader.ReadIntBoolean();
            }
        }
        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() >= FRenderingObjectVersion.TextureStreamingMeshUVChannelData)
            UVChannelData = new FMeshUVChannelInfo(reader);
    }
}
