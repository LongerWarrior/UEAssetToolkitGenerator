namespace CookedAssetSerializer.AssetTypes;

/*public class SkeletalMeshSerializer : Serializer<SkeletalMeshExport>
{
    public SkeletalMeshSerializer(Settings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;
        
        if (!SetupAssetInfo()) return;

        //if (ClassExport.Extras.Length > 0) {
        //    Debug.WriteLine(Asset.FilePath);
        //    Debug.WriteLine(ClassExport.Extras.Length);
        //}
        
        SerializeHeaders();
        
        AssignAssetSerializedData();
    }
}*/

public class SkeletalMeshSerializer : SimpleAssetSerializer<SkeletalMeshExport>
{
    public SkeletalMeshSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }
}