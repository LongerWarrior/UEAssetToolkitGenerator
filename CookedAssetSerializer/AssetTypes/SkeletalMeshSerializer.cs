using System.Security.Cryptography;
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
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        AssetData.Add("AssetObjectData", properties);

        var materials = new JArray();
        List<short> materialindexes = new();
        materialindexes.AddRange((ClassExport.LODModels[0].Sections).Select(section => section.MaterialIndex));
        var mats = ClassExport.Materials;
        foreach (var index in materialindexes)
        {
            if (index < mats.Length)
            {
                var materialData = new JObject();
                materialData.Add("MaterialSlotName", mats[index].MaterialSlotName.ToName());
                var fPackageIndex = mats[index].Material;
                if (fPackageIndex != null) materialData.Add("MaterialInterface", Index(fPackageIndex.Index, Dict));
                materials.Add(materialData);
            }
        }
        AssetData.Add("Materials", materials);
        
        // Export raw mesh data into seperate FBX file that can be imported back into UE
        var path2 = Path.ChangeExtension(OutPath, "fbx");
        string error = "";
        bool tooLarge = false;
        //new SkeletalMeshFBX(BuildSkeletonStruct(), path2, false, ref error, ref tooLarge);

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
        using (var md5 = MD5.Create()) 
        {
            using var stream1 = File.OpenRead(path2);
            var hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            AssetData.Add("ModelFileHash", hash);
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