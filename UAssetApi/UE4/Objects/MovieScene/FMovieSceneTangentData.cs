namespace UAssetAPI.StructTypes; 
public struct FMovieSceneTangentData {
	public float ArriveTangent; // 0x00(0x04)
	public float LeaveTangent; // 0x04(0x04)
	public float ArriveTangentWeight; // 0x08(0x04)
	public float LeaveTangentWeight; // 0x0c(0x04)
	public ERichCurveTangentWeightMode TangentWeightMode; // 0x10(0x01)
	public byte[] padding;
	//char pad_11[0x3]; // 0x11(0x03)
	public JObject ToJson() {
		JObject value = new JObject();

		value.Add("ArriveTangent", ArriveTangent);
		value.Add("LeaveTangent", LeaveTangent);
		value.Add("ArriveTangentWeight", ArriveTangentWeight);
		value.Add("LeaveTangentWeight", LeaveTangentWeight);
		value.Add("TangentWeightMode", TangentWeightMode.ToString());

		return value;
	}

	public void Write(AssetBinaryWriter writer) {
		writer.Write(ArriveTangent);
		writer.Write(LeaveTangent);
		writer.Write(ArriveTangentWeight);
		writer.Write(LeaveTangentWeight);
		writer.Write((byte)TangentWeightMode);
		writer.Write(padding);
	}
}


