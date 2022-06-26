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

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeTexture() {
            if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
            DisableGeneration.Add("LightingGuid");
            DisableGeneration.Add("ImportedSize");
            string path2 = Path.ChangeExtension(path1, "png");
            JObject ja = new JObject();
            Texture2DExport texture = exports[asset.mainExport - 1] as Texture2DExport;

            if (texture != null) {

                var type = exports[asset.mainExport - 1].ClassIndex.ToImport(asset).ObjectName.ToName();
                ja.Add("AssetClass", type);
                ja.Add("AssetPackage", gamepath);
                ja.Add("AssetName", name);
                JObject asdata = new JObject();

                ja.Add("AssetSerializedData", asdata);

                JObject aodata = SerializaListOfProperties(texture.Data);
                aodata.Add("$ReferencedObjects", JArray.FromObject(refobjects.Distinct<int>()));
                refobjects = new List<int>();
                asdata.Add("AssetObjectData", aodata);

                asdata.Add("TextureWidth", texture.Mips[0].SizeX);
                asdata.Add("TextureHeight", texture.Mips[0].SizeY);
                asdata.Add("TextureDepth", texture.Mips[0].SizeZ);
                bool iscube = false;
                int NumSlices = 1;
                if (texture.ClassIndex.ToImport(asset).ObjectName.ToName() == "TextureCube") {
                    NumSlices = 6;
                    iscube = true;
                } else if (texture.ClassIndex.ToImport(asset).ObjectName.ToName() == "Texture2DArray") {
                    NumSlices = texture.Mips[0].SizeZ;
                } 

                asdata.Add("NumSlices", NumSlices);
                asdata.Add("CookedPixelFormat", texture.PlatformData.PixelFormat.ToString());

                Thread.Sleep(50);

                bool srgb = true;
                if (FindPropertyData(texture,"SRGB",out PropertyData prop)) {
                    srgb = ((BoolPropertyData)prop).Value;
                }
                if (FindPropertyData(texture, "CompressionSettings", out PropertyData _compression)) {
                    BytePropertyData compression = (BytePropertyData)_compression;
                    if (compression.ByteType == BytePropertyType.Long) {
                        if (asset.GetNameReference(compression.Value).ToString().Contains("TC_Normalmap")) {
                            texture.isNormalMap = true;
                        }
                    }
                }
                var bitmap = TextureDecoder.Decode(texture, texture.Mips[0],NumSlices,out string hash,srgb,iscube);
                var hashend = bitmap.Bytes.Length.ToString("x2");
                using var fs = new FileStream(path2, FileMode.Create, FileAccess.Write);
                using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
                using var stream = data.AsStream();
                stream.CopyTo(fs);
                fs.Close();
                Thread.Sleep(50);

                hash += hashend;
                asdata.Add("SourceImageHash", hash);

                ja.Add(ObjectHierarchy(asset));
                File.WriteAllText(path1, ja.ToString());

            }
        }


    }


}