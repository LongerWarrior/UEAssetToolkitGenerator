using static UAssetAPI.Kismet.KismetSerializer;

namespace UAssetAPI;
public class SoundCueExport : NormalExport
{
    public FStripDataFlags SoundNodeStripDataFlags;
    public bool populated = false;


    public SoundCueExport(Export super) : base(super)
    {

    }


    public SoundCueExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        if (reader.Ver >= UE4Version.VER_UE4_COOKED_ASSETS_IN_EDITOR_SUPPORT) {
            SoundNodeStripDataFlags = new FStripDataFlags (reader);
        }
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write(0);

        if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_COOKED_ASSETS_IN_EDITOR_SUPPORT) {
            SoundNodeStripDataFlags.Write(writer);
        }
    }

    public void PopulateSoundCueInfo() {

        if (populated) return;

        if (FindPropertyData(Data, "FirstNode",out PropertyData prop)) {

            var FirstNode = ((ObjectPropertyData)prop).Value.Index;

            ((SoundNodeExport)Asset.Exports[FirstNode - 1]).Populate(null, new Guid("00000000000000000000000000000000"));

        }

        populated = true;

    }

    public string[] GetCueGraph() {
        PopulateSoundCueInfo();
        var cuegraph = new List<string>();
        for (int i = 0; i < Asset.Exports.Count; i++) {
            if ( Asset.Exports[i] is SoundNodeExport soundnode)
            cuegraph.AddRange(soundnode.ToGraphInfo());
        }
        return cuegraph.ToArray();
    }
}
