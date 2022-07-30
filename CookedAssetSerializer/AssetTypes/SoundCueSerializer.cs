using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static UAssetAPI.Kismet.KismetSerializer;

namespace CookedAssetSerializer.AssetTypes
{
    public class SoundCueSerializer : SimpleAssetSerializer<SoundCueExport>
    {
        public SoundCueSerializer(Settings settings, UAsset asset) : base(settings, asset)
        {
            DisableGeneration.Add("FirstNode");
            Setup();
            SoundGraphData = new Dictionary<int, List<int>>();
            SerializeAsset(new JProperty("SoundGraphData", string.Join(Environment.NewLine, ClassExport.GetCueGraph())));
        }
    }
}