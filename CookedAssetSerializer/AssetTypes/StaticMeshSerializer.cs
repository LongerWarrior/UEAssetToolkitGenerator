using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using CookedAssetSerializer.FBX;
using static CookedAssetSerializer.FBX.StaticMeshFBX;

namespace CookedAssetSerializer.AssetTypes;

public class StaticMeshSerializer : Serializer<StaticMeshExport>
{
    public StaticMeshSerializer(JSONSettings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }
    
    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;
        
        DisableGeneration.Add("bAllowCPUAccess");
        DisableGeneration.Add("MinLOD");
        DisableGeneration.Add("ExtendedBounds");
        DisableGeneration.Add("LightmapUVDensity");
        DisableGeneration.Add("StaticMaterials");

        if (!SetupAssetInfo()) return;
        
        SerializeHeaders();
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        RefObjects = new List<int>();
        AssetData.Add("AssetObjectData", properties);
        
        var materialsData = new JArray();
        List<int> materialindexes = new();
        materialindexes.AddRange((ClassExport.RenderData?.LODs[0].Sections).Select(section => section.MaterialIndex));

        var mats = ClassExport.StaticMaterials;

        foreach (var index in materialindexes)
        {
            if (index < ClassExport.StaticMaterials.Length)
            {
                var materialData = new JObject();
                materialData.Add("MaterialSlotName", mats[index].MaterialSlotName.ToName());
                materialData.Add("MaterialInterface", Index(mats[index].MaterialInterface.Index, Dict));
                materialsData.Add(materialData);
            }
        }

        AssetData.Add("Materials", materialsData);
        AssetData.Add("NavCollision", Index(ClassExport.NavCollision.Index, Dict));
        AssetData.Add("BodySetup", Index(ClassExport.BodySetup.Index, Dict));
        AssetData.Add("MinimumLodNumber", 0);
        AssetData.Add("LodNumber", 1);
        AssetData.Add("ScreenSize", JArray.FromObject(new List<int> {1, 0, 0, 0, 0, 0, 0, 0}));

        var path2 = "";
        string error = "";
        bool tooLarge = false;
        if (Settings.UseSMActorX)
        {
            path2 = Path.ChangeExtension(OutPath, "pskx");
        }
        else
        {
            path2 = Path.ChangeExtension(OutPath, "fbx");
            //new StaticMeshFBX(BuildStaticMeshStruct(), path2, false, ref error, ref tooLarge);
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

    FStaticMeshStruct BuildStaticMeshStruct()
    {
        foreach (var lod in ClassExport.RenderData.LODs)
        {
            if (lod.VertexBuffer != null)
            {
                foreach (var uvItem in lod.VertexBuffer.UV)
                {
                    uvItem.Normal[2].Data &= 0xFFFFFFu;
                }
            }
        }
        
        FStaticMeshStruct mesh;
        mesh.Name = AssetName;
        mesh.RenderData = ClassExport.RenderData;
        mesh.StaticMaterials = ClassExport.StaticMaterials;
        return mesh;
    } 
}