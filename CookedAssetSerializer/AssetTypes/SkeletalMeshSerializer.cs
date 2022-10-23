using CookedAssetSerializer.FBX;
using static CookedAssetSerializer.FBX.SkeletalMeshFBX;

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
        
        SerializeHeaders();

        AssetData.Add(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
        
        // Export raw mesh data into seperate FBX file that can be imported back into UE
        var path2 = Path.ChangeExtension(OutPath, "fbx");
        string error = "";
        bool tooLarge = false;
        new SkeletalMeshFBX(BuildSkeletonStruct(), path2, false, ref error, ref tooLarge);

        if (!File.Exists(path2)) 
        {
            IsSkipped = true;
            if (error != "")
            {
                if (tooLarge) SkippedCode = error;
            }
            else
            {
                SkippedCode = "No FBX file supplied!";
            }
            return;
        }
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
    
    FSkeletalMeshStruct BuildSkeletonStruct()
    {
        FSkeletalMeshStruct skm;
        skm.Name = AssetName;
        skm.RefSkeleton = ClassExport.ReferenceSkeleton;
        skm.Materials = ClassExport.Materials;
        skm.LODModels = ClassExport.LODModels;
        return skm;
    } 
}

/*public class SkeletalMeshSerializer : SimpleAssetSerializer<SkeletalMeshExport>
{
    public SkeletalMeshSerializer(Settings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset(new JProperty("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset)));
    }
}*/