namespace UAssetAPI.StructTypes.StaticMesh;

public struct FMeshUVHalf : IUStruct {
    public readonly ushort U;
    public readonly ushort V;

    public FMeshUVHalf(ushort u, ushort v) {
        U = u;
        V = v;
    }
    public FMeshUVHalf(AssetBinaryReader reader) {
        U = reader.ReadUInt16();
        V = reader.ReadUInt16();
    }
}

