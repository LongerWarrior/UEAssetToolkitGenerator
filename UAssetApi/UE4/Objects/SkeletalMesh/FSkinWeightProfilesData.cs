namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkinWeightProfilesData
{
    public readonly Dictionary<FName, FRuntimeSkinWeightProfileData> OverrideData;

    public FSkinWeightProfilesData(AssetBinaryReader reader)
    {
        var length = reader.ReadInt32();
        OverrideData = new Dictionary<FName, FRuntimeSkinWeightProfileData>();
        for (var i = 0; i < length; i++)
        {
            OverrideData[reader.ReadFName()] = new FRuntimeSkinWeightProfileData(reader);
        }
    }
}