using System.ComponentModel;

namespace CookedAssetSerializer.NativizedAssets;

public enum ENativizationMethod
{
    [Description("Disable Blueprint nativization (default).")]
    Disabled,

    [Description("Enable nativization for all Blueprint assets.")]
    Inclusive,

    [Description("Enable nativization for selected Blueprint assets only.")]
    Exclusive
}