using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI.StructTypes.SkeletalMesh;

public enum EClassDataStripFlag : byte
{
    CDSF_AdjacencyData = 1,
    CDSF_MinLodData = 2,
};

public class FStaticLODModel
{
    public FSkelMeshSection[] Sections;
    public FMultisizeIndexContainer? Indices;
    public short[] ActiveBoneIndices;
    public FSkelMeshChunk[] Chunks;
    public int Size;
    public int NumVertices;
    public short[] RequiredBones;
    public FIntBulkData RawPointIndices;
    public int[] MeshToImportVertexMap;
    public int MaxImportVertex;
    public int NumTexCoords;
    public FSkeletalMeshVertexBuffer VertexBufferGPUSkin;
    public FSkeletalMeshVertexColorBuffer ColorVertexBuffer;
    public FMultisizeIndexContainer AdjacencyIndexBuffer;
    public FSkeletalMeshVertexClothBuffer ClothVertexBuffer;
    public bool SkipLod => Indices == null || Indices.Indices16.Length < 1 && Indices.Indices32.Length < 1;

    public FStaticLODModel()
    {
        Chunks = Array.Empty<FSkelMeshChunk>();
        MeshToImportVertexMap = Array.Empty<int>();
        ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer();
    }

    public FStaticLODModel(AssetBinaryReader reader, bool bHasVertexColors) : this()
    {
        var stripDataFlags =  new FStripDataFlags(reader);
        var skelMeshVer = reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>();

        Sections = reader.ReadArray(() => new FSkelMeshSection(reader));

        if (skelMeshVer < FSkeletalMeshCustomVersion.SplitModelAndRenderData)
        {
            Indices = new FMultisizeIndexContainer(reader);
        }
        else
        {
            // UE4.19+ uses 32-bit index buffer (for editor data)
            Indices = new FMultisizeIndexContainer {Indices32 = reader.ReadBulkArray(() => reader.ReadUInt32())};
        }

        ActiveBoneIndices = reader.ReadArray(() => reader.ReadInt16()); 

        if (skelMeshVer < FSkeletalMeshCustomVersion.CombineSectionWithChunk)
        {
            Chunks = reader.ReadArray(() => new FSkelMeshChunk(reader));
        }

        Size = reader.ReadInt32();
        if (!stripDataFlags.IsDataStrippedForServer())
            NumVertices = reader.ReadInt32();

        RequiredBones = reader.ReadArray(() => reader.ReadInt16());
        if (!stripDataFlags.IsEditorDataStripped())
            RawPointIndices = new FIntBulkData(reader);

        if (reader.Ver >= UE4Version.ADD_SKELMESH_MESHTOIMPORTVERTEXMAP)
        {
            MeshToImportVertexMap = reader.ReadArray(() => reader.ReadInt32());
            MaxImportVertex = reader.ReadInt32();
        }

        if (!stripDataFlags.IsDataStrippedForServer())
        {
            NumTexCoords = reader.ReadInt32();
            if (skelMeshVer < FSkeletalMeshCustomVersion.SplitModelAndRenderData)
            {
                VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer(reader);
                if (skelMeshVer >= FSkeletalMeshCustomVersion.UseSeparateSkinWeightBuffer)
                {
                    var skinWeights = new FSkinWeightVertexBuffer(reader, VertexBufferGPUSkin.bExtraBoneInfluences);
                    if (skinWeights.Weights.Length > 0)
                    {
                        // Copy data to VertexBufferGPUSkin
                        if (VertexBufferGPUSkin.bUseFullPrecisionUVs)
                        {
                            for (var i = 0; i < NumVertices; i++)
                            {
                                VertexBufferGPUSkin.VertsFloat[i].Infs = skinWeights.Weights[i];
                            }
                        }
                        else
                        {
                            for (var i = 0; i < NumVertices; i++)
                            {
                                VertexBufferGPUSkin.VertsHalf[i].Infs = skinWeights.Weights[i];
                            }
                        }
                    }
                }

                if (bHasVertexColors)
                {
                    if (skelMeshVer < FSkeletalMeshCustomVersion.UseSharedColorBufferFormat)
                    {
                        ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(reader);
                    }
                    else
                    {
                        var newColorVertexBuffer = new FColorVertexBuffer(reader);
                        ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(newColorVertexBuffer.Data);
                    }
                }

                if (reader.Ver < UE4Version.REMOVE_EXTRA_SKELMESH_VERTEX_INFLUENCES)
                    throw new Exception("Unsupported: extra SkelMesh vertex influences (old mesh format)");


                if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
                    AdjacencyIndexBuffer = new FMultisizeIndexContainer(reader);

                if (reader.Ver >=  UE4Version.VER_UE4_APEX_CLOTH && HasClothData())
                    ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(reader);
            }
        }

    }

