namespace UAssetAPI;

public class UserDefinedStructExport : StructExport
{
    public EStructFlags StructFlags;
    public List<PropertyData> DefaultStructInstance;

    public UserDefinedStructExport(Export super) : base(super)
    {

    }


    public UserDefinedStructExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        StructFlags = (EStructFlags)reader.ReadInt32();

        List<PropertyData> resultingList = new List<PropertyData>();
        PropertyData data = null;
        while ((data = MainSerializer.Read(reader, true)) != null) {
            resultingList.Add(data);
        }

        DefaultStructInstance = resultingList;

    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);
        writer.Write((uint)StructFlags);

        if (DefaultStructInstance != null) {
            foreach (var t in DefaultStructInstance) {
                MainSerializer.Write(t, writer, true);
            }
        }
        writer.Write(new FName("None"));

    }
}
