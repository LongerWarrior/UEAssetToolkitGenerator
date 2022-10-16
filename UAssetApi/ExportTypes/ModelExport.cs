namespace UAssetAPI;

public readonly struct FVert : IUStruct
{
    /** Index of a Vertex */
    public readonly int pVertex;

    /** If shared, index of unique side. Otherwise INDEX_NONE. */
    public readonly int iSide;

    /** The vertex's shadow map coordinate. */
    public readonly FVector2D ShadowTexCoord;

    /** The vertex's shadow map coordinate for the backface of the node. */
    public readonly FVector2D BackfaceShadowTexCoord;

    public FVert(AssetBinaryReader reader)
    {
        pVertex = reader.ReadInt32();
        iSide = reader.ReadInt32();
        ShadowTexCoord = new FVector2D(reader);
        BackfaceShadowTexCoord = new FVector2D(reader);
    }
}

/** Flags associated with a Bsp node. */
public enum EBspNodeFlags : byte
{
    // Flags.
    NF_NotCsg = 0x01, // Node is not a Csg splitter, i.e. is a transparent poly.
    NF_NotVisBlocking = 0x04, // Node does not block visibility, i.e. is an invisible collision hull.
    NF_BrightCorners = 0x10, // Temporary.
    NF_IsNew = 0x20, // Editor: Node was newly-added.
    NF_IsFront = 0x40, // Filter operation bounding-sphere precomputed and guaranteed to be front.
    NF_IsBack = 0x80 // Guaranteed back.
}

/**
 * FBspNode defines one node in the Bsp, including the front and back
 * pointers and the polygon data itself.  A node may have 0 or 3 to (MAX_NODE_VERTICES-1)
 * vertices. If the node has zero vertices, it's only used for splitting and
 * doesn't contain a polygon (this happens in the editor).
 *
 * vNormal, vTextureU, vTextureV, and others are indices into the level's
 * vector table.  iFront,iBack should be INDEX_NONE to indicate no children.
 *
 * If iPlane==INDEX_NONE, a node has no coplanars.  Otherwise iPlane
 * is an index to a coplanar polygon in the Bsp.  All polygons that are iPlane
 * children can only have iPlane children themselves, not fronts or backs.
 */
public readonly struct FBspNode : IUStruct
{
    public const int MAX_NODE_VERTICES = 255;
    public const int MAX_ZONES = 64;

    // Persistent information.
    public readonly FPlane Plane; // 16 Plane the node falls into (X, Y, Z, W).

    public readonly int
        iVertPool; // 4  Index of first vertex in vertex pool, =iTerrain if NumVertices==0 and NF_TerrainFront.

    public readonly int iSurf; // 4  Index to surface information.

    /** The index of the node's first vertex in the UModel's vertex buffer. */
    public readonly int iVertexIndex;

    /** The index in ULevel::ModelComponents of the UModelComponent containing this node. */
    public readonly ushort ComponentIndex;

    /** The index of the node in the UModelComponent's Nodes array. */
    public readonly ushort ComponentNodeIndex;

    /** The index of the element in the UModelComponent's Element array. */
    public readonly int ComponentElementIndex;

    // iBack:  4  Index to node in front (in direction of Normal).
    // iFront: 4  Index to node in back  (opposite direction as Normal).
    // iPlane: 4  Index to next coplanar poly in coplanar list.
    public readonly int iBack;
    public readonly int iFront;
    public readonly int iPlane;

    /** 4  Collision bound. */
    public readonly int iCollisionBound;

    /** 2 Visibility zone in 1=front, 0=back. */
    public readonly byte iZone0;

    public readonly byte iZone1;

    /**1  Number of vertices in node.*/
    public readonly byte NumVertices;

    /** 1  Node flags. */
    public readonly EBspNodeFlags NodeFlags;

    /**4  Leaf in back and front, INDEX_NONE=not a leaf.*/
    public readonly int iLeaf0;

    public readonly int iLeaf1;

    public FBspNode(AssetBinaryReader reader)
    {
        Plane = new FPlane(reader);
        iVertPool = reader.ReadInt32();
        iSurf = reader.ReadInt32();
        iVertexIndex = reader.ReadInt32();
        ComponentIndex = reader.ReadUInt16();
        ComponentNodeIndex = reader.ReadUInt16();
        ComponentElementIndex = reader.ReadInt32();
        iBack = reader.ReadInt32();
        iFront = reader.ReadInt32();
        iPlane = reader.ReadInt32();
        iCollisionBound = reader.ReadInt32();
        iZone0 = reader.ReadByte();
        iZone1 = reader.ReadByte();
        NumVertices = reader.ReadByte();
        NodeFlags = (EBspNodeFlags)reader.ReadByte();
        iLeaf0 = reader.ReadInt32();
        iLeaf1 = reader.ReadInt32();
    }
}

