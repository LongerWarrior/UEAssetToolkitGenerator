using UAssetAPI.StructTypes.SkeletalMesh;

namespace UAssetAPI;

/// <summary>
/// Export for a skeletal mesh asset.
/// </summary>
public class SkeletalMeshExport : NormalExport
{
    public FBoxSphereBounds ImportedBounds;
    public FSkeletalMaterial[] Materials;
    public FReferenceSkeleton ReferenceSkeleton;
    public FStaticLODModel[] LODModels;
    public bool bHasVertexColors;
    public byte NumVertexColorChannels;
    public FPackageIndex[] MorphTargets;
    public SkeletalMeshExport(Export super) : base(super)
    {

    }

    public SkeletalMeshExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
        reader.ReadInt32();

        // GetOrDefault<bool>(nameof(bHasVertexColors));
        bHasVertexColors = false;
        if (this["bHasVertexColors"] is BoolPropertyData _boolprop) {
            bHasVertexColors = _boolprop.Value; 
        }

        NumVertexColorChannels = 0;//GetOrDefault<byte>(nameof(NumVertexColorChannels));

        // GetOrDefault<FPackageIndex[]>(nameof(MorphTargets));
        MorphTargets = Array.Empty<FPackageIndex>();
        if (this["MorphTargets"] is ArrayPropertyData mtarray) {
            MorphTargets = new FPackageIndex[mtarray.Value.Length];
            for (int i = 0; i < mtarray.Value.Length; i++) {
                MorphTargets[i] = ((ObjectPropertyData)mtarray.Value[i]).Value;
            }
        }



        var stripDataFlags = new FStripDataFlags(reader);
        ImportedBounds = new FBoxSphereBounds(reader.ReadVector(), reader.ReadVector(), reader.ReadSingle());
        Materials = reader.ReadArray(() => new FSkeletalMaterial(reader));
        ReferenceSkeleton = new FReferenceSkeleton();
        ReferenceSkeleton.Read(reader);

        if (reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>() < FSkeletalMeshCustomVersion.SplitModelAndRenderData) {
            LODModels = reader.ReadArray(() => new FStaticLODModel(reader, bHasVertexColors));
        } else {
            if (!stripDataFlags.IsEditorDataStripped()) {
                LODModels = reader.ReadArray(() => new FStaticLODModel(reader, bHasVertexColors));
            }

            var bCooked = reader.ReadIntBoolean();
            if (reader["SkeletalMesh.KeepMobileMinLODSettingOnDesktop"]) {
                var minMobileLODIdx = reader.ReadInt32();
            }

            if (bCooked && LODModels == null) {
                var useNewCookedFormat = reader["SkeletalMesh.UseNewCookedFormat"];
                LODModels = new FStaticLODModel[reader.ReadInt32()];
                for (var i = 0; i < LODModels.Length; i++) {
                    LODModels[i] = new FStaticLODModel();
                    if (useNewCookedFormat) {
                        LODModels[i].SerializeRenderItem(reader, bHasVertexColors, NumVertexColorChannels);
                    } else {
                        LODModels[i].SerializeRenderItem_Legacy(reader, bHasVertexColors, NumVertexColorChannels);
                    }
                }

                if (useNewCookedFormat) {
                    var numInlinedLODs = reader.ReadByte();
                    var numNonOptionalLODs = reader.ReadByte();
                }
            }
        }

        if (reader.Ver <  UE4Version.VER_UE4_REFERENCE_SKELETON_REFACTOR) {
            var length = reader.ReadInt32();
            reader.BaseStream.Position += 12 * length; // TMap<FName, int32> DummyNameIndexMap
        }

        var dummyObjs = reader.ReadArray(() => reader.XFERPTR());

    }

    public override void Write(AssetBinaryWriter writer) {
        base.Write(writer);

        writer.Write((int)0);

    }
}
