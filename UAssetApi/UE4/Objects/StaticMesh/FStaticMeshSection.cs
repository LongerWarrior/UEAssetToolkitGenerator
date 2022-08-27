namespace UAssetAPI.StructTypes.StaticMesh;

public class FStaticMeshSection
{
    public readonly int MaterialIndex;
    public readonly int FirstIndex;
    public readonly int NumTriangles;
    public readonly int MinVertexIndex;
    public readonly int MaxVertexIndex;
    public readonly bool bEnableCollision;
    public readonly bool bCastShadow;
    public readonly bool bForceOpaque;
    public readonly bool bVisibleInRayTracing;

    public FStaticMeshSection(AssetBinaryReader reader)
    {
        MaterialIndex = reader.ReadInt32();
        FirstIndex = reader.ReadInt32();
        NumTriangles = reader.ReadInt32();
        MinVertexIndex = reader.ReadInt32();
        MaxVertexIndex = reader.ReadInt32();
        bEnableCollision = reader.ReadIntBoolean();
        bCastShadow = reader.ReadIntBoolean();
        bForceOpaque = reader.Asset.GetCustomVersion<FRenderingObjectVersion>() >= FRenderingObjectVersion.StaticMeshSectionForceOpaqueField && reader.ReadIntBoolean();
        bVisibleInRayTracing = !reader["StaticMesh.HasVisibleInRayTracing"] || reader.ReadIntBoolean();
    }
}