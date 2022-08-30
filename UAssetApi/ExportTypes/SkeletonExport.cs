namespace UAssetAPI;

public class SkeletonExport : NormalExport {
    //public FBoneNode[] BoneTree;
    public FReferenceSkeleton ReferenceSkeleton;
    public Guid Guid;
    //public Guid VirtualBoneGuid;
    public Dictionary<FName, FReferencePose> AnimRetargetSources;
    public Dictionary<FName, FSmartNameMapping> NameMappings;
    public FName[] ExistingMarkerNames;

    //static Dictionary<string, byte> BoneEnumToByte = new Dictionary<string, byte>()
    //{
    //    { "Animation", 0 },
    //    { "Skeleton", 1  },
    //    { "AnimationScaled", 2  },
    //    { "AnimationRelative", 3  },
    //    { "OrientAndScale", 4 },
    //};
    public SkeletonExport(Export super) : base(super) {

    }

    public SkeletonExport() {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting) {
        base.Read(reader, nextStarting);

        int idk = reader.ReadInt32();

        ReferenceSkeleton = new FReferenceSkeleton();
        ReferenceSkeleton.Read(reader);

        int numOfRetargetSources = reader.ReadInt32();
        AnimRetargetSources = new Dictionary<FName, FReferencePose>(numOfRetargetSources);
        for (var i = 0; i < numOfRetargetSources; i++) {
            FName name = reader.ReadFName();
            FReferencePose pose = new FReferencePose();
            pose.Read(reader);

            if (pose.ReferencePose != null) ReferenceSkeleton.AdjustBoneScales(pose.ReferencePose);
            AnimRetargetSources[reader.ReadFName()] = pose;
        }

        Guid = new Guid(reader.ReadBytes(16));

        int mapLength = reader.ReadInt32();
        NameMappings = new Dictionary<FName, FSmartNameMapping>(mapLength);
        for (var i = 0; i < mapLength; i++) {
            FName name = reader.ReadFName();
            FSmartNameMapping mapping = new FSmartNameMapping();
            mapping.Read(reader);

            NameMappings[name] = mapping;
        }

        byte GlobalStripFlags = reader.ReadByte();
        byte ClassStripFlags = reader.ReadByte();

        if (!((GlobalStripFlags & 1) != 0)) {
            ExistingMarkerNames = reader.ReadArray(reader.ReadFName);
        }

    }

    public override void Write(AssetBinaryWriter writer) {
        base.Write(writer);

        writer.Write((int)0);

        //writer.Write(CollisionDisableTable.Count);
        //foreach (KeyValuePair<FRigidBodyIndexPair, bool> entry in CollisionDisableTable) {
        //    writer.Write(entry.Key.Indices[0]);
        //    writer.Write(entry.Key.Indices[1]);
        //    writer.Write(entry.Value ? 1 : 0);
        //}
    }
}

