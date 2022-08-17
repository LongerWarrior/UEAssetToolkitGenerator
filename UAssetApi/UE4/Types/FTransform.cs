namespace UAssetAPI;

/// <summary>
/// Transform composed of Scale, Rotation (as a quaternion), and Translation.
/// Transforms can be used to convert from one space to another, for example by transforming
/// positions and directions from local space to world space.
/// 
/// Transformation of position vectors is applied in the order:  Scale -> Rotate -> Translate.
/// Transformation of direction vectors is applied in the order: Scale -> Rotate.
/// 
/// Order matters when composing transforms: C = A * B will yield a transform C that logically
/// first applies A then B to any subsequent transformation. Note that this is the opposite order of quaternion (FQuat) multiplication.
/// 
/// Example: LocalToWorld = (DeltaRotation * LocalToWorld) will change rotation in local space by DeltaRotation.
/// Example: LocalToWorld = (LocalToWorld * DeltaRotation) will change rotation in world space by DeltaRotation.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public struct FTransform
{
    /// <summary>
    /// Rotation of this transformation, as a quaternion
    /// </summary>
    [JsonProperty]
    public FQuat Rotation;

    /// <summary>
    /// Translation of this transformation, as a vector.
    /// </summary>
    [JsonProperty]
    public FVector Translation;

    /// <summary>
    /// 3D scale (always applied in local space) as a vector.
    /// </summary>
    [JsonProperty]
    public FVector Scale3D;

    public FTransform(FQuat rotation, FVector translation, FVector scale3D)
    {
        Rotation = rotation;
        Translation = translation;
        Scale3D = scale3D;
    }

    public JToken ToJson() {

        throw new NotImplementedException();
    }

    public string ToString() {
        string res = "";
        FRotator Rotator = Rotation.ToRotator();
        res = Translation.X.ToString("F6") + ";"+Translation.Y.ToString("F6") + ";"+Translation.Z.ToString("F6") + "|";
        res += Rotator.Pitch.ToString("F6") + ";"+ Rotator.Yaw.ToString("F6") + ";"+ Rotator.Roll.ToString("F6") + "|";
        res += Scale3D.X.ToString("F6") + ";" + Scale3D.Y.ToString("F6") + ";" + Scale3D.Z.ToString("F6");
        return res.Replace(",",".").Replace(";",",");
    }
}
