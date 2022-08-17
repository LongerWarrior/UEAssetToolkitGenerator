using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI
{

    public class StaticMeshExport : NormalExport {

        public FStripDataFlags stripDataFlags;
        public bool bCooked;
        public FPackageIndex BodySetup;
        public FPackageIndex NavCollision;
        public Guid LightingGuid;
        public FPackageIndex[] Sockets; // UStaticMeshSocket[]
        public FStaticMeshRenderData RenderData;
        public FStaticMaterial[] StaticMaterials;
        public FPackageIndex[] Materials; // UMaterialInterface[]

        public StaticMeshExport(Export super) : base(super) {

        }
        public StaticMeshExport() {

        }

        public override void Read(AssetBinaryReader reader, int nextStarting) {
            base.Read(reader, nextStarting);

            int idk = reader.ReadInt32();
            stripDataFlags = new FStripDataFlags(reader);
            bool bCooked = reader.ReadIntBoolean();

            BodySetup = reader.XFER_OBJECT_POINTER();
            if (reader.Ver >= UE4Version.VER_UE4_STATIC_MESH_STORE_NAV_COLLISION) {
                NavCollision = reader.XFER_OBJECT_POINTER();
            }

            if (!stripDataFlags.IsEditorDataStripped()) {
                Console.WriteLine("Static Mesh with Editor Data not implemented yet");
                //Ar.Position = validPos;
                return;
            }

            LightingGuid = new Guid(reader.ReadBytes(16)); // LocalLightingGuid
            var len = reader.ReadInt32();
            Sockets = new FPackageIndex[len];
            for (int i = 0; i < len; i++) {
                Sockets[i] = reader.XFERPTR();
            }

            RenderData = new FStaticMeshRenderData(reader, bCooked);

            if (bCooked && reader.Ver >= UE4Version.VER_UE4_20) {
                var bHasOccluderData = reader.ReadIntBoolean();
                if (bHasOccluderData) {
                    len = reader.ReadInt32();
                    var Vertices = new FVector[len];
                    for (int i = 0; i < len; i++) {
                        Vertices[i] = reader.ReadVector();
                    }
                    len = reader.ReadInt32();
                    var Indices = new ushort[len];
                    for (int i = 0; i < len; i++) {
                        Indices[i] = reader.ReadUInt16();
                    }
                }
            }

            if (reader.Ver >= UE4Version.VER_UE4_14) {
                var bHasSpeedTreeWind = reader.ReadIntBoolean();
                if (bHasSpeedTreeWind) {
                    //Skipping FSpeedTreeWind
                    if (reader.Ver >= UE4Version.VER_UE4_SPEEDTREE_WIND_V7) {
                        reader.BaseStream.Position += 1460;
                    } else {
                        reader.BaseStream.Position += 1448;
                    }
                }

                if (reader.Asset.GetCustomVersion<FEditorObjectVersion>() >= FEditorObjectVersion.RefactorMeshEditorMaterials) {
                    // UE4.14+ - "Materials" are deprecated, added StaticMaterials
                    len = reader.ReadInt32();
                    StaticMaterials = new FStaticMaterial[len];
                    for (int i = 0; i < len; i++) {
                        StaticMaterials[i] = new FStaticMaterial(reader);
                    }
                }
            }

            if (StaticMaterials is { Length: > 0 }) {
                Materials = new FPackageIndex[StaticMaterials.Length];
                for (var i = 0; i < Materials.Length; i++) {
                    Materials[i] = StaticMaterials[i].MaterialInterface;
                }
            }

            if (Materials is null && this["StaticMaterials"] is ArrayPropertyData smarray) {
                Materials = new FPackageIndex[smarray.Value.Length];
                for (int i = 0; i < smarray.Value.Length; i++) {
                    var staticmaterialstruct = (StructPropertyData)smarray.Value[i];
                    foreach(var prop in staticmaterialstruct.Value) {
                        if (prop.Name.ToName() == "MaterialInterface" && prop is ObjectPropertyData _interfaceprop) {
                            Materials[i] = _interfaceprop.Value;
                            break;
                        }
                    }
                }
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
