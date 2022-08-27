namespace UAssetAPI.StructTypes;

public struct FInt32RangeBound {
    public ERangeBoundTypes Type; // 0x00(0x01)
    public int Value; // 0x04(0x04)

    public JObject ToJson() {
        JObject value = new JObject();
        value.Add("Type", Type.ToString());
        value.Add("Value", Value);
        return value;
    }
}
