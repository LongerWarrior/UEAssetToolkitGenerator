using System.Collections.Generic;
using Newtonsoft.Json;
using UAssetAPI;

namespace CookedAssetSerializer
{
    public struct Settings
    {
        [JsonProperty] public string ContentDir;
        [JsonProperty] public string JSONDir;
        [JsonProperty] public string OutputDir;
        [JsonProperty] public UE4Version GlobalUEVersion;
        [JsonProperty] public bool RefreshAssets;
        [JsonProperty] public List<EAssetType> SkipSerialization;
        [JsonProperty] public List<string> CircularDependency;
        [JsonProperty] public List<string> SimpleAssets;
        [JsonProperty] public List<string> TypesToCopy;
        [JsonProperty] public int SelectedIndex;
    }
}