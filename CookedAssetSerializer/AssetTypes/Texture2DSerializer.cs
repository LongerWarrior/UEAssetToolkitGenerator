using SkiaSharp;
using Textures;


namespace CookedAssetSerializer.AssetTypes;

public class Texture2DSerializer : Serializer<Texture2DExport>
{
    public Texture2DSerializer(JSONSettings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;

        DisableGeneration.Add("LightingGuid");
        DisableGeneration.Add("ImportedSize");
        var path2 = Path.ChangeExtension(OutPath, "png");

        if (!SetupAssetInfo()) return;

        SerializeHeaders();
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        RefObjects = new List<int>();
        AssetData.Add("AssetObjectData", properties);
        
        var isCube = false;
        var numSlices = 1;
        if (ClassExport.Mips.Length > 0) 
        {
            AssetData.Add("TextureWidth", ClassExport.Mips[0].SizeX);
            AssetData.Add("TextureHeight", ClassExport.Mips[0].SizeY);
            AssetData.Add("TextureDepth", ClassExport.Mips[0].SizeZ);
            
            if (ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName() == "TextureCube") 
            {
                numSlices = 6;
                isCube = true;
                ClassExport.Mips[0].SizeY *= numSlices;
                ClassExport.Mips[0].SizeZ = 1;
            }
            if (ClassExport.ClassIndex.ToImport(Asset).ObjectName.ToName() == "Texture2DArray") 
            {
                numSlices = ClassExport.Mips[0].SizeZ;
                ClassExport.Mips[0].SizeY *= numSlices;
                ClassExport.Mips[0].SizeZ = 1;

            }
            AssetData.Add("NumSlices", numSlices);
            AssetData.Add("CookedPixelFormat", ClassExport.PlatformData.PixelFormat.ToString());

            var srgb = true;
            if (FindPropertyData(ClassExport, "SRGB", out PropertyData prop)) 
            {
                srgb = ((BoolPropertyData)prop).Value;
            }
            if (FindPropertyData(ClassExport, "CompressionSettings", out PropertyData _compression)) 
            {
                var compression = (BytePropertyData)_compression;
                if (compression.ByteType == BytePropertyType.Long) 
                {
                    if (Asset.GetNameReference(compression.Value).ToString().Contains("TC_Normalmap")) 
                    {
                        ClassExport.isNormalMap = true;
                    }
                }
            }
            
            var bitmap = ClassExport.Decode(ClassExport.Mips[0], numSlices, out string hash, srgb, isCube);
            var hashEnd = bitmap.Bytes.Length.ToString("x2");
            
            using var fs = new FileStream(path2, FileMode.Create, FileAccess.Write);
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = data.AsStream();
            stream.CopyTo(fs);
            fs.Close();
            
            hash += hashEnd;
            AssetData.Add("SourceImageHash", hash);
        } 
        else
        {
            AssetData.Add("TextureWidth", 1);
            AssetData.Add("TextureHeight", 1);
            AssetData.Add("TextureDepth", 1);
            AssetData.Add("NumSlices", numSlices);
            
            using var fs = new FileStream(path2, FileMode.Create, FileAccess.Write);

            var bitmap = new SKBitmap(1, 1);
            bitmap.Encode(SKEncodedImageFormat.Png, 100).AsStream().CopyTo(fs); 
            fs.Close();

            AssetData.Add("CookedPixelFormat", ClassExport.PlatformData.PixelFormat.ToString());
            AssetData.Add("SourceImageHash", 0.ToString("x2"));
        }
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}