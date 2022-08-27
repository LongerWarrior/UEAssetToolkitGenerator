namespace UAssetAPI.StructTypes; 
public struct FMovieSceneTrackFieldData {
	public TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier> Field;

        public FMovieSceneTrackFieldData(TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier> field) {
            Field = field;
        }

	public FMovieSceneTrackFieldData Read(AssetBinaryReader reader) {
		Field = new TMovieSceneEvaluationTree<FMovieSceneTrackIdentifier>();

		Field.RootNode = new FMovieSceneEvaluationTreeNode();
		Field.RootNode.Read(reader);

		Field.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
		int entriesamount = reader.ReadInt32();
		Field.ChildNodes.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Field.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		int itemsamount = reader.ReadInt32();

		Field.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			Field.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
			Field.ChildNodes.Items[i].Read(reader);
		}

		Field.Data = new TEvaluationTreeEntryContainer<FMovieSceneTrackIdentifier>();

		entriesamount = reader.ReadInt32();
		Field.Data.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Field.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		itemsamount = reader.ReadInt32();

		Field.Data.Items = new FMovieSceneTrackIdentifier[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			Field.Data.Items[i] = new FMovieSceneTrackIdentifier(reader.ReadUInt32());
		}
		return new FMovieSceneTrackFieldData(Field);
	}

	public void Write(AssetBinaryWriter writer) {

		Field.RootNode.Write(writer);
		int entriesamount = Field.ChildNodes.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Field.ChildNodes.Entries[i].Write(writer);
		}

		int itemsamount = Field.ChildNodes.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			Field.ChildNodes.Items[i].Write(writer);
		}

		entriesamount = Field.Data.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Field.Data.Entries[i].Write(writer);
		}
		itemsamount = Field.Data.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			Field.Data.Items[i].Write(writer);
		}

	}

	public JObject ToJson() {
		JObject res = new JObject();

		JObject serdata = new JObject();
		serdata.Add("RootNode", Field.RootNode.ToJson());

		JArray entries = new JArray();
		JArray items = new JArray();
		foreach (FEntry entry in Field.ChildNodes.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FMovieSceneEvaluationTreeNode item in Field.ChildNodes.Items) {
			items.Add(item.ToJson());
		}
		JObject childnodes = new JObject();

		childnodes.Add("Entries", entries);
		childnodes.Add("Items", items);
		serdata.Add("ChildNodes", childnodes);

		entries = new JArray();
		items = new JArray();
		foreach (FEntry entry in Field.Data.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FMovieSceneTrackIdentifier item in Field.Data.Items) {
			items.Add(item.ToJson());
		}
		JObject data = new JObject();
		data.Add("Entries", entries);
		data.Add("Items", items);
		serdata.Add("Data", data);
		res.Add("Field", serdata);

		return res;
	}
}


