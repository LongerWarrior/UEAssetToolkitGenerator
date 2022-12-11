namespace UAssetAPI.StructTypes.StaticMesh; 

public class FStaticMeshVertexBuffer
{
    public readonly int NumTexCoords;
    public readonly int Strides;
    public readonly int NumVertices;
    public readonly bool UseFullPrecisionUVs;
    public readonly bool UseHighPrecisionTangentBasis;
    public readonly FStaticMeshUVItem[] UV;  // StaticMeshVertexBuffer 

    public FStaticMeshVertexBuffer(AssetBinaryReader reader)
    {
        var stripDataFlags = new FStripDataFlags(reader, reader.Ver >= UE4Version.VER_UE4_STATIC_SKELETAL_MESH_SERIALIZATION_FIX);

        // SerializeMetaData
        NumTexCoords = reader.ReadInt32();
        Strides = reader.Ver < UE4Version.VER_UE4_19 ? reader.ReadInt32() : -1;
        NumVertices = reader.ReadInt32();
        UseFullPrecisionUVs = reader.ReadIntBoolean();
        UseHighPrecisionTangentBasis = reader.Ver >= UE4Version.VER_UE4_12 && reader.ReadIntBoolean();

        if (!stripDataFlags.IsDataStrippedForServer())
        {
            if (reader.Ver < UE4Version.VER_UE4_19)
            {
                //UV = Ar.ReadBulkArray(() => new FStaticMeshUVItem(Ar, UseHighPrecisionTangentBasis, NumTexCoords, UseFullPrecisionUVs));
            }
            else
            {
                var tempTangents = Array.Empty<FPackedNormal[]>();
                // BulkSerialize
                var itemSize = reader.ReadInt32();
                var itemCount = reader.ReadInt32();
                var position = reader.BaseStream.Position;

                if (itemCount != NumVertices)
                    throw new Exception($"NumVertices={itemCount} != NumVertices={NumVertices}");

                tempTangents = new FPackedNormal[NumVertices][];
                for (var i = 0; i < NumVertices; i++) {
                    tempTangents[i] = FStaticMeshUVItem.SerializeTangents(reader, UseHighPrecisionTangentBasis);
                }

                if (reader.BaseStream.Position - position != itemCount * itemSize)
                    throw new Exception($"Read incorrect amount of tangent bytes, at {reader.BaseStream.Position}, should be: {position + itemSize * itemCount} behind: {position + (itemSize * itemCount) - reader.BaseStream.Position}");

                itemSize = reader.ReadInt32();
                itemCount = reader.ReadInt32();
                position = reader.BaseStream.Position;

                if (itemCount != NumVertices * NumTexCoords)
                        throw new Exception($"NumVertices={itemCount} != {NumVertices * NumTexCoords}");

                var uv = new FMeshUVFloat[NumVertices][];
                for (var i = 0; i < NumVertices; i++) {
                    uv[i] = FStaticMeshUVItem.SerializeTexcoords(reader, NumTexCoords, UseFullPrecisionUVs);
                }

                if (reader.BaseStream.Position - position != itemCount * itemSize)
                    throw new Exception($"Read incorrect amount of Texture Coordinate bytes, at {reader.BaseStream.Position}, should be: {position + itemSize * itemCount} behind: {position + (itemSize * itemCount) - reader.BaseStream.Position}");

                UV = new FStaticMeshUVItem[NumVertices];
                for (var i = 0; i < NumVertices; i++)
                {
                    UV[i] = new FStaticMeshUVItem(tempTangents[i], uv[i]);
                }
            }
        }
        else
        {
            UV = Array.Empty<FStaticMeshUVItem>();
        }
    }
}

