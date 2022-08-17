namespace UAssetAPI.StructTypes;

public struct FSectionEvaluationData {
    public int ImplIndex; // 0x00(0x04)
    public FFrameNumber ForcedTime; // 0x04(0x04)
	public ESectionEvaluationFlags Flags; // 0x08(0x01)

    public FSectionEvaluationData(int implIndex, FFrameNumber forcedTime, byte flags) {
        ImplIndex = implIndex;
        ForcedTime = forcedTime;
        Flags = (ESectionEvaluationFlags)flags;
    }

	public void Write(AssetBinaryWriter writer) {
		writer.Write(ImplIndex);
		writer.Write(ForcedTime.Value);
		writer.Write((byte)Flags);
	}
	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("ImplIndex", ImplIndex);
		res.Add("ForcedTime", ForcedTime.ToJson());
		res.Add("Flags", Flags.ToString());
		return res;
	}

}


