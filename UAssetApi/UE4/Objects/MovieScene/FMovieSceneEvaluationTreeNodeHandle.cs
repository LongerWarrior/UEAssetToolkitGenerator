namespace UAssetAPI.StructTypes;

public struct FMovieSceneEvaluationTreeNodeHandle {
	/** Entry handle for the parent's children in FMovieSceneEvaluationTree::ChildNodes */
	public FEvaluationTreeEntryHandle ChildrenHandle;
	/** The index of this child within its parent's children */
	public int Index;

	public FMovieSceneEvaluationTreeNodeHandle(int _ChildrenHandle, int _Index) {
		ChildrenHandle.EntryIndex = _ChildrenHandle;
		Index = _Index;
	}
	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("ChildrenHandle", ChildrenHandle.ToJson());
		res.Add("Index", Index);
		return res;
	}

}


