namespace UAssetAPI.StructTypes; 

public struct FMovieSceneSubSequenceTree {
	public TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry> Data;

    public FMovieSceneSubSequenceTree(TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry> data) {
        Data = data;
    }

    public FMovieSceneSubSequenceTree Read(AssetBinaryReader reader) {

		Data = new TMovieSceneEvaluationTree<FMovieSceneSubSequenceTreeEntry>();

		Data.RootNode = new FMovieSceneEvaluationTreeNode();
		Data.RootNode.Read(reader);

		Data.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
		int entriesamount = reader.ReadInt32();
		Data.ChildNodes.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Data.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		int itemsamount = reader.ReadInt32();

		Data.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			Data.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
			Data.ChildNodes.Items[i].Read(reader);
		}

		Data.Data = new TEvaluationTreeEntryContainer<FMovieSceneSubSequenceTreeEntry>();

		entriesamount = reader.ReadInt32();
		Data.Data.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Data.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		itemsamount = reader.ReadInt32();

		Data.Data.Items = new FMovieSceneSubSequenceTreeEntry[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			Data.Data.Items[i] = new FMovieSceneSubSequenceTreeEntry(new FMovieSceneSequenceID(reader.ReadUInt32()), reader.ReadByte());
		}
		return new FMovieSceneSubSequenceTree(Data);
	}

	public void Write(AssetBinaryWriter writer) {

		Data.RootNode.Write(writer);
		int entriesamount = Data.ChildNodes.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Data.ChildNodes.Entries[i].Write(writer);
		}

		int itemsamount = Data.ChildNodes.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			Data.ChildNodes.Items[i].Write(writer);
		}

		entriesamount = Data.Data.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Data.Data.Entries[i].Write(writer);
		}
		itemsamount = Data.Data.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			Data.Data.Items[i].Write(writer);
		}

	}
	public JObject ToJson() {
		JObject res = new JObject();

		JObject serdata = new JObject();
		serdata.Add("RootNode", Data.RootNode.ToJson());

		JArray entries = new JArray();
		JArray items = new JArray();
		foreach (FEntry entry in Data.ChildNodes.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FMovieSceneEvaluationTreeNode item in Data.ChildNodes.Items) {
			items.Add(item.ToJson());
		}
		JObject childnodes = new JObject();

		childnodes.Add("Entries", entries);
		childnodes.Add("Items", items);
		serdata.Add("ChildNodes", childnodes);

		entries = new JArray();
		items = new JArray();
		foreach (FEntry entry in Data.Data.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FMovieSceneSubSequenceTreeEntry item in Data.Data.Items) {
			items.Add(item.ToJson());
		}
		JObject data = new JObject();
		data.Add("Entries", entries);
		data.Add("Items", items);
		serdata.Add("Data", data);
		res.Add("Data", serdata);

		return res;
	}
}


