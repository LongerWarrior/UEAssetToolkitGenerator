namespace UAssetAPI;

public class FFormatContainer {
    public SortedDictionary<FName, FByteBulkData> Formats;

    public FFormatContainer(AssetBinaryReader reader) {
        int numFormats = reader.ReadInt32();
        Formats = new SortedDictionary<FName, FByteBulkData>();
        for (var i = 0; i < numFormats; i++) {
            Formats[reader.ReadFName()] = new FByteBulkData(reader);
        }
    }

    public void Write(AssetBinaryWriter writer) {
        writer.Write(Formats.Count);
        foreach(KeyValuePair < FName, FByteBulkData> entry in Formats) {
            writer.Write(entry.Key);
            entry.Value.Write(writer);
        }
    }
}
