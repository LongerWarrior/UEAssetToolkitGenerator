using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Cryptography;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class StaticMeshSerializer : Serializer<StaticMeshExport>
    {
        public StaticMeshSerializer(Settings assetSettings)
        {
            Settings = assetSettings;
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
            
            var path2 = Path.ChangeExtension(OutPath, "fbx");
            if (!File.Exists(path2)) 
            {
                Console.WriteLine("Error. File doesn't exist: " + path2);
                return;
            }

            if (!SetupAssetInfo()) return;
            
            SerializeHeaders();
            
            //AssignAssetSerializedData();
            
            var properties = SerializaListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
            properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
            RefObjects = new List<int>();
            AssetData.Add("AssetObjectData", properties);
            
            var materialsData = new JArray();
            if (FindPropertyData(ClassExport, "StaticMaterials", out PropertyData _materials)) 
            {
                var materials = (ArrayPropertyData)_materials;
                foreach (var propertyData in materials.Value) 
                {
                    var material = (StructPropertyData)propertyData;
                    var materialData = new JObject();
                    if (FindPropertyData(material.Value, "MaterialSlotName", out PropertyData _name)) 
                    {
                        var slotName = (NamePropertyData)_name;
                        materialData.Add("MaterialSlotName", slotName.Value.ToName());
                    }
                    if (FindPropertyData(material.Value, "MaterialInterface", out PropertyData _interface)) 
                    {
                        var materialInterface = (ObjectPropertyData)_interface;
                        materialData.Add("MaterialInterface", Index(materialInterface.Value.Index, Dict));
                    }
                    materialsData.Add(materialData);
                }
            }
            
            AssetData.Add("Materials", materialsData);
            AssetData.Add("NavCollision", Index(ClassExport.NavCollision.Index, Dict));
            AssetData.Add("BodySetup", Index(ClassExport.BodySetup.Index, Dict));
            AssetData.Add("MinimumLodNumber", 0);
            AssetData.Add("LodNumber", 1);
            AssetData.Add("ScreenSize", JArray.FromObject(new List<int> {1, 0, 0, 0, 0, 0, 0, 0}));

            using (var md5 = MD5.Create()) 
            {
                // TODO: Can probably remove this check as it is already being done above
                if (File.Exists(path2))
                {
                    using var stream1 = File.OpenRead(path2);
                    var hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
                    AssetData.Add("ModelFileHash", hash);
                } 
                else 
                {
                    Console.WriteLine(path2);
                    return;
                }
            }
            
            AssignAssetSerializedData();
            
            WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
        }
    }
}