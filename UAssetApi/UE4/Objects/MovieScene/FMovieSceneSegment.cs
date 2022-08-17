namespace UAssetAPI.StructTypes; 
public class FMovieSceneSegment {

	public FFrameNumberRange Range;
	public FMovieSceneSegmentIdentifier ID;
	public bool bAllowEmpty;
	public List<PropertyData>[] Impls;
	//public FSectionEvaluationData[] Impls;
	
	public JToken ToJson() {
		JObject value = new JObject();
		value.Add("Range", Range.ToJson());
		value.Add("ID", ID.IdentifierIndex);
		value.Add("bAllowEmpty", bAllowEmpty);

		JArray jimpls = new JArray();
            /*for (int i = 0; i < Impls.Length; i++) {
			jimpls.Add(new JObject().Add());
            }*/
		value.Add("Impls", jimpls);
		return value;
	}
}


