namespace CookedAssetSerializer;

public struct JSONSettings
{
    [JsonProperty] public string ContentDir;
    [JsonProperty] public string AssetRegistry;
    [JsonProperty] public List<string> ParseDir;
    [JsonProperty] public string JSONDir;
    [JsonProperty] public string CookedDir;
    [JsonProperty] public string InfoDir;
    [JsonProperty] public string DefaultGameConfig;
    [JsonProperty] public UE4Version GlobalUEVersion;
    [JsonProperty] public bool RefreshAssets;
    [JsonProperty] public bool DummyWithProps;
    [JsonProperty] public List<EAssetType> SkipSerialization;
    [JsonProperty] public List<EAssetType> DummyAssets;
    [JsonProperty] public List<EAssetType> DeleteAssets;
    [JsonProperty] public List<string> CircularDependency;
    [JsonProperty] public List<string> SimpleAssets;
    [JsonProperty] public List<string> TypesToCopy;
    [JsonProperty] public int SelectedIndex;
}