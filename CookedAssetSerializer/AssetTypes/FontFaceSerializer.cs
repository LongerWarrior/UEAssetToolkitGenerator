using System.Security.Cryptography;

namespace CookedAssetSerializer.AssetTypes;

public class FontFaceSerializer : Serializer<FontFaceExport>
{
    public FontFaceSerializer(JSONSettings settings, UAsset asset)
    {
        Settings = settings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;
	        
        if (!SetupAssetInfo()) return;
	        
        SerializeHeaders();
        
        AssignAssetSerializedData();
        
        AssetData.Add("AssetObjectData", SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects));
        
        var ttf = Path.ChangeExtension(OutPath,"ttf");
        long length = 0;
        if (ClassExport.bSaveInlineData) 
        {
            File.WriteAllBytes(ttf, ClassExport.FontFaceData.Data);
            length = ClassExport.FontFaceData.Data.Length;
        } 
        else 
        {
            var targetFile = Path.ChangeExtension(Asset.FilePath, "ufont");
            if (File.Exists(targetFile)) 
            {
                File.Copy(targetFile, ttf,true);
                length = new FileInfo(targetFile).Length;
            }
        }

        using (var md5 = MD5.Create()) 
        {
            using (var stream1 = File.OpenRead(ttf)) 
            {
                string hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
                hash += length.ToString("x2");
                AssetData.Add("FontPayloadHash", hash);
            }
        }
        
        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}