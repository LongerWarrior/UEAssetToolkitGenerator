using UAssetAPI.StructTypes.StaticMesh;

namespace UAssetAPI.StructTypes;

public class FSkeletalMeshAreaWeightedTriangleSampler : FWeightedRandomSampler {
    public FSkeletalMeshAreaWeightedTriangleSampler(AssetBinaryReader reader) : base(reader) {
    }
}


public class FSkeletalMeshSamplingRegionBuiltData {
    /** Triangles included in this region. */
    int[] TriangleIndices;

    /** Vertices included in this region. */
    int[] Vertices;

    /** Bones included in this region. */
    int[] BoneIndices;

    /** Provides random area weighted sampling of the TriangleIndices array. */
    FSkeletalMeshAreaWeightedTriangleSampler AreaWeightedSampler;

    public FSkeletalMeshSamplingRegionBuiltData(AssetBinaryReader reader) {
        TriangleIndices = reader.ReadArray(() => reader.ReadInt32());
        BoneIndices = reader.ReadArray(() => reader.ReadInt32());
        AreaWeightedSampler = new FSkeletalMeshAreaWeightedTriangleSampler(reader);

        if (reader.Asset.GetCustomVersion<FNiagaraObjectVersion>() >= FNiagaraObjectVersion.SkeletalMeshVertexSampling) {
            Vertices = reader.ReadArray(() => reader.ReadInt32());
        }
        
    }
}


public class SkeletalMeshSamplingRegionBuiltDataPropertyData : PropertyData<FSkeletalMeshSamplingRegionBuiltData>
{
    public SkeletalMeshSamplingRegionBuiltDataPropertyData(FName name) : base(name)
    {

    }

    public SkeletalMeshSamplingRegionBuiltDataPropertyData()
    {

    }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        Value = new FSkeletalMeshSamplingRegionBuiltData(reader);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        return 0;
    }

    private static readonly FName CurrentPropertyType = new FName("SkeletalMeshSamplingRegionBuiltData");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override string ToString()
    {
        return Value.ToString();
    }
}