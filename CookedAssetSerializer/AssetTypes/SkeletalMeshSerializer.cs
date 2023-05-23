using System.Security.Cryptography;
using CookedAssetSerializer.FBX;
using static CookedAssetSerializer.FBX.SkeletalMeshFBX;

namespace CookedAssetSerializer.AssetTypes;

public class SkeletalMeshSerializer : Serializer<SkeletalMeshExport>
{
    public SkeletalMeshSerializer(JSONSettings assetSettings, UAsset asset)
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

        if (Settings.ForceOneLOD)
        {
            foreach (var prop in properties.Properties())
            {
                if (prop.Name == "LODInfo")
                {
                    var lodInfo = (JArray)prop.Value;
                    if (lodInfo != null)
                    {
                        var firstLOD = lodInfo[0];
                        lodInfo.Clear();
                        lodInfo.Add(firstLOD);
                    }
                }
            }
        }
        
        var path2 = "";
        string error = "";
        bool tooLarge = false;
        if (Settings.UseSKMActorX)
        {
            path2 = Path.ChangeExtension(OutPath, "pskx");
        }
        else
        {
            path2 = Path.ChangeExtension(OutPath, "fbx");
            //new SkeletalMeshFBX(BuildSkeletonStruct(), path2, false, ref error, ref tooLarge);
        }
        
        if (!File.Exists(path2)) 
        {
            IsSkipped = true;
            if (error != "")
            {
                if (tooLarge) SkippedCode = error;
            }
            else
            {
                SkippedCode = "No model file supplied!";
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