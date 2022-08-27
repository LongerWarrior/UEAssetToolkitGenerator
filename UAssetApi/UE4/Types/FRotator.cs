namespace UAssetAPI;

/// <summary>
/// Implements a container for rotation information.
/// All rotation values are stored in degrees.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class FRotator
{
    /// <summary>Rotation around the right axis (around Y axis), Looking up and down (0=Straight Ahead, +Up, -Down)</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Pitch;

    /// <summary>Rotation around the up axis (around Z axis), Running in circles 0=East, +North, -South.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Yaw;

    /// <summary>Rotation around the forward axis (around X axis), Tilting your head, 0=Straight, +Clockwise, -CCW.</summary>
    [JsonProperty]
    [JsonConverter(typeof(FSignedZeroJsonConverter))]
    public float Roll;

    public FRotator(float pitch, float yaw, float roll)
    {
        Pitch = pitch;
        Yaw = yaw;
        Roll = roll;
    }

    public FRotator()
    {

    }

    public JObject ToJson() {
        JObject res = new JObject();
        res.Add(new JProperty("Pitch", Pitch));
        res.Add(new JProperty("Yaw", Yaw));
        res.Add(new JProperty("Roll", Roll));
        return res;
    }

    public static float NormalizeAxis(float angle) {
        angle = ClampAxis(angle);

        if (angle > 180.0f) {
            // shift to (-180,180]
            angle -= 360.0f;
        } 

        return angle;
    }

    public static float ClampAxis(float angle) {
        // returns Angle in the range (-360,360)
        angle %= 360.0f;

        if (angle < 0.0f) {
            // shift to [0,360) range
            angle += 360.0f;
        }

        return angle;
    }
}
