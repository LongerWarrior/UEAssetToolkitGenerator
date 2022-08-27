namespace UAssetAPI;

/// <summary>
/// Floating point quaternion that can represent a rotation about an axis in 3-D space.
/// The X, Y, Z, W components also double as the Axis/Angle format.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class FQuat
{
    /// <summary>The quaternion's X-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float X;

    /// <summary>The quaternion's Y-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Y;

    /// <summary>The quaternion's Z-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Z;

    /// <summary>The quaternion's W-component.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float W;

    public FQuat(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public FQuat()
    {

    }

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("X", X));
        res.Add(new JProperty("Y", Y));
        res.Add(new JProperty("Z", Z));
        res.Add(new JProperty("W", W));
        return res;
    }

    public FRotator ToRotator() {

        var singularityTest = Z * X - W * Y;
        var yawY = 2.0f * (W * Z + X * Y);
        var yawX = (1.0f - 2.0f * (Y * Y + Z * Z));

        // reference
        // http://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
        // http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/

        // this value was found from experience, the above websites recommend different values
        // but that isn't the case for us, so I went through different testing, and finally found the case
        // where both of world lives happily.
        const float SINGULARITY_THRESHOLD = 0.4999995f;
        const double RAD_TO_DEG = 180.0f / Math.PI;
        var rotatorFromQuat = new FRotator();

        if (singularityTest < -SINGULARITY_THRESHOLD) {
            rotatorFromQuat.Pitch = -90.0f;
            rotatorFromQuat.Yaw =(float)(Math.Atan2(yawY, yawX) * RAD_TO_DEG);
            rotatorFromQuat.Roll = FRotator.NormalizeAxis((float)(-rotatorFromQuat.Yaw - (2.0f * Math.Atan2(X, W) * RAD_TO_DEG)));
        } else if (singularityTest > SINGULARITY_THRESHOLD) {
            rotatorFromQuat.Pitch = 90.0f;
            rotatorFromQuat.Yaw = (float)(Math.Atan2(yawY, yawX) * RAD_TO_DEG);
            rotatorFromQuat.Roll = FRotator.NormalizeAxis((float)(rotatorFromQuat.Yaw - (2.0f * Math.Atan2(X, W) * RAD_TO_DEG)));
        } else {
            rotatorFromQuat.Pitch = (float)(Math.Asin(2.0f * singularityTest) * RAD_TO_DEG);
            rotatorFromQuat.Yaw = (float)(Math.Atan2(yawY, yawX) * RAD_TO_DEG);
            rotatorFromQuat.Roll = FRotator.NormalizeAxis((float)(Math.Atan2(-2.0f * (W * X + Y * Z), (1.0f - 2.0f * (X * X + Y * Y))) * RAD_TO_DEG));
        }

        return rotatorFromQuat;

    }
}
