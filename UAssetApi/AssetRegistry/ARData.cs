namespace UAssetAPI.AssetRegistry;

public struct AssetData {
    public readonly string AssetClass;
    public readonly string AssetName;
    //public IDictionary<string, string> TagsAndValues;

    public AssetData(FName assetClass, FName assetName, IDictionary<FName, string> tagsAndValues) {
        AssetClass = assetClass.ToName();
        AssetName = assetName.ToName();
        //if (tagsAndValues.Count == 0) { TagsAndValues = null;
        //    return;
        //}
        //TagsAndValues = null;
        //TagsAndValues = new Dictionary<string, string>(tagsAndValues.Count);
        //foreach (KeyValuePair<FName, string> item in tagsAndValues) {
        //    TagsAndValues[item.Key.ToName()] = item.Value;
        //}
    }
}

public static class ARData {
    public static Dictionary<string, AssetData> AssetList = new();
}