    // UE ref https://github.com/EpicGames/UnrealEngine/blob/26450a5a59ef65d212cf9ce525615c8bd673f42a/Engine/Source/Runtime/Engine/Private/SkeletalMeshLODRenderData.cpp#L710
    public void SerializeRenderItem(AssetBinaryReader reader, bool bHasVertexColors, byte numVertexColorChannels)
    {
        var stripDataFlags = new FStripDataFlags(reader);
        var bIsLODCookedOut = reader.ReadIntBoolean();
        var bInlined = reader.ReadIntBoolean();

        RequiredBones = reader.ReadArray(() => reader.ReadInt16());
        if (!stripDataFlags.IsDataStrippedForServer() && !bIsLODCookedOut)
        {
            Sections = new FSkelMeshSection[reader.ReadInt32()];
            for (var i = 0; i < Sections.Length; i++)
            {
                Sections[i] = new FSkelMeshSection();
                Sections[i].SerializeRenderItem(reader);
            }

            ActiveBoneIndices = reader.ReadArray(() => reader.ReadInt16());

            reader.BaseStream.Position += 4; //var buffersSize = Ar.Read<uint>();

            if (bInlined)
            {
                SerializeStreamedData(reader, bHasVertexColors);
            }
            else
            {
                var bulk = new FByteBulkData(reader);
                if (bulk.Header.ElementCount > 0)
                {

                    using (var tempAr = new AssetBinaryReader(new MemoryStream(bulk.Data), reader.Asset))
                    {
                        SerializeStreamedData(tempAr, bHasVertexColors);
                    }

                    var skipBytes = 5;
                    if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
                        skipBytes += 5;
                    skipBytes += 4 * 4 + 2 * 4 + 2 * 4;
                    skipBytes += FSkinWeightVertexBuffer.MetadataSize(reader);
                    reader.BaseStream.Position += skipBytes;

                    if (HasClothData())
                    {
                        var clothIndexMapping = reader.ReadArray(() => reader.ReadInt64());
                        reader.BaseStream.Position += 2 * 4;
                    }

                    var profileNames = reader.ReadArray(() => reader.ReadFName());
                }
            }
        }

    }

