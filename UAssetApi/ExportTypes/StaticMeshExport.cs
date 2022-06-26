
namespace UAssetAPI
{

    public class StaticMeshExport : NormalExport {

        public FStripDataFlags stripDataFlags;
        public bool bCooked;
        public FPackageIndex BodySetup;
        public FPackageIndex NavCollision;

        public StaticMeshExport(Export super) : base(super) {

        }
        public StaticMeshExport() {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting) {
            base.Read(reader, nextStarting);

            int idk = reader.ReadInt32();
            stripDataFlags = new FStripDataFlags(reader);
            bool bCooked = reader.ReadInt32() !=0;

            BodySetup = reader.XFER_OBJECT_POINTER();
            if (reader.Asset.EngineVersion >= UE4Version.VER_UE4_STATIC_MESH_STORE_NAV_COLLISION) 
            {
                NavCollision = reader.XFER_OBJECT_POINTER();
            }
        }

        public override void Write(AssetBinaryWriter writer) {
            base.Write(writer);
            writer.Write(0);

            stripDataFlags.Write(writer);
            writer.Write(bCooked ? 1 : 0);
            writer.XFER_OBJECT_POINTER(BodySetup);
            if (writer.Asset.EngineVersion >= UE4Version.VER_UE4_STATIC_MESH_STORE_NAV_COLLISION) {
                writer.XFER_OBJECT_POINTER(NavCollision);
            }
        }
    }
}
