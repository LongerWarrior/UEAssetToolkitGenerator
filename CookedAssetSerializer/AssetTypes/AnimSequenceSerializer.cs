using System.Security.Cryptography;

namespace CookedAssetSerializer.AssetTypes;

public class AnimSequenceSerializer : Serializer<NormalExport>
{
    public AnimSequenceSerializer(Settings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }
    
    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;

        if (!SetupAssetInfo()) return;

        SerializeHeaders();
        
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        List<string> waveProps = new();
        foreach (var prop in properties.Properties()) 
        {
            if (!waveProps.Contains(prop.Name)) 
            {
                waveProps.Add(prop.Name);
            }
        }
        properties.Add("$ReferencedObjects", new JArray());
        AssetData.Add("AssetObjectData", properties);
        
        var path2 = Path.ChangeExtension(OutPath, "fbx");
        if (!File.Exists(path2)) 
        {
            IsSkipped = true;
            SkippedCode = "No FBX file supplied!";
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
}