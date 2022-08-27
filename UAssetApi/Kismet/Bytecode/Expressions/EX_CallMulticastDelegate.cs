namespace UAssetAPI.Kismet.Bytecode.Expressions;

/// <summary>
/// A single Kismet bytecode instruction, corresponding to the <see cref="EExprToken.CallMulticastDelegate"/> instruction.
/// </summary>
public class EX_CallMulticastDelegate : KismetExpression 
    {
    /// <summary>
    /// The token of this expression.
    /// </summary>
    public override EExprToken Token { get { return EExprToken.CallMulticastDelegate; } }

    /// <summary>
    /// Stack node.
    /// </summary>
    [JsonProperty]
    public FPackageIndex StackNode;

    /// <summary>
    /// List of parameters for this function.
    /// </summary>
    [JsonProperty]
    public KismetExpression[] Parameters;

    public KismetExpression Delegate;
    public EX_CallMulticastDelegate()
    {

    }

    /// <summary>
    /// Reads out the expression from a BinaryReader.
    /// </summary>
    /// <param name="reader">The BinaryReader to read from.</param>
    public override void Read(AssetBinaryReader reader)
    {
        StackNode = reader.XFER_FUNC_POINTER();
        Delegate = ExpressionSerializer.ReadExpression(reader);
        Parameters = reader.ReadExpressionArray(EExprToken.EndFunctionParms);
    }

    /// <summary>
    /// Writes the expression to a BinaryWriter.
    /// </summary>
    /// <param name="writer">The BinaryWriter to write from.</param>
    /// <returns>The iCode offset of the data that was written.</returns>
    public override int Write(AssetBinaryWriter writer)
    {

        int offset = 0;
        offset += writer.XFER_FUNC_POINTER(StackNode); ;
        offset += ExpressionSerializer.WriteExpression(Delegate, writer);


        for (int i = 0; i < Parameters.Length; i++) {
            offset += ExpressionSerializer.WriteExpression(Parameters[i], writer);
        }
        offset += ExpressionSerializer.WriteExpression(new EX_EndFunctionParms(), writer);



        return offset;

    }
}
