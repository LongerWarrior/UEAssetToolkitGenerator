namespace UAssetAPI.StructTypes;

public struct FSectionEvaluationDataTree {

	public TMovieSceneEvaluationTree<List<PropertyData>> Tree;

    public FSectionEvaluationDataTree(TMovieSceneEvaluationTree<List<PropertyData>> tree) {
        Tree = tree;
    }

	public FSectionEvaluationDataTree Read(AssetBinaryReader reader) {

		Tree = new TMovieSceneEvaluationTree<List<PropertyData>>();

		Tree.RootNode = new FMovieSceneEvaluationTreeNode();
		Tree.RootNode.Read(reader);

		Tree.ChildNodes = new TEvaluationTreeEntryContainer<FMovieSceneEvaluationTreeNode>();
		int entriesamount = reader.ReadInt32();
		Tree.ChildNodes.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Tree.ChildNodes.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		int itemsamount = reader.ReadInt32();

		Tree.ChildNodes.Items = new FMovieSceneEvaluationTreeNode[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			Tree.ChildNodes.Items[i] = new FMovieSceneEvaluationTreeNode();
			Tree.ChildNodes.Items[i].Read(reader);
		}

		Tree.Data = new TEvaluationTreeEntryContainer<List<PropertyData>>();

		entriesamount = reader.ReadInt32();
		Tree.Data.Entries = new FEntry[entriesamount];
		for (int i = 0; i < entriesamount; i++) {
			Tree.Data.Entries[i] = new FEntry(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
		}
		itemsamount = reader.ReadInt32();

		List<PropertyData>[] items = new List<PropertyData>[itemsamount];
		for (int i = 0; i < itemsamount; i++) {
			List<PropertyData> resultingList = new List<PropertyData>();
			PropertyData data = null;
			while ((data = MainSerializer.Read(reader, true)) != null) {
				resultingList.Add(data);
			}
			items[i] = resultingList;
		}
		Tree.Data.Items = items;
		//Tree.Data.Items = new StructPropertyData[itemsamount];
		//for (int i = 0; i < itemsamount; i++) {
		//	Tree.Data.Items[i] = new StructPropertyData(new FName("Impls"), new FName("SectionEvaluationData"));
		//	Tree.Data.Items[i].Read(reader, false, 1);
		//}
		return new FSectionEvaluationDataTree(Tree);
	}

	public void Write(AssetBinaryWriter writer) {

		Tree.RootNode.Write(writer);
		int entriesamount = Tree.ChildNodes.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Tree.ChildNodes.Entries[i].Write(writer);
		}

		int itemsamount = Tree.ChildNodes.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {
			Tree.ChildNodes.Items[i].Write(writer);
		}

		entriesamount = Tree.Data.Entries.Length;
		writer.Write(entriesamount);
		for (int i = 0; i < entriesamount; i++) {
			Tree.Data.Entries[i].Write(writer);
		}
		itemsamount = Tree.Data.Items.Length;
		writer.Write(itemsamount);
		for (int i = 0; i < itemsamount; i++) {

			if (Tree.Data.Items[i] != null) {
				foreach (var t in Tree.Data.Items[i]) {
					MainSerializer.Write(t, writer, true);
				}
			}
			writer.Write(new FName("None"));
		}

	}
	public JObject ToJson() {
		JObject res = new JObject();

		//JObject serdata = new JObject();
		//serdata.Add("RootNode", Tree.RootNode.ToJson());

		//JArray entries = new JArray();
		//JArray items = new JArray();
		//foreach (FEntry entry in Tree.ChildNodes.Entries) {
		//	entries.Add(entry.ToJson());
		//}
		//foreach (FMovieSceneEvaluationTreeNode item in Tree.ChildNodes.Items) {
		//	items.Add(item.ToJson());
		//}
		//JObject childnodes = new JObject();

		//childnodes.Add("Entries", entries);
		//childnodes.Add("Items", items);
		//serdata.Add("ChildNodes", childnodes);

		//entries = new JArray();
		//items = new JArray();
		//foreach (FEntry entry in Tree.Data.Entries) {
		//	entries.Add(entry.ToJson());
		//}


		////foreach (List<PropertyData> item in Tree.Data.Items) {
		////	items.Add(item.ToJson());
		//	//items.Add(Program.S item);
		////}

		//JObject data = new JObject();
		//data.Add("Entries", entries);
		//data.Add("Items", items);
		//serdata.Add("Data", data);
		//res.Add("Tree", serdata);

		return res;
	}
}


