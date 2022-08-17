namespace UAssetAPI;

public class BodySetupExport : NormalExport
{
    public Guid BodySetupGuid;
    public FFormatContainer CookedFormatData;
    public bool bCooked;
    public bool bTemp;

    public BodySetupExport(Export super) : base(super)
    {

    }


    public BodySetupExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        BodySetupGuid = new Guid (reader.ReadBytes(16));
        bCooked = reader.ReadIntBoolean();
        if (!bCooked) return;
        if (reader.Ver >= UE4Version.VER_UE4_STORE_HASCOOKEDDATA_FOR_BODYSETUP) {
            bTemp = reader.ReadIntBoolean(); 
        }

        CookedFormatData = new FFormatContainer(reader);
    }
    

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);
        writer.Write(((Guid)BodySetupGuid).ToByteArray());
        writer.Write(bCooked?1:0);
        if (!bCooked) return;
        if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_STORE_HASCOOKEDDATA_FOR_BODYSETUP) {
            writer.Write(bTemp ? 1 : 0);
        }
        CookedFormatData.Write(writer);
    }
}
