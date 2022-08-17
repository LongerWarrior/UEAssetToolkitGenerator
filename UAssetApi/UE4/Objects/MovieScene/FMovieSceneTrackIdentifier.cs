namespace UAssetAPI.StructTypes; 

public class FMovieSceneTrackIdentifier {
	public uint Value;

	public FMovieSceneTrackIdentifier(uint value) {
		Value = value;
	}

	public JObject ToJson() {
		JObject value = new JObject();
		value.Add("Value", Value);
		return value;
	}

    public void Write(AssetBinaryWriter writer) {
		writer.Write(Value);
    }
}

