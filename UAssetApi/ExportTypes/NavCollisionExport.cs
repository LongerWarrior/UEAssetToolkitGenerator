namespace UAssetAPI;

public class NavConsts {
    public const int Initial = 1;
    public const int AreaClass = 2;
    public const int ConvexTransforms = 3;
    public const int ShapeGeoExport = 4;
    public const int Latest = ShapeGeoExport;
    public const uint Magic = 0xA237F237;
}

public class NavCollisionExport : NormalExport
{
    public FFormatContainer? CookedFormatData;
    public FPackageIndex AreaClass; // UNavArea
    public Guid NavCollisionGuid;
    public uint myMagic;
    public int version;
    public bool bCooked;
    public bool bTemp;

    public NavCollisionExport(Export super) : base(super)
    {

    }


    public NavCollisionExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
        var idk = reader.ReadInt32();

        var startPos = reader.BaseStream.Position;
        myMagic = reader.ReadUInt32();

        if (myMagic != NavConsts.Magic) {
            version = NavConsts.Initial;
            reader.BaseStream.Position = startPos;
        } else {
            version = reader.ReadInt32();
        }

        NavCollisionGuid = new Guid(reader.ReadBytes(16)); // Zeroed GUID, unused
        bCooked = reader.ReadIntBoolean();

        if (bCooked)
            CookedFormatData = new FFormatContainer(reader);

        if (version >= NavConsts.AreaClass)
            AreaClass = reader.XFERPTR();
    }
    

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);
        if (myMagic == NavConsts.Magic) {
            writer.Write(myMagic);
            writer.Write(version);
        }
        writer.Write(NavCollisionGuid.ToByteArray());
        writer.Write(bCooked ? 1 : 0);
        if (bCooked) {
            CookedFormatData.Write(writer);
        }
        if (version >= NavConsts.AreaClass) {
            writer.XFERPTR(AreaClass);
        }

    }
}
