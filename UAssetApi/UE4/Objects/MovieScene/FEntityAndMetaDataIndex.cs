namespace UAssetAPI.StructTypes;

public struct FEntityAndMetaDataIndex {
	public int EntityIndex;
	public int MetaDataIndex;

        public FEntityAndMetaDataIndex(int entityIndex, int metaDataIndex) {
            EntityIndex = entityIndex;
            MetaDataIndex = metaDataIndex;
        }

	public void Write(AssetBinaryWriter writer) {
		writer.Write(EntityIndex);
		writer.Write(MetaDataIndex);
        }
	public JObject ToJson() {
		JObject res = new JObject();
		res.Add("EntityIndex", EntityIndex);
		res.Add("MetaDataIndex", MetaDataIndex);
		return res;
	}
}


