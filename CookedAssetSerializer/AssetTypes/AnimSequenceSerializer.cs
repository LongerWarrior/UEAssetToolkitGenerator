using System.Security.Cryptography;

namespace CookedAssetSerializer.AssetTypes;

public class AnimSequenceSerializer : SimpleAssetSerializer<NormalExport>
{
    public AnimSequenceSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup(false, true, "AnimSequence")) return;
        SerializeAsset(null, /*null*/GetModelHash(), null, 
            null, false, false, true);
    }
    
    private JProperty GetModelHash()
    {
        var path2 = "";
        if (Settings.UseAMActorX)
        {
            path2 = Path.ChangeExtension(OutPath, "pskx");
        }
        else
        {
            path2 = Path.ChangeExtension(OutPath, "fbx");
        }
        
        if (!File.Exists(path2)) 
        {
            IsSkipped = true;
            SkippedCode = "No FBX file supplied!";
            return new JProperty("None");
        }
        using (var md5 = MD5.Create()) 
        {
            using var stream1 = File.OpenRead(path2);
            var hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            return new JProperty("ModelFileHash", hash);
        }
    }
}