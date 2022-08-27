namespace UAssetAPI.StructTypes;

public struct FMovieSceneEvaluationTreeNode {
	/** The time-range that this node represents */
	public FFrameNumberRange Range;
	public FMovieSceneEvaluationTreeNodeHandle Parent;
	/** Identifier for the child node entries associated with this node (FMovieSceneEvaluationTree::ChildNodes) */
	public FEvaluationTreeEntryHandle ChildrenID;
	/** Identifier for externally stored data entries associated with this node */
	public FEvaluationTreeEntryHandle DataID;

	public void Read(AssetBinaryReader reader) {
		Range = new FFrameNumberRange();
		Range.Read(reader);
		Parent = new FMovieSceneEvaluationTreeNodeHandle(reader.ReadInt32(), reader.ReadInt32());
		ChildrenID = new FEvaluationTreeEntryHandle(reader.ReadInt32());
		DataID = new FEvaluationTreeEntryHandle(reader.ReadInt32());
	}

	public void Write(AssetBinaryWriter writer) {
		Range.Write(writer);
		writer.Write(Parent.ChildrenHandle.EntryIndex);
		writer.Write(Parent.Index);
		writer.Write(ChildrenID.EntryIndex);
		writer.Write(DataID.EntryIndex);
	}
	public JObject ToJson() {
		JObject res = new JObject();

		res.Add("Range", Range.ToJson());
		res.Add("Parent", Parent.ToJson());
		res.Add("ChildrenID", ChildrenID.ToJson());
		res.Add("DataID", DataID.ToJson());

		return res;
	}

}


