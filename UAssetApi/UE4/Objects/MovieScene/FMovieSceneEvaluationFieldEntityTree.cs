namespace UAssetAPI.StructTypes; 


public struct FMovieSceneEvaluationFieldEntityTree {

	public TMovieSceneEvaluationTree<FEntityAndMetaDataIndex> SerializedData;

	public FMovieSceneEvaluationFieldEntityTree(TMovieSceneEvaluationTree<FEntityAndMetaDataIndex> serializedData) {
		SerializedData = serializedData;
	}

	public FMovieSceneEvaluationFieldEntityTree Read(AssetBinaryReader reader) {
		SerializedData = new TMovieSceneEvaluationTree<FEntityAndMetaDataIndex>();

		SerializedData.RootNode = new FMovieSceneEvaluationTreeNode();
		SerializedData.RootNode.Read(reader);

		SerializedData.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
		int entriesamount = reader.ReadInt32();
		SerializedData.ChildNodes.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			SerializedData.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		int itemsamount = reader.ReadInt32();

		SerializedData.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			SerializedData.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
			SerializedData.ChildNodes.Items[i].Read(reader);
		}

		SerializedData.Data = new TEvaluationTreeEntryContainer<FEntityAndMetaDataIndex>();

		entriesamount = reader.ReadInt32();
		SerializedData.Data.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			SerializedData.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		itemsamount = reader.ReadInt32();

		SerializedData.Data.Items = new FEntityAndMetaDataIndex[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			SerializedData.Data.Items[i] = new FEntityAndMetaDataIndex(reader.ReadInt32(), reader.ReadInt32());
		}
		return new FMovieSceneEvaluationFieldEntityTree(SerializedData);
	}

	public void Write(AssetBinaryWriter writer) {

		SerializedData.RootNode.Write(writer);
		int entriesamount = SerializedData.ChildNodes.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			SerializedData.ChildNodes.Entries[i].Write(writer);
		}

		int itemsamount = SerializedData.ChildNodes.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			SerializedData.ChildNodes.Items[i].Write(writer);
		}

		entriesamount = SerializedData.Data.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			SerializedData.Data.Entries[i].Write(writer);
		}
		itemsamount = SerializedData.Data.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			SerializedData.Data.Items[i].Write(writer);
		}

	}

	public JObject ToJson() {
		JObject res = new JObject();

		JObject serdata = new JObject();
		serdata.Add("RootNode", SerializedData.RootNode.ToJson());

		JArray entries = new JArray();
		JArray items = new JArray();
		foreach (FEntry entry in SerializedData.ChildNodes.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FMovieSceneEvaluationTreeNode item in SerializedData.ChildNodes.Items) {
			items.Add(item.ToJson());
		}
		JObject childnodes = new JObject();

		childnodes.Add("Entries", entries);
		childnodes.Add("Items", items);
		serdata.Add("ChildNodes", childnodes);

		entries = new JArray();
		items = new JArray();
		foreach (FEntry entry in SerializedData.Data.Entries) {
			entries.Add(entry.ToJson());
		}
		foreach (FEntityAndMetaDataIndex item in SerializedData.Data.Items) {
			items.Add(item.ToJson());
		}
		JObject data = new JObject();
		data.Add("Entries", entries);
		data.Add("Items", items);
		serdata.Add("Data", data);
		res.Add("SerializedData", serdata);

		return res;
	}
}


