namespace UAssetAPI.StructTypes.SkeletalMesh;

public class FSkelMeshVertexBase
{
    public FVector Pos;
    public FPackedNormal[] Normal;
    public FSkinWeightInfo? Infs;

    public FSkelMeshVertexBase()
    {
        Normal = Array.Empty<FPackedNormal>();
    }

    public void SerializeForGPU(AssetBinaryReader reader, bool bExtraBoneInfluences)
    {
        Normal = new FPackedNormal[3];
        Normal[0] = new FPackedNormal(reader);
        Normal[2] = new FPackedNormal(reader);
        if (reader.Asset.GetCustomVersion<FSkeletalMeshCustomVersion>() < FSkeletalMeshCustomVersion.UseSeparateSkinWeightBuffer)
        {
            // serialized as separate buffer starting with UE4.15
            Infs = new FSkinWeightInfo(reader, bExtraBoneInfluences);
        }
        Pos = reader.ReadVector();
    }

    public void SerializeForEditor(AssetBinaryReader reader)
    {
        Normal = new FPackedNormal[3];
        Pos = reader.ReadVector();
        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() < FRenderingObjectVersion.IncreaseNormalPrecision)
        {
            Normal[0] = new FPackedNormal(reader);
            Normal[1] = new FPackedNormal(reader);
            Normal[2] = new FPackedNormal(reader);
        }
        else
        {
            // New normals are stored with full floating point precision
            Normal[0] = new FPackedNormal(reader.ReadVector());
            Normal[1] = new FPackedNormal(reader.ReadVector());
            Normal[2] = new FPackedNormal(reader.ReadVector());
        }
    }
}