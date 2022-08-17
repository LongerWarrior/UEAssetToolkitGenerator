using System.Diagnostics;
using System.Security.Cryptography;


namespace CookedAssetSerializer.AssetTypes;

public class SkeletalMeshSerializer : Serializer<SkeletalMeshExport>
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
        
        //SerializeHeaders();
        
        //AssignAssetSerializedData();
        

    }
}