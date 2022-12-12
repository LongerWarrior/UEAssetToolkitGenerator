using System.Drawing;

namespace UAssetAPI.StructTypes.StaticMesh;

public class FColorVertexBuffer {
    [JsonConverter(typeof(ColorJsonConverter))]
    public readonly Color[] Data;
    public readonly int Stride;
    public readonly int NumVertices;

    public FColorVertexBuffer(AssetBinaryReader reader) {
        var stripDataFlags = new FStripDataFlags(reader);

        Stride = reader.ReadInt32();
        NumVertices = reader.ReadInt32();

        if (!stripDataFlags.IsDataStrippedForServer() & NumVertices > 0) {
            var size = reader.ReadInt32();
            var len = reader.ReadInt32();
            Data = new Color[len];
            for (int i = 0; i < len; i++) {
                Data[i] = Color.FromArgb(reader.ReadInt32());
            }
        } else {
            Data = Array.Empty<Color>();
        }
    }
}

public class FPositionVertexBuffer {
    public readonly FVector[] Verts;
    public readonly int Stride;
    public readonly int NumVertices;

    public FPositionVertexBuffer(AssetBinaryReader reader) {
        Stride = reader.ReadInt32();
        NumVertices = reader.ReadInt32();
        var size = reader.ReadInt32();
        var len = reader.ReadInt32();
        Verts = new FVector[len];
        for (int i = 0; i < len; i++) {
            Verts[i] = reader.ReadVector();
        }
    }
}


public class FWeightedRandomSampler : IUStruct {
    public readonly float[] Prob;
    public readonly int[] Alias;
    public readonly float TotalWeight;

    public FWeightedRandomSampler(AssetBinaryReader reader) {
        Prob = reader.ReadArray(() => reader.ReadSingle());
        Alias = reader.ReadArray(() => reader.ReadInt32());
        TotalWeight = reader.ReadSingle();
    }
}

public class FStaticMeshLODResources
{
    public FStaticMeshSection[] Sections { get; }
    public FCardRepresentationData? CardRepresentationData { get; set; }
    public float MaxDeviation { get; }
    public FPositionVertexBuffer? PositionVertexBuffer { get; private set; }
    public FStaticMeshVertexBuffer? VertexBuffer { get; private set; }
    public FColorVertexBuffer? ColorVertexBuffer { get; private set; }
    public FRawStaticIndexBuffer? IndexBuffer { get; private set; }
    public FRawStaticIndexBuffer? ReversedIndexBuffer { get; private set; }
    public FRawStaticIndexBuffer? DepthOnlyIndexBuffer { get; private set; }
    public FRawStaticIndexBuffer? ReversedDepthOnlyIndexBuffer { get; private set; }
    public FRawStaticIndexBuffer? WireframeIndexBuffer { get; private set; }
    public FRawStaticIndexBuffer? AdjacencyIndexBuffer { get; private set; }
    public bool SkipLod => VertexBuffer == null || IndexBuffer == null ||
                           PositionVertexBuffer == null || ColorVertexBuffer == null;

    public enum EClassDataStripFlag : byte
    {
        CDSF_AdjacencyData = 1,
        CDSF_MinLodData = 2,
        CDSF_ReversedIndexBuffer = 4,
        CDSF_RayTracingResources = 8,

        // PUBG all 3 bits set, no idea what indicates what, they're just always set.
        CDSF_StripIndexBuffers = 128 | 64 | 32
    }

    public FStaticMeshLODResources(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader);
        Sections = reader.ReadArray(() => new FStaticMeshSection(reader));
        MaxDeviation = reader.ReadSingle();

        if (!reader["StaticMesh.UseNewCookedFormat"])
        {
            if (!stripDataFlags.IsDataStrippedForServer() && !stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_MinLodData))
            {
                SerializeBuffersLegacy(reader, stripDataFlags);
            }

