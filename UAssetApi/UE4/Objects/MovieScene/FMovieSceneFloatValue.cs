namespace UAssetAPI.StructTypes; 

public struct FMovieSceneFloatValue {
	public float Value; // 0x00(0x04)
	public FMovieSceneTangentData Tangent; // 0x04(0x14)
	public ERichCurveInterpMode InterpMode; // 0x18(0x01)
	public ERichCurveTangentMode TangentMode; // 0x19(0x01)
	public short PaddingByte; // 0x1a(0x01)
							  //char pad_1B[0x1]; // 0x1b(0x01)
	public JObject ToJson() {
		JObject value = new JObject();

		value.Add("Value", Value);
		value.Add("Tangent", Tangent.ToJson());
		value.Add("InterpMode", InterpMode.ToString());
		value.Add("TangentMode", TangentMode.ToString());
		value.Add("PaddingByte", PaddingByte);

		return value;
	}
	public void Read(AssetBinaryReader reader) {
		Value = reader.ReadSingle();
		Tangent.ArriveTangent = reader.ReadSingle();
		Tangent.LeaveTangent = reader.ReadSingle();
		Tangent.ArriveTangentWeight = reader.ReadSingle();
		Tangent.LeaveTangentWeight = reader.ReadSingle();
		Tangent.TangentWeightMode = (ERichCurveTangentWeightMode)reader.ReadByte();
		Tangent.padding = reader.ReadBytes(3);
		InterpMode = (ERichCurveInterpMode)reader.ReadSByte();
		TangentMode = (ERichCurveTangentMode)reader.ReadSByte();
		PaddingByte = reader.ReadInt16();
	}
	public void Write(AssetBinaryWriter writer) {
		writer.Write(Value);
		Tangent.Write(writer);
		writer.Write((sbyte)InterpMode);
		writer.Write((sbyte)TangentMode);
		writer.Write(PaddingByte);
	}
}


