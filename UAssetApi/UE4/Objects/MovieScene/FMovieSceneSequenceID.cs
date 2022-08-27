namespace UAssetAPI.StructTypes; 
public class FMovieSceneSequenceID {
	public uint Value;

	public FMovieSceneSequenceID(uint value) {
		Value = value;
	}
	public JObject ToJson() {
		JObject value = new JObject();
		value.Add("Value", Value);
		return value;
	}
}