            return;
        }

        var bIsLODCookedOut = reader.ReadIntBoolean();
        var bInlined = reader.ReadIntBoolean();

        if (!stripDataFlags.IsDataStrippedForServer() && !bIsLODCookedOut) {
            if (bInlined) {
                SerializeBuffers(reader);
            } else {
                //var bulkData = new FByteBulkData(reader);
                //if (bulkData.Header.ElementCount > 0) {
                //    var tempAr = new FByteArchive("StaticMeshBufferReader", bulkData.Data, Ar.Versions);
                //    SerializeBuffers(tempAr);
                //    tempAr.Dispose();
                //}

                //// https://github.com/EpicGames/UnrealEngine/blob/4.27/Engine/Source/Runtime/Engine/Private/StaticMesh.cpp#L560
                //reader.BaseStream.Position += 8; // DepthOnlyNumTriangles + Packed
                //reader.BaseStream.Position += 4 * 4 + 2 * 4 + 2 * 4 + 5 * 2 * 4;
                //// StaticMeshVertexBuffer = 2x int32, 2x bool
                //// PositionVertexBuffer = 2x int32
                //// ColorVertexBuffer = 2x int32
                //// IndexBuffer = int32 + bool
                //// ReversedIndexBuffer
                //// DepthOnlyIndexBuffer
                //// ReversedDepthOnlyIndexBuffer
                //// WireframeIndexBuffer
                ////if (FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.RemovingTessellation)
                ////{
                ////    Ar.Position += 2 * 4; // AdjacencyIndexBuffer
                ////}
            }

            // FStaticMeshBuffersSize
            // uint32 SerializedBuffersSize = 0;
            // uint32 DepthOnlyIBSize       = 0;
            // uint32 ReversedIBsSize       = 0;
            reader.BaseStream.Position += 12;
        }
    }

    // Pre-UE4.23 code
    public void SerializeBuffersLegacy(AssetBinaryReader reader, FStripDataFlags stripDataFlags)
    {
        PositionVertexBuffer = new FPositionVertexBuffer(reader);
        VertexBuffer = new FStaticMeshVertexBuffer(reader);
        ColorVertexBuffer = new FColorVertexBuffer(reader);


        IndexBuffer = new FRawStaticIndexBuffer(reader);

        //if (Ar.Game != EGame.GAME_PlayerUnknownsBattlegrounds || !stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_StripIndexBuffers))
        //{
        if (reader.Ver >= UE4Version.VER_UE4_SOUND_CONCURRENCY_PACKAGE && !stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_ReversedIndexBuffer))
        {
            ReversedIndexBuffer = new FRawStaticIndexBuffer(reader);
            DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(reader);
            ReversedDepthOnlyIndexBuffer = new FRawStaticIndexBuffer(reader);
        }
        else
        {
            // UE4.8 or older, or when has CDSF_ReversedIndexBuffer
            DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(reader);
        }

        if (reader.Ver >= UE4Version.VER_UE4_FTEXT_HISTORY && reader.Ver < UE4Version.VER_UE4_RENAME_CROUCHMOVESCHARACTERDOWN)
        {
            var _ = new FDistanceFieldVolumeData(reader); // distanceFieldData
        }

        if (!stripDataFlags.IsEditorDataStripped())
            WireframeIndexBuffer = new FRawStaticIndexBuffer(reader);

        if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
            AdjacencyIndexBuffer = new FRawStaticIndexBuffer(reader);
        //}

        if (reader.Ver >  UE4Version.VER_UE4_16)
        {
            for (var i = 0; i < Sections.Length; i++)
            {
                var _ = new FWeightedRandomSampler(reader);
            }

            _ = new FWeightedRandomSampler(reader);
        }
    }

    public void SerializeBuffers(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader);

        PositionVertexBuffer = new FPositionVertexBuffer(reader);
        VertexBuffer = new FStaticMeshVertexBuffer(reader);
        ColorVertexBuffer = new FColorVertexBuffer(reader);

        IndexBuffer = new FRawStaticIndexBuffer(reader);

        if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_ReversedIndexBuffer))
        {
            ReversedIndexBuffer = new FRawStaticIndexBuffer(reader);
        }

        DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(reader);

        if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_ReversedIndexBuffer))
            ReversedDepthOnlyIndexBuffer = new FRawStaticIndexBuffer(reader);

        if (!stripDataFlags.IsEditorDataStripped())
            WireframeIndexBuffer = new FRawStaticIndexBuffer(reader);

        if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
            AdjacencyIndexBuffer = new FRawStaticIndexBuffer(reader);

        if (reader["StaticMesh.HasRayTracingGeometry"] && !stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_RayTracingResources))
        {
            var size = reader.ReadInt32();
            var len = reader.ReadInt32();
            var _ =reader.ReadBytes(len); // rayTracingGeometry
        }

        // https://github.com/EpicGames/UnrealEngine/blob/4.27/Engine/Source/Runtime/Engine/Private/StaticMesh.cpp#L547
        var areaWeightedSectionSamplers = new FWeightedRandomSampler[Sections.Length];
        for (var i = 0; i < Sections.Length; i++)
        {
            areaWeightedSectionSamplers[i] = new FWeightedRandomSampler(reader);
        }

        _ = new FWeightedRandomSampler(reader); // areaWeightedSampler
    }
}

