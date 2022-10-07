namespace UAssetAPI;
/**
 * Indicates animation data key format.
 */
public enum AnimationKeyFormat : byte
{
    AKF_ConstantKeyLerp,
    AKF_VariableKeyLerp,
    AKF_PerTrackCompression,
    AKF_MAX
}