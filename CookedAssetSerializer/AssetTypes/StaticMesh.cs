using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using System.Security.Cryptography;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeStaticMesh() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            DisableGeneration.Add("bAllowCPUAccess");
            DisableGeneration.Add("MinLOD");
            DisableGeneration.Add("ExtendedBounds");
            DisableGeneration.Add("LightmapUVDensity");
            DisableGeneration.Add("StaticMaterials");
            var path2 = Path.ChangeExtension(path1, "fbx");
            if (!File.Exists(path2)) {
                Console.WriteLine("Error. File doesn't exist: " + path2);
                return;
            }

            var ja = new JObject();
            if (Exports[Asset.mainExport - 1] is not StaticMeshExport mesh) return;
            var type = Exports[Asset.mainExport - 1].ClassIndex.ToImport(Asset).ObjectName.ToName();
            ja.Add("AssetClass", type);
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);

            var aoData = SerializaListOfProperties(mesh.Data);
            aoData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            RefObjects = new List<int>();
            asData.Add("AssetObjectData", aoData);

            var jMaterials = new JArray();
            if (FindPropertyData(mesh, "StaticMaterials", out var _materials)) {
                var materials = (ArrayPropertyData)_materials;
                foreach (StructPropertyData _material in materials.Value) {
                    var jMaterial = new JObject();
                    if (FindPropertyData(_material.Value, "MaterialSlotName", out PropertyData _name)) {
                        var namedSlot = (NamePropertyData)_name;
                        jMaterial.Add("MaterialSlotName", namedSlot.Value.ToName());
                    }

                    if (FindPropertyData(_material.Value, "MaterialInterface", out PropertyData _interface)) {
                        var minInterface = (ObjectPropertyData)_interface;
                        jMaterial.Add("MaterialInterface", Index(minInterface.Value.Index));
                    }

                    jMaterials.Add(jMaterial);
                }
            }

            asData.Add("Materials", jMaterials);
            asData.Add("NavCollision", Index(mesh.NavCollision.Index));
            asData.Add("BodySetup", Index(mesh.BodySetup.Index));
            asData.Add("MinimumLodNumber", 0);
            asData.Add("LodNumber", 1);
            asData.Add("ScreenSize", JArray.FromObject(new List<int> { 1, 0, 0, 0, 0, 0, 0, 0 }));

            using (var md5 = MD5.Create()) {
                if (File.Exists(path2)) {
                    using var stream1 = File.OpenRead(path2);
                    var hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2"))
                        .Aggregate((a, b) => a + b);
                    asData.Add("ModelFileHash", hash);
                } else {
                    Console.WriteLine(path2);
                    return;
                }
            }

            ja.Add(ObjectHierarchy(Asset, false));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
