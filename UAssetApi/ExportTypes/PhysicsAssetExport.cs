namespace UAssetAPI;

public struct FRigidBodyIndexPair {
    public int[] Indices;

    public FRigidBodyIndexPair(int index1, int index2) {
        Indices = new int[2] { index1,index2};
    }
}

/// <summary>
/// Export for a physics asset.
/// </summary>
public class PhysicsAssetExport : NormalExport
{
    public Dictionary<FRigidBodyIndexPair,bool> CollisionDisableTable;

    public PhysicsAssetExport(Export super) : base(super)
    {

    }

    //public PhysicsAssetExport(UDataTable data, UAsset asset, byte[] extras) : base(asset, extras)
    //{
    //    Table = data;
    //}

    public PhysicsAssetExport()
    {

    }

    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);

        reader.ReadInt32();

        CollisionDisableTable = new Dictionary<FRigidBodyIndexPair, bool>();

        int numEntries = reader.ReadInt32();
        for (int i = 0; i < numEntries; i++)
        {
            CollisionDisableTable.Add(new FRigidBodyIndexPair(reader.ReadInt32(), reader.ReadInt32()), reader.ReadIntBoolean());
        }
    }

    public override void Write(AssetBinaryWriter writer) {
        base.Write(writer);

        writer.Write((int)0);

        writer.Write(CollisionDisableTable.Count);
        foreach (KeyValuePair<FRigidBodyIndexPair, bool> entry in CollisionDisableTable) {
            writer.Write(entry.Key.Indices[0]);
            writer.Write(entry.Key.Indices[1]);
            writer.Write(entry.Value ? 1 : 0);
        }
    }
}
