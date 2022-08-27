namespace UAssetAPI.StructTypes.StaticMesh;

public class FStaticMeshRenderData
{
    private const int MAX_STATIC_UV_SETS_UE4 = 8;
    private const int MAX_STATIC_LODS_UE4 = 8;

    public readonly FStaticMeshLODResources[] LODs;
    public readonly FNaniteResources? NaniteResources;
    public readonly FBoxSphereBounds Bounds;
    public readonly bool bLODsShareStaticLighting;
    public readonly float[]? ScreenSize;

    public FStaticMeshRenderData(AssetBinaryReader reader, bool bCooked)
    {
        if (!bCooked) return;

        // TODO Read minMobileLODIdx only when platform is desktop and CVar r.StaticMesh.KeepMobileMinLODSettingOnDesktop is nonzero
        if (reader["StaticMesh.KeepMobileMinLODSettingOnDesktop"])
        {
            var minMobileLODIdx = reader.ReadInt32();
        }

        var len = reader.ReadInt32();
        LODs = new FStaticMeshLODResources[len];
        for (int i = 0; i < len; i++) {
            LODs[i] = new FStaticMeshLODResources(reader);
        }

        if (reader.Ver >= UE4Version.VER_UE4_23)
        {
            var numInlinedLODs = reader.ReadByte();
        }

        if (reader.Ver >= UE4Version.VER_UE4_RENAME_CROUCHMOVESCHARACTERDOWN)
        {
            var stripped = false;
            if (reader.Ver >= UE4Version.VER_UE4_RENAME_WIDGET_VISIBILITY)
            {
                var stripDataFlags = new FStripDataFlags(reader);
                stripped = stripDataFlags.IsDataStrippedForServer();
                if (reader.Ver >= UE4Version.VER_UE4_21)
                {
                    stripped |= stripDataFlags.IsClassDataStripped(0x01);
                }
            }

            if (!stripped)
            {
                for (var i = 0; i < LODs.Length; i++)
                {
                    var bValid = reader.ReadIntBoolean();
                    if (bValid)
                    {
                         var _ = new FDistanceFieldVolumeData(reader);
                    }
                }
            }
        }

        Bounds = new FBoxSphereBounds(reader.ReadVector(), reader.ReadVector(), reader.ReadSingle());

        if (reader["StaticMesh.HasLODsShareStaticLighting"])
            bLODsShareStaticLighting = reader.ReadIntBoolean();

        if (reader.Ver < UE4Version.VER_UE4_14)
        {
            var bReducedBySimplygon = reader.ReadIntBoolean();
        }

        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() < FRenderingObjectVersion.TextureStreamingMeshUVChannelData)
        {
            reader.BaseStream.Position += 4 * MAX_STATIC_UV_SETS_UE4; // StreamingTextureFactor for each UV set
            reader.BaseStream.Position += 4; // MaxStreamingTextureFactor
        }

        ScreenSize = new float[reader.Ver >= UE4Version.VER_UE4_9 ? MAX_STATIC_LODS_UE4 : 4];
        for (var i = 0; i < ScreenSize.Length; i++)
        {
            if (reader.Ver >= UE4Version.VER_UE4_20) // FPerPlatformProperty
            {
                var bFloatCooked = reader.ReadIntBoolean();
            }

            ScreenSize[i] = reader.ReadSingle();
        }

    }

    private void SerializeInlineDataRepresentations(AssetBinaryReader reader)
    {
        // Defined class flags for possible stripping
        const byte CardRepresentationDataStripFlag = 2;

        var stripFlags = new FStripDataFlags(reader);
        if (!stripFlags.IsDataStrippedForServer() && !stripFlags.IsClassDataStripped(CardRepresentationDataStripFlag))
        {
            foreach (var lod in LODs)
            {
                var bValid = reader.ReadIntBoolean();
                if (bValid)
                {
                    lod.CardRepresentationData = new FCardRepresentationData(reader);
                }
            }
        }
    }
}