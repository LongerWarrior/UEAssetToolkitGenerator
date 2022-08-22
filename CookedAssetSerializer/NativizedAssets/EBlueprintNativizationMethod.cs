using System.ComponentModel;

namespace NativizedAssets;

public enum EBlueprintNativizationMethod
{

    [Description("Disable Blueprint nativization (default).")]
    Disabled,

    [Description("Enable nativization for all Blueprint assets.")]
    Inclusive,

    [Description("Enable nativization for selected Blueprint assets only.")]
    Exclusive
}