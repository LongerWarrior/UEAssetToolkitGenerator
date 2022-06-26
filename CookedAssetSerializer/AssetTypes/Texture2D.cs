﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void SerializeTexture() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            DisableGeneration.Add("LightingGuid");
            DisableGeneration.Add("ImportedSize");
            var path2 = Path.ChangeExtension(path1, "png");
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not Texture2DExport texture) return;
            var type = Exports[Asset.mainExport - 1].ClassIndex.ToImport(Asset).ObjectName.ToName();
            ja.Add("AssetClass", type);
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);

            var aoData = SerializaListOfProperties(texture.Data);
            aoData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));
            RefObjects = new List<int>();
            asData.Add("AssetObjectData", aoData);
            asData.Add("TextureWidth", texture.Mips[0].SizeX);
            asData.Add("TextureHeight", texture.Mips[0].SizeY);
            asData.Add("TextureDepth", texture.Mips[0].SizeZ);
            var isCube = false;
            var numSlices = 1;
            if (texture.ClassIndex.ToImport(Asset).ObjectName.ToName() == "TextureCube") {
                numSlices = 6;
                isCube = true;
                texture.Mips[0].SizeY *= numSlices;
                texture.Mips[0].SizeZ = 1;
            }

            if (texture.ClassIndex.ToImport(Asset).ObjectName.ToName() == "Texture2DArray") {
                numSlices = texture.Mips[0].SizeZ;
                texture.Mips[0].SizeY *= numSlices;
                texture.Mips[0].SizeZ = 1;
            }

            asData.Add("NumSlices", numSlices);
            asData.Add("CookedPixelFormat", texture.PlatformData.PixelFormat.ToString());

            Thread.Sleep(50);

            var srgb = true;
            if (FindPropertyData(texture, "SRGB", out var prop)) srgb = ((BoolPropertyData)prop).Value;
            if (FindPropertyData(texture, "CompressionSettings", out var _compression)) {
                var compression = (BytePropertyData)_compression;
                if (compression.ByteType == BytePropertyType.Long)
                    if (Asset.GetNameReference(compression.Value).ToString().Contains("TC_Normalmap"))
                        texture.isNormalMap = true;
            }

            var bitmap = TextureDecoder.Decode(texture, texture.Mips[0], numSlices, out var hash, srgb, isCube);
            var hashEnd = bitmap.Bytes.Length.ToString("x2");
            using var fs = new FileStream(path2, FileMode.Create, FileAccess.Write);
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = data.AsStream();
            stream.CopyTo(fs);
            fs.Close();
            Thread.Sleep(50);

            hash += hashEnd;
            asData.Add("SourceImageHash", hash);

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
