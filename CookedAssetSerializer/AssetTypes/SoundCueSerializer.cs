using static UAssetAPI.Kismet.KismetSerializer;

namespace CookedAssetSerializer.AssetTypes
{
    public class SoundCueSerializer : SimpleAssetSerializer<SoundCueExport>
    {
        public SoundCueSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
        {
            DisableGeneration.Add("FirstNode");
            if (!Setup()) return;
            SoundGraphData = new Dictionary<int, List<int>>();
            SerializeAsset(new JProperty("SoundCueGraph", string.Join(Environment.NewLine, ClassExport.GetCueGraph())));
        }
    }
}