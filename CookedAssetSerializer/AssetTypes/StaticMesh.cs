using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using Textures;
using SkiaSharp;
using System.Security.Cryptography;
using System.Threading;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeStaticMesh() {
            if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
            DisableGeneration.Add("bAllowCPUAccess");
            DisableGeneration.Add("MinLOD");
            DisableGeneration.Add("ExtendedBounds");
            DisableGeneration.Add("LightmapUVDensity");
            DisableGeneration.Add("StaticMaterials");
            string path2 = Path.ChangeExtension(path1, "fbx");
            if (!File.Exists(path2)) {
                Console.WriteLine("Error. File doen't exist : " + path2);
                return;
            }
            JObject ja = new JObject();
            StaticMeshExport mesh = Exports[Asset.mainExport - 1] as StaticMeshExport;

            if (mesh != null) {

                var type = Exports[Asset.mainExport - 1].ClassIndex.ToImport(Asset).ObjectName.ToName();
                ja.Add("AssetClass", type);
                ja.Add("AssetPackage", gamepath);
                ja.Add("AssetName", name);
                JObject asdata = new JObject();

                ja.Add("AssetSerializedData", asdata);

                JObject aodata = SerializaListOfProperties(mesh.Data);
                aodata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
                RefObjects = new List<int>();
                asdata.Add("AssetObjectData", aodata);


                JArray jmaterials = new JArray();
                if (FindPropertyData(mesh, "StaticMaterials", out PropertyData _materials)) {
                    ArrayPropertyData materials = (ArrayPropertyData)_materials;
                    foreach (StructPropertyData _material in materials.Value) {
                        JObject jmaterial = new JObject();
                        if (FindPropertyData(_material.Value, "MaterialSlotName", out PropertyData _name)) {
                            NamePropertyData namedslot = (NamePropertyData)_name;
                            jmaterial.Add("MaterialSlotName", namedslot.Value.ToName());
                        }
                        if (FindPropertyData(_material.Value, "MaterialInterface", out PropertyData _interface)) {
                            ObjectPropertyData minterface = (ObjectPropertyData)_interface;
                            jmaterial.Add("MaterialInterface", Index(minterface.Value.Index));
                        }
                        jmaterials.Add(jmaterial);
                    }
                }



                asdata.Add("Materials", jmaterials);
                asdata.Add("NavCollision", Index(mesh.NavCollision.Index));
                asdata.Add("BodySetup", Index(mesh.BodySetup.Index));
                asdata.Add("MinimumLodNumber", 0);
                asdata.Add("LodNumber", 1);
                asdata.Add("ScreenSize", JArray.FromObject(new List<int> { 1,0,0,0,0,0,0,0}));

                using (var md5 = MD5.Create()) {
                    if (File.Exists(path2)) {
                        using (var stream1 = File.OpenRead(path2)) {
                            string hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
                            asdata.Add("ModelFileHash", hash);
                        }
                    } else {
                        Console.WriteLine(path2);
                        return;
                    }
                }
               

                ja.Add(ObjectHierarchy(Asset,false));
                File.WriteAllText(path1, ja.ToString());

            }
        }


    }


}