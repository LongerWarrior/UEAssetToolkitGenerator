using static UAssetAPI.Kismet.KismetSerializer;

namespace CookedAssetSerializer.AssetTypes
{
    public class SoundCueSerializer : SimpleAssetSerializer<SoundCueExport>
    {
        public SoundCueSerializer(Settings settings, UAsset asset) : base(settings, asset)
        {
            DisableGeneration.Add("FirstNode");
            Setup(true);
            SoundGraphData = new Dictionary<int, List<int>>();
            SerializeAsset(new JProperty("SoundCueGraph", string.Join(Environment.NewLine, ClassExport.GetCueGraph())));
        }
    }
}