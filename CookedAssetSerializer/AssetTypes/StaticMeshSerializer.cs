using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using CookedAssetSerializer.FBX;
using static CookedAssetSerializer.FBX.StaticMeshFBX;

namespace CookedAssetSerializer.AssetTypes;

public class StaticMeshSerializer : Serializer<StaticMeshExport>
{
    public StaticMeshSerializer(Settings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.String; size: 128MB")]
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
        
        //AssignAssetSerializedData();
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        RefObjects = new List<int>();
        AssetData.Add("AssetObjectData", properties);
        
        var materialsData = new JArray();
        //if (FindPropertyData(ClassExport, "StaticMaterials", out PropertyData _materials))
        //{
        //    var materials = (ArrayPropertyData)_materials;
        //    foreach (var propertyData in materials.Value)
        //    {
        //        var material = (StructPropertyData)propertyData;
        //        var materialData = new JObject();
        //        if (FindPropertyData(material.Value, "MaterialSlotName", out PropertyData _name))
        //        {
        //            var slotName = (NamePropertyData)_name;
        //            materialData.Add("MaterialSlotName", slotName.Value.ToName());
        //        }
        //        if (FindPropertyData(material.Value, "MaterialInterface", out PropertyData _interface))
        //        {
        //            var materialInterface = (ObjectPropertyData)_interface;
        //            materialData.Add("MaterialInterface", Index(materialInterface.Value.Index, Dict));
        //        }
        //        materialsData.Add(materialData);
        //    }
        //}
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

        // Export raw mesh data into seperate FBX file that can be imported back into UE
        var path2 = Path.ChangeExtension(OutPath, "fbx");
        string error = "";
        new StaticMeshFBX(BuildStaticMeshStruct(), path2, false, ref error);

        if (!File.Exists(path2) || error != "") 
        {
            IsSkipped = true;
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
        FStaticMeshStruct mesh;
        mesh.Name = AssetName;
        mesh.RenderData = ClassExport.RenderData;
        mesh.StaticMaterials = ClassExport.StaticMaterials;
        return mesh;
    } 
}