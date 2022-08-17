namespace UAssetAPI.StructTypes; 

public class FMovieSceneEvaluationKey {
	public FMovieSceneSequenceID SequenceID;
	public FMovieSceneTrackIdentifier TrackIdentifier;
	public uint SectionIndex;

	public FMovieSceneEvaluationKey(uint _SequenceID, uint _TrackIdentifier, uint _SectionIndex) {
		SequenceID = new FMovieSceneSequenceID(_SequenceID);
		TrackIdentifier = new FMovieSceneTrackIdentifier(_TrackIdentifier);
		SectionIndex = _SectionIndex;

	}
	public JObject ToJson() {
		JObject value = new JObject();
		value.Add("SequenceID", SequenceID.ToJson());
		value.Add("TrackIdentifier", TrackIdentifier.ToJson());
		value.Add("SectionIndex", SectionIndex);
		return value;
	}
}


