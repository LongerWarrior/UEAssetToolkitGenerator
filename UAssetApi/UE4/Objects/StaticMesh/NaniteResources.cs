namespace UAssetAPI.StructTypes.StaticMesh;

public class FPackedHierarchyNode
{
    public const int MAX_BVH_NODE_FANOUT_BITS = 3;
    public const int MAX_BVH_NODE_FANOUT = 1 << MAX_BVH_NODE_FANOUT_BITS;

    public FVector4[] LODBounds;
    public FMisc0[] Misc0;
    public FMisc1[] Misc1;
    public FMisc2[] Misc2;

    public FPackedHierarchyNode(AssetBinaryReader reader)
    {

        LODBounds = new FVector4[MAX_BVH_NODE_FANOUT];
        for (int i = 0; i < MAX_BVH_NODE_FANOUT; i++) {
            LODBounds[i] = new FVector4(reader);
        }

        Misc0 = new FMisc0[MAX_BVH_NODE_FANOUT];
        for (int i = 0; i < MAX_BVH_NODE_FANOUT; i++) {
            Misc0[i] = new FMisc0(reader);
        }

        Misc1 = new FMisc1[MAX_BVH_NODE_FANOUT];
        for (int i = 0; i < MAX_BVH_NODE_FANOUT; i++) {
            Misc1[i] = new FMisc1(reader);
        }

        Misc2 = new FMisc2[MAX_BVH_NODE_FANOUT];
        for (int i = 0; i < MAX_BVH_NODE_FANOUT; i++) {
            Misc2[i] = new FMisc2(reader);
        }
    }

    public struct FMisc0
    {
        public FVector BoxBoundsCenter;
        public uint MinLODError_MaxParentLODError;

        public FMisc0(AssetBinaryReader reader) {
            BoxBoundsCenter = reader.ReadVector();
            MinLODError_MaxParentLODError = reader.ReadUInt32();
        }
    }

    public struct FMisc1
    {
        public FVector BoxBoundsExtent;
        public uint ChildStartReference;

        public FMisc1(AssetBinaryReader reader) {
            BoxBoundsExtent = reader.ReadVector();
            ChildStartReference = reader.ReadUInt32();
        }
    }

    public struct FMisc2
    {
        public uint ResourcePageIndex_NumPages_GroupPartSize;

        public FMisc2(AssetBinaryReader reader) {
            ResourcePageIndex_NumPages_GroupPartSize = reader.ReadUInt32();
        }
    }
}

public struct FPageStreamingState
{
    public uint BulkOffset;
    public uint BulkSize;
    public uint PageSize;
    public uint DependenciesStart;
    public uint DependenciesNum;
    public uint Flags;

    public FPageStreamingState(AssetBinaryReader reader) {
        BulkOffset = reader.ReadUInt32(); ;
        BulkSize = reader.ReadUInt32(); ;
        PageSize = reader.ReadUInt32(); ;
        DependenciesStart = reader.ReadUInt32(); ;
        DependenciesNum = reader.ReadUInt32(); ;
        Flags = reader.ReadUInt32(); ;
    }
}

public class FNaniteResources
{
    // Persistent State
    public byte[] RootClusterPage; // Root page is loaded on resource load, so we always have something to draw.
    public FByteBulkData StreamableClusterPages; // Remaining pages are streamed on demand.
    public ushort[] ImposterAtlas;
    public FPackedHierarchyNode[] HierarchyNodes;
    public uint[] HierarchyRootOffsets;
    public FPageStreamingState[] PageStreamingStates;
    public uint[] PageDependencies;
    public int PositionPrecision = 0;
    public uint NumInputTriangles = 0;
    public uint NumInputVertices = 0;
    public ushort NumInputMeshes = 0;
    public ushort NumInputTexCoords = 0;
    public uint ResourceFlags = 0;

    public FNaniteResources(AssetBinaryReader reader)
    {
        var stripFlags = new FStripDataFlags(reader);
        if (!stripFlags.IsDataStrippedForServer())
        {
            ResourceFlags = reader.ReadUInt32();
            var len = reader.ReadInt32();
            RootClusterPage = reader.ReadBytes(len);
            StreamableClusterPages = new FByteBulkData(reader);
            len = reader.ReadInt32();
            PageStreamingStates = new FPageStreamingState[len];
            for (int i = 0; i < len; i++) {
                PageStreamingStates[i] = new FPageStreamingState(reader);
            }

            len = reader.ReadInt32();
            HierarchyNodes = new FPackedHierarchyNode[len];
            for (int i = 0; i < len; i++) {
                HierarchyNodes[i] = new FPackedHierarchyNode(reader);
            }

            len = reader.ReadInt32();
            HierarchyRootOffsets = new uint[len];
            for (int i = 0; i < len; i++) {
                HierarchyRootOffsets[i] = reader.ReadUInt32();
            }

            len = reader.ReadInt32();
            PageDependencies = new uint[len];
            for (int i = 0; i < len; i++) {
                PageDependencies[i] = reader.ReadUInt32();
            }
            len = reader.ReadInt32();
            ImposterAtlas = new ushort[len];
            for (int i = 0; i < len; i++) {
                ImposterAtlas[i] = reader.ReadUInt16();
            }

            PositionPrecision = reader.ReadInt32();
            NumInputTriangles = reader.ReadUInt32();
            NumInputVertices = reader.ReadUInt32();
            NumInputMeshes = reader.ReadUInt16();
            NumInputTexCoords = reader.ReadUInt16();
        }
    }
}