public readonly struct FZoneSet : IUStruct
{
    public readonly ulong MaskBits;

    public FZoneSet(AssetBinaryReader reader)
    {
        MaskBits = reader.ReadUInt64();
    }
}

public readonly struct FZoneProperties : IUStruct
{
    public readonly FPackageIndex ZoneActor;
    public readonly float LastRenderTime;
    public readonly FZoneSet Connectivity;
    public readonly FZoneSet Visibility;

    public FZoneProperties(AssetBinaryReader reader)
    {
        ZoneActor = new FPackageIndex(reader);
        Connectivity = new FZoneSet(reader);
        Visibility = new FZoneSet(reader);
        LastRenderTime = reader.ReadSingle();
    }
}

/**
 * One Bsp polygon.  Lists all of the properties associated with the
 * polygon's plane.  Does not include a point list; the actual points
 * are stored along with Bsp nodes, since several nodes which lie in the
 * same plane may reference the same poly.
 */
public readonly struct FBspSurf : IUStruct
{
    public readonly FPackageIndex Material; // UMaterialInterface
    public readonly uint PolyFlags; // Polygon flags.
    public readonly int pBase; // Polygon & texture base point index (where U,V==0,0).
    public readonly int vNormal; // Index to polygon normal.
    public readonly int vTextureU; // Texture U-vector index.
    public readonly int vTextureV; // Texture V-vector index.
    public readonly int iBrushPoly; // Editor brush polygon index.
    public readonly FPackageIndex Actor; // ABrush Brush actor owning this Bsp surface.
    public readonly FPlane Plane; // The plane this surface lies on.
    public readonly float LightMapScale; // The number of units/lightmap texel on this surface.
    public readonly int iLightmassIndex; // Index to the lightmass settings

    public FBspSurf(AssetBinaryReader reader)
    {
        Material = new FPackageIndex(reader);
        PolyFlags = reader.ReadUInt32();
        pBase = reader.ReadInt32();
        vNormal = reader.ReadInt32();
        vTextureU = reader.ReadInt32();
        vTextureV = reader.ReadInt32();
        ;
        iBrushPoly = reader.ReadInt32();
        Actor = new FPackageIndex(reader);
        Plane = new FPlane(reader);
        LightMapScale = reader.ReadSingle();
        iLightmassIndex = reader.ReadInt32();
    }
}

public struct FModelVertex : IUStruct
{
    public FVector Position;
    public FVector TangentX;
    public FVector4 TangentZ;
    public FVector2D TexCoord;
    public FVector2D ShadowTexCoord;

    public FModelVertex(AssetBinaryReader reader)
    {
        Position = reader.ReadVector();

        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() < FRenderingObjectVersion.IncreaseNormalPrecision)
        {
            TangentX = (FVector)new FDeprecatedSerializedPackedNormal(reader);
            TangentZ = (FVector4)new FDeprecatedSerializedPackedNormal(reader);
        }
        else
        {
            TangentX = reader.ReadVector();
            TangentZ = new FVector4(reader);
        }

        TexCoord = new FVector2D(reader);
        ShadowTexCoord = new FVector2D(reader);
    }

    public FVector GetTangentY()
    {
        return ((FVector)TangentZ ^ TangentX) * TangentZ.W;
    }
}

public struct FDeprecatedModelVertex : IUStruct
{
    public FVector Position;
    public FDeprecatedSerializedPackedNormal TangentX;
    public FDeprecatedSerializedPackedNormal TangentZ;
    public FVector2D TexCoord;
    public FVector2D ShadowTexCoord;

    public static implicit operator FModelVertex(FDeprecatedModelVertex v)
    {
        return new()
        {
            Position = v.Position,
            TangentX = (FVector)v.TangentX,
            TangentZ = (FVector4)v.TangentZ,
            TexCoord = v.TexCoord,
            ShadowTexCoord = v.ShadowTexCoord
        };
    }

    public FDeprecatedModelVertex(AssetBinaryReader reader)
    {
        Position = reader.ReadVector();
        TangentX = new FDeprecatedSerializedPackedNormal(reader);
        TangentZ = new FDeprecatedSerializedPackedNormal(reader);
        TexCoord = new FVector2D(reader);
        ShadowTexCoord = new FVector2D(reader);
    }
}

/** A vertex buffer for a set of BSP nodes. */
public class FModelVertexBuffer : IUStruct
{
    public readonly FModelVertex[] Vertices;

