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
            SetupSerialization(out string name, out string gamepath, out string path1);
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
                if (texture.Data.Count > 0) {
                    foreach (PropertyData property in texture.Data) {
                        asdata.Add(SerializePropertyData(property));
                    }
                }

                asdata.Add("TextureWidth", texture.Mips[0].SizeX);
                asdata.Add("TextureHeight", texture.Mips[0].SizeY);
                asdata.Add("TextureDepth", texture.Mips[0].SizeZ);
                if (texture.ClassIndex.ToImport(asset).ObjectName.ToName() == "TextureCube") {
                    asdata.Add("NumSlices", 6);
                } else if (texture.ClassIndex.ToImport(asset).ObjectName.ToName() == "Texture2DArray") {
                    asdata.Add("NumSlices", texture.Mips[0].SizeZ);
                } else {
                    asdata.Add("NumSlices", 1);
                }

                asdata.Add("CookedPixelFormat", texture.PlatformData.PixelFormat.ToString());

                Thread.Sleep(50);

                var bitmap = TextureDecoder.Decode(texture, texture.Mips[0]);
                using var fs = new FileStream(path2, FileMode.Create, FileAccess.Write);
                using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
                using var stream = data.AsStream();
                stream.CopyTo(fs);
                fs.Close();

                Thread.Sleep(50);
                using (var md5 = MD5.Create()) {
                    using (var stream1 = File.OpenRead(path2)) {
                        string hash = md5.ComputeHash(stream1).Select(x => x.ToString("X2")).Aggregate((a, b) => a + b);
                        asdata.Add("SourceImageHash", hash);
                    }
                }

                /*FString UTexture2DGenerator::ComputeTextureHash(UTexture2D * Texture) {
                    TArray64<uint8> OutSourceMipMapData;
                    check(Texture->Source.GetMipData(OutSourceMipMapData, 0));

                    FString TextureHash = FMD5::HashBytes(OutSourceMipMapData.GetData(), OutSourceMipMapData.Num() * sizeof(uint8));
                    TextureHash.Append(FString::Printf(TEXT("%llx"), OutSourceMipMapData.Num()));
                    return TextureHash;
                }*/


                ja.Add(ObjectHierarchy(asset));
                File.WriteAllText(path1, ja.ToString());

            }
        }


    }


}