namespace UAssetAPI.Kismet.Bytecode.Expressions;

/// <summary>
/// A single Kismet bytecode instruction, corresponding to the <see cref="EExprToken.RotationConst"/> instruction.
/// </summary>
public class EX_RotationConst : KismetExpression
{
    /// <summary>Rotation around the right axis (around Y axis), Looking up and down (0=Straight Ahead, +Up, -Down)</summary>
    [JsonProperty]
    public float Pitch;

    /// <summary>Rotation around the up axis (around Z axis), Running in circles 0=East, +North, -South.</summary>
    [JsonProperty]
    public float Yaw;

    /// <summary>Rotation around the forward axis (around X axis), Tilting your head, 0=Straight, +Clockwise, -CCW.</summary>
    [JsonProperty]
    public float Roll;

    /// <summary>
    /// The token of this expression.
    /// </summary>
    public override EExprToken Token { get { return EExprToken.RotationConst; } }

    public EX_RotationConst()
    {

    }

    /// <summary>
    /// Reads out the expression from a BinaryReader.
    /// </summary>
    /// <param name="reader">The BinaryReader to read from.</param>
    public override void Read(AssetBinaryReader reader)
    {
        Pitch = reader.ReadSingle();
        Yaw = reader.ReadSingle();
        Roll = reader.ReadSingle();
    }

    /// <summary>
    /// Writes the expression to a BinaryWriter.
    /// </summary>
    /// <param name="writer">The BinaryWriter to write from.</param>
    /// <returns>The iCode offset of the data that was written.</returns>
    public override int Write(AssetBinaryWriter writer)
    {
        writer.Write(Pitch);
        writer.Write(Yaw);
        writer.Write(Roll);
        return sizeof(int) * 3;
    }
}
