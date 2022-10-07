namespace UAssetAPI;

public enum EAnimAssetCurveFlags
{
    AACF_NONE = 0,
    AACF_Editable = 0x00000004,
    AACF_Metadata = 0x00000010,
}

public class FAnimCurveBase
{
    public SmartNamePropertyData Name;
    public int CurveTypeFlags; // Should be editor only

    public FAnimCurveBase() { }

    public FAnimCurveBase(FStructFallback data)
    {
        Name = data.GetOrDefault<FSmartName>(nameof(Name));
        CurveTypeFlags = data.GetOrDefault<int>(nameof(CurveTypeFlags));
    }
    
    public override void Read(AssetBinaryReader reader, int nextStarting)
    {
        base.Read(reader, nextStarting);
        
        // Skeleton = GetOrDefault<FPackageIndex>(nameof(Skeleton));
        Skeleton = reader.XFER_OBJECT_POINTER();
        
        if (reader.Ver >= UE4Version.VER_UE4_SKELETON_GUID_SERIALIZATION)
        {
            SkeletonGuid = new Guid(reader.ReadBytes(16));
        }
    }
}

public class FFloatCurve : FAnimCurveBase
{
    public RichCurveKeyPropertyData FloatCurve;

    public FFloatCurve() { }

    public FFloatCurve(FStructFallback data) : base(data)
    {
        FloatCurve = data.GetOrDefault<FRichCurve>(nameof(FloatCurve));
    }
}

public struct FRawCurveTracks
{
    public FFloatCurve[] FloatCurves;

    public FRawCurveTracks(FStructFallback data)
    {
        FloatCurves = data.GetOrDefault<FFloatCurve[]>(nameof(FloatCurves));
    }
}