    public FModelVertexBuffer(AssetBinaryReader reader)
    {
        if (reader.Asset.GetCustomVersion<FRenderingObjectVersion>() < FRenderingObjectVersion.IncreaseNormalPrecision)
        {
            var deprecatedVertices = reader.ReadBulkArray(() => new FDeprecatedModelVertex(reader));
            Vertices = new FModelVertex[deprecatedVertices.Length];
            for (var i = 0; i < Vertices.Length; i++) Vertices[i] = deprecatedVertices[i];
        }
        else
        {
            Vertices = reader.ReadArray(() => new FModelVertex(reader));
        }
    }
}

public readonly struct FLightmassPrimitiveSettings : IUStruct
{
    public readonly bool bUseTwoSidedLighting;
    public readonly bool bShadowIndirectOnly;
    public readonly bool bUseEmissiveForStaticLighting;
    public readonly bool bUseVertexNormalForHemisphereGather;
    public readonly float EmissiveLightFalloffExponent;
    public readonly float EmissiveLightExplicitInfluenceRadius;
    public readonly float EmissiveBoost;
    public readonly float DiffuseBoost;
    public readonly float FullyOccludedSamplesFraction;

    public FLightmassPrimitiveSettings(AssetBinaryReader reader)
    {
        bUseTwoSidedLighting = reader.ReadIntBoolean();
        bShadowIndirectOnly = reader.ReadIntBoolean();
        FullyOccludedSamplesFraction = reader.ReadSingle();
        bUseEmissiveForStaticLighting = reader.ReadIntBoolean();
        bUseVertexNormalForHemisphereGather = reader.Ver >= UE4Version.VER_UE4_NEW_LIGHTMASS_PRIMITIVE_SETTING &&
                                              reader.ReadIntBoolean();
        EmissiveLightFalloffExponent = reader.ReadSingle();
        EmissiveLightExplicitInfluenceRadius = reader.ReadSingle();
        EmissiveBoost = reader.ReadSingle();
        DiffuseBoost = reader.ReadSingle();
    }
}

public class ModelExport : NormalExport
{
    public FBoxSphereBounds Bounds;
    public FVector[] Vectors;
    public FVector[] Points;
    public FBspNode[] Nodes;
    public FBspSurf[] Surfs;
    public FVert[] Verts;
    public int NumSharedSides;
    public bool RootOutside;
    public bool Linked;
    public uint NumUniqueVertices;
    public FModelVertexBuffer VertexBuffer;
    public Guid LightingGuid;
    public FLightmassPrimitiveSettings[] LightmassSettings;

    public ModelExport(Export super) : base(super)
    {
    }

    public ModelExport()
    {
    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        const int StripVertexBufferFlag = 1;
        var stripData = new FStripDataFlags(reader);

        Bounds = new FBoxSphereBounds(reader);

        Vectors = reader.ReadBulkArray(() => new FVector(reader));
        Points = reader.ReadBulkArray(() => new FVector(reader));
        Nodes = reader.ReadBulkArray(() => new FBspNode(reader));

        if (reader.Ver < UE4Version.VER_UE4_BSP_UNDO_FIX)
        {
            var surfsOwner = new FPackageIndex(reader);
            Surfs = reader.ReadArray(() => new FBspSurf(reader));
        }
        else
        {
            Surfs = reader.ReadArray(() => new FBspSurf(reader));
        }

        Verts = reader.ReadBulkArray(() => new FVert(reader));

        NumSharedSides = reader.ReadInt32();
        if (reader.Ver < UE4Version.VER_UE4_REMOVE_ZONES_FROM_MODEL)
        {
            var dummyZones = reader.ReadArray(() => new FZoneProperties(reader));
        }

        var bHasEditorOnlyData = !reader.Asset.PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly) ||
                                 reader.Ver < UE4Version.VER_UE4_REMOVE_UNUSED_UPOLYS_FROM_UMODEL;
        if (bHasEditorOnlyData)
        {
            var dummyPolys = new FPackageIndex(reader);
            reader.SkipBulkArrayData(); // DummyLeafHulls
            reader.SkipBulkArrayData(); // DummyLeaves
        }

        RootOutside = reader.ReadIntBoolean();
        Linked = reader.ReadIntBoolean();

        if (reader.Ver < UE4Version.VER_UE4_REMOVE_ZONES_FROM_MODEL)
        {
            var dummyPortalNodes = reader.ReadBulkArray(() => reader.ReadInt32());
        }

        NumUniqueVertices = reader.ReadUInt32();

        if (!stripData.IsEditorDataStripped() || !stripData.IsClassDataStripped(StripVertexBufferFlag))
            VertexBuffer = new FModelVertexBuffer(reader);

        LightingGuid = new Guid(reader.ReadBytes(16));
        LightmassSettings = reader.ReadArray(() => new FLightmassPrimitiveSettings(reader));
    }

    public override void Write(AssetBinaryWriter writer)
    {
        base.Write(writer);

        writer.Write((int)0);
    }
}