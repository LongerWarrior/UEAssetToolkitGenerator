namespace UAssetAPI.StructTypes; 

// ScriptStruct MovieScene.MovieSceneFloatChannel
// Size: 0xa0 (Inherited: 0x08)
public class FMovieSceneFloatChannel {
	public ERichCurveExtrapolation PreInfinityExtrap; // 0x08(0x01)
	public ERichCurveExtrapolation PostInfinityExtrap; // 0x09(0x01)
	public FFrameNumber[] Times; // 0x10(0x10)
	public FMovieSceneFloatValue[] Values; // 0x20(0x10)
	public float DefaultValue; // 0x30(0x04)
	public bool bHasDefaultValue; // 0x34(0x01)
	//FMovieSceneKeyHandleMap KeyHandles; // 0x38(0x60)
	public FFrameRate TickResolution; // 0x98(0x08)

	public FMovieSceneFloatChannel() {
		PreInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
		PostInfinityExtrap = ERichCurveExtrapolation.RCCE_Constant;
		Times = new FFrameNumber[0];
		Values = new FMovieSceneFloatValue[0];
		DefaultValue = 0.0f;
		bHasDefaultValue = false;
		TickResolution = new FFrameRate(60000, 1);
        }
	public JObject ToJson() {
		JObject res = new JObject();

		res.Add(new JProperty("PreInfinityExtrap", PreInfinityExtrap.ToString()));
		res.Add(new JProperty("PostInfinityExtrap", PostInfinityExtrap.ToString()));

		JArray times = new JArray();
		foreach (FFrameNumber time in Times) {
			times.Add(time.ToJson());
		}
		res.Add(new JProperty("Times", times));

		JArray values = new JArray();
		foreach (FMovieSceneFloatValue value in Values) {
			values.Add(value.ToJson());
		}
		res.Add(new JProperty("Values", values));

		res.Add(new JProperty("DefaultValue", DefaultValue));
		res.Add(new JProperty("bHasDefaultValue", bHasDefaultValue));
		res.Add(new JProperty("TickResolution", TickResolution.ToJson()));

		return res;
	}

}


