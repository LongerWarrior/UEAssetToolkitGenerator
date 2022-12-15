namespace UAssetAPI.StructTypes.StaticMesh;

public class FStaticMeshUVItem
{
    public readonly FPackedNormal[] Normal; // VertexTangent
    public readonly FMeshUVFloat[] UV; // VertexUV

    public FStaticMeshUVItem(AssetBinaryReader reader, bool useHighPrecisionTangents, int numStaticUVSets, bool useStaticFloatUVs)
    {
        Normal = SerializeTangents(reader, useHighPrecisionTangents);
        UV = SerializeTexcoords(reader, numStaticUVSets, useStaticFloatUVs);
    }

    public FStaticMeshUVItem(FPackedNormal[] normal, FMeshUVFloat[] uv)
    {
        Normal = normal;
        UV = uv;
    }

    public static FPackedNormal[] SerializeTangents(AssetBinaryReader reader, bool useHighPrecisionTangents)
    {
        if (!useHighPrecisionTangents)
            return new [] { new FPackedNormal(reader), new FPackedNormal(0), new FPackedNormal(reader) }; // # TangentX and TangentZ

        return new [] { (FPackedNormal)new FPackedRGBA16N(reader), new FPackedNormal(0), (FPackedNormal)new FPackedRGBA16N(reader) };
    }

    public static FMeshUVFloat[] SerializeTexcoords(AssetBinaryReader reader, int numStaticUVSets, bool useStaticFloatUVs)
    {
        var uv = new FMeshUVFloat[numStaticUVSets];
        if (useStaticFloatUVs)
        {
            for (var i = 0; i < uv.Length; i++)
            {
                uv[i] = new FMeshUVFloat(reader);
            }
        }
        else
        {
            for (var i = 0; i < uv.Length; i++)
            {
                uv[i] = (FMeshUVFloat) new FMeshUVHalf(reader);
            }
        }
        return uv;
    }
}

