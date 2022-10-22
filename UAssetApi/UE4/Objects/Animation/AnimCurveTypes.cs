using UAssetAPI;
using UAssetApi.ExportTypes;

namespace UAssetApi.UE4.Objects.Animation;

public enum EAnimAssetCurveFlags
{
    AACF_NONE = 0,
    AACF_Editable = 0x00000004,
    AACF_Metadata = 0x00000010,
}

public class FAnimCurveBase : NormalExport
{
    // public FSmartName Name;
    public FName Name;
    public int CurveTypeFlags; // Should be editor only

    public FAnimCurveBase()
    {
        // Name = data.GetOrDefault<FSmartName>(nameof(Name));
        // CurveTypeFlags = data.GetOrDefault<int>(nameof(CurveTypeFlags));
        Name = this["Name"] is NamePropertyData name ? name.Value : default;
        CurveTypeFlags = this["CurveTypeFlags"] is IntPropertyData flags ? flags.Value : default;
    }
}

public class FFloatCurve : FAnimCurveBase
{
    public RichCurveKeyPropertyData FloatCurve;

    public FFloatCurve()
    {
        FloatCurve = this["FloatCurve"] is RichCurveKeyPropertyData curve ? curve : default;
    }
}

/*public struct FRawCurveTracks
{
    public FFloatCurve[] FloatCurves;

    public FRawCurveTracks()
    {
        FloatCurves = this["FloatCurves"] is ArrayPropertyData curves ? 
            curves.Value.Select(x => new FFloatCurve()).ToArray() : Array.Empty<FFloatCurve>();
    }
}*/
