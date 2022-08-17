namespace UAssetAPI.StructTypes;

public struct FEvaluationTreeEntryHandle {
	/** Specifies an index into TEvaluationTreeEntryContainer<T>::Entries */
	public int EntryIndex;
	public FEvaluationTreeEntryHandle (int _EntryIndex) {
		EntryIndex = _EntryIndex;

	}

	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("EntryIndex", EntryIndex);
		return res;
	}
}


