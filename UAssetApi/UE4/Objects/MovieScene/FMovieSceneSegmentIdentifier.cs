namespace UAssetAPI.StructTypes; 
public struct FMovieSceneSegmentIdentifier {
	public int IdentifierIndex; // 0x00(0x04)

	public FMovieSceneSegmentIdentifier(int identifierIndex) {
		IdentifierIndex = identifierIndex;
	}

	public void Write(AssetBinaryWriter writer) {
		writer.Write(IdentifierIndex);
        }
	public JToken ToJson() {
		JObject value = new JObject();
		value.Add("IdentifierIndex", IdentifierIndex);
		return value;
	}
}


