namespace UAssetAPI.StructTypes; 

public struct FMovieSceneSubSequenceTreeEntry {
	public FMovieSceneSequenceID SequenceID;
	public ESectionEvaluationFlags Flags;

        public FMovieSceneSubSequenceTreeEntry(FMovieSceneSequenceID sequenceID, byte flags) {
            SequenceID = sequenceID;
            Flags = (ESectionEvaluationFlags)flags;
        }

	public void Write(AssetBinaryWriter writer) {
		writer.Write(SequenceID.Value);
		writer.Write((byte)Flags);
        }

	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("SequenceID", SequenceID.ToJson());
		res.Add("Items", Flags.ToString());
		return res;
	}
}


