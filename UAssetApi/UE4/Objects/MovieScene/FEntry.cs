namespace UAssetAPI.StructTypes;

public struct FEntry {
	/** The index into Items of the first item */
	public int StartIndex;
	/** The number of currently valid items */
	public int Size;
	/** The total capacity of allowed items before reallocating */
	public int Capacity;

        public FEntry(int startIndex, int size, int capacity) {
            StartIndex = startIndex;
            Size = size;
            Capacity = capacity;
        }

	public void Write(AssetBinaryWriter writer) {
		writer.Write(StartIndex);
		writer.Write(Size);
		writer.Write(Capacity);
        }

	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("StartIndex", StartIndex);
		res.Add("Size", Size);
		res.Add("Capacity", Capacity);
		return res;
	}
 }


