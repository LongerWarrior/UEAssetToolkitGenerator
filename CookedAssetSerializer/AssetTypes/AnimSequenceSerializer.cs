namespace CookedAssetSerializer.AssetTypes;

public class AnimSequenceSerializer : Serializer<AnimSequenceExport>
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
        
        DisableGeneration.Add("RawCurveData");
        DisableGeneration.Add("SequenceLength");
        DisableGeneration.Add("TrackToSkeletonMapTable");
        DisableGeneration.Add("NumFrames");

        if (!SetupAssetInfo()) return;

        SerializeHeaders();



        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
}