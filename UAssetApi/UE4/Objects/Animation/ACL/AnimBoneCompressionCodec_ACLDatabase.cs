namespace  UAssetAPI;

/*[JsonConverter(typeof(FACLDatabaseCompressedAnimDataConverter))]
public class FACLDatabaseCompressedAnimData : ICompressedAnimData
{
    public int CompressedNumberOfFrames { get; set; }

    /** Maps the compressed_tracks instance. Used in cooked build only. #1#
    public byte[] CompressedByteStream;

    /** The codec instance that owns us. #1#
    public UAnimBoneCompressionCodec_ACLDatabase? Codec;

    /** The sequence name hash that owns this data. #1#
    public uint SequenceNameHash;

    /#1#** Holds the compressed_tracks instance for the anim sequence #2#
    public byte[] CompressedClip;#1#

    public void SerializeCompressedData(FAssetArchive Ar)
    {
        ((ICompressedAnimData) this).BaseSerializeCompressedData(Ar);

        SequenceNameHash = Ar.Read<uint>();

        /*if (!Ar.Owner.HasFlags(EPackageFlags.PKG_FilterEditorOnly))
        {
            CompressedClip = Ar.ReadArray<byte>();
        }#1#
    }

    public void Bind(byte[] bulkData)
    {
        //var compressedClipData = new CompressedTracks(bulkData);
        throw new NotImplementedException();
    }
}

public class FACLDatabaseCompressedAnimDataConverter : JsonConverter<FACLDatabaseCompressedAnimData>
{
    public override void WriteJson(JsonWriter writer, FACLDatabaseCompressedAnimData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("CompressedNumberOfFrames");
        writer.WriteValue(value.CompressedNumberOfFrames);

        writer.WritePropertyName("SequenceNameHash");
        writer.WriteValue(value.SequenceNameHash);

        writer.WriteEndObject();
    }

    public override FACLDatabaseCompressedAnimData ReadJson(JsonReader reader, Type objectType, FACLDatabaseCompressedAnimData existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class UAnimBoneCompressionCodec_ACLDatabase : UAnimBoneCompressionCodec_ACLBase
{
    public override ICompressedAnimData AllocateAnimData() => new FACLDatabaseCompressedAnimData { Codec = this };
}*/