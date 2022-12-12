namespace UAssetAPI;

public class FRawAnimSequenceTrack
{
    public FVector[] PosKeys;
    public FQuat[] RotKeys;
    public FVector[] ScaleKeys;

    public void Read(AssetBinaryReader reader) {
        PosKeys = reader.ReadBulkArray(() => new FVector(reader));
        RotKeys = reader.ReadBulkArray(() => new FQuat());

        if (reader.Ver >= UE4Version.VER_UE4_ANIM_SUPPORT_NONUNIFORM_SCALE_ANIMATION)
        {
            ScaleKeys = reader.ReadBulkArray(() => new FVector(reader));
        }
    }
}