    public void SerializeRenderItem_Legacy(AssetBinaryReader reader, bool bHasVertexColors, byte numVertexColorChannels)
    {
        var stripDataFlags = new FStripDataFlags(reader);

        Sections = new FSkelMeshSection[reader.ReadInt32()];
        for (var i = 0; i < Sections.Length; i++)
        {
            Sections[i] = new FSkelMeshSection();
            Sections[i].SerializeRenderItem(reader);
        }

        Indices = new FMultisizeIndexContainer(reader);
        VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer {bUseFullPrecisionUVs = true};

        ActiveBoneIndices = reader.ReadArray(() => reader.ReadInt16());
        RequiredBones = reader.ReadArray(() => reader.ReadInt16());

        if (!stripDataFlags.IsDataStrippedForServer() && !stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_MinLodData))
        {
            var positionVertexBuffer = new FPositionVertexBuffer(reader);
            var staticMeshVertexBuffer = new FStaticMeshVertexBuffer(reader);
            var skinWeightVertexBuffer = new FSkinWeightVertexBuffer(reader, VertexBufferGPUSkin.bExtraBoneInfluences);

            if (bHasVertexColors)
            {
                var newColorVertexBuffer = new FColorVertexBuffer(reader);
                ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(newColorVertexBuffer.Data);
            }

            if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
                AdjacencyIndexBuffer = new FMultisizeIndexContainer(reader);

            if (HasClothData())
                ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(reader);

            NumVertices = positionVertexBuffer.NumVertices;
            NumTexCoords = staticMeshVertexBuffer.NumTexCoords;

            VertexBufferGPUSkin.VertsFloat = new FGPUVertFloat[NumVertices];
            for (var i = 0; i < VertexBufferGPUSkin.VertsFloat.Length; i++)
            {
                VertexBufferGPUSkin.VertsFloat[i] = new FGPUVertFloat
                {
                    Pos = positionVertexBuffer.Verts[i],
                    Infs = skinWeightVertexBuffer.Weights[i],
                    Normal = staticMeshVertexBuffer.UV[i].Normal,
                    UV = staticMeshVertexBuffer.UV[i].UV
                };
            }
        }

        if (reader.Ver >=  UE4Version.VER_UE4_23)
        {
            var skinWeightProfilesData = new FSkinWeightProfilesData(reader);
        }
    }

    private void SerializeStreamedData(AssetBinaryReader reader, bool bHasVertexColors)
    {
        var stripDataFlags = new FStripDataFlags(reader);

        Indices = new FMultisizeIndexContainer(reader);
        VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer {bUseFullPrecisionUVs = true};

        var positionVertexBuffer = new FPositionVertexBuffer(reader);
        var staticMeshVertexBuffer = new FStaticMeshVertexBuffer(reader);
        var skinWeightVertexBuffer = new FSkinWeightVertexBuffer(reader, VertexBufferGPUSkin.bExtraBoneInfluences);

        if (bHasVertexColors)
        {
            var newColorVertexBuffer = new FColorVertexBuffer(reader);
            ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(newColorVertexBuffer.Data);
        }

        if (!stripDataFlags.IsClassDataStripped((byte) EClassDataStripFlag.CDSF_AdjacencyData))
            AdjacencyIndexBuffer = new FMultisizeIndexContainer(reader);

        if (HasClothData())
            ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(reader);

        var skinWeightProfilesData = new FSkinWeightProfilesData(reader);

        //if (Ar.Game is >= EGame.GAME_UE5_0 or EGame.GAME_UE4_25_Plus) // Note: This was added in UE4.27, but we're only reading it on UE5 for compatibility with Fortnite
        if (reader["SkeletalMesh.HasRayTracingData"])
        {
            var rayTracingData = reader.ReadArray(() => reader.ReadByte());
        }

        NumVertices = positionVertexBuffer.NumVertices;
        NumTexCoords = staticMeshVertexBuffer.NumTexCoords;

        VertexBufferGPUSkin.VertsFloat = new FGPUVertFloat[NumVertices];
        for (var i = 0; i < VertexBufferGPUSkin.VertsFloat.Length; i++)
        {
            VertexBufferGPUSkin.VertsFloat[i] = new FGPUVertFloat
            {
                Pos = positionVertexBuffer.Verts[i],
                Infs = skinWeightVertexBuffer.Weights[i],
                Normal = staticMeshVertexBuffer.UV[i].Normal,
                UV = staticMeshVertexBuffer.UV[i].UV
            };
        }
    }

    private bool HasClothData()
    {
        for (var i = 0; i < Chunks.Length; i++)
            if (Chunks[i].HasClothData) // pre-UE4.13 code
                return true;
        for (var i = 0; i < Sections.Length; i++) // UE4.13+
            if (Sections[i].HasClothData)
                return true;
        return false;
    }
}