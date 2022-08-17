namespace UAssetAPI;

public struct FBoxSphereBounds {
    /** Holds the origin of the bounding box and sphere. */
    public FVector Origin;
    /** Holds the extent of the bounding box. */
    public FVector BoxExtent;
    /** Holds the radius of the bounding sphere. */
    public float SphereRadius;

    public FBoxSphereBounds(FVector origin, FVector boxExtent, float sphereRadius) {
        Origin = origin;
        BoxExtent = boxExtent;
        SphereRadius = sphereRadius;
    }

    public FBoxSphereBounds(AssetBinaryReader reader) {
        Origin = reader.ReadVector();
        BoxExtent = reader.ReadVector();
        SphereRadius = reader.ReadSingle();
    }
}
