namespace UAssetAPI.StructTypes;

public abstract class MaterialInputPropertyData<T> : PropertyData<T>
{
    [JsonProperty]
    public int Mask;
    [JsonProperty]
    public int MaskR;
    [JsonProperty]
    public int MaskG;
    [JsonProperty]
    public int MaskB;
    [JsonProperty]
    public int MaskA;
    [JsonProperty]
    public int OutputIndex;
    [JsonProperty]
    public FString InputName;
    [JsonProperty]
    public FName ExpressionName;

    public MaterialInputPropertyData()
    {

    }

    public MaterialInputPropertyData(FName name) : base(name)
    {

    }

    protected void ReadExpressionInput(AssetBinaryReader reader, bool includeHeader, long leng)
    {
        if (reader.Asset.GetCustomVersion<FCoreObjectVersion>() >= FCoreObjectVersion.MaterialInputNativeSerialize) {
            OutputIndex = reader.ReadInt32();
            if (reader.Asset.GetCustomVersion<FFrameworkObjectVersion>() >= FFrameworkObjectVersion.PinsStoreFName) {
                InputName = reader.ReadFName().Value;
            } else {
                InputName = reader.ReadFString();
            }
            Mask = reader.ReadInt32();
            MaskR = reader.ReadInt32();
            MaskG = reader.ReadInt32();
            MaskB = reader.ReadInt32();
            MaskA = reader.ReadInt32();
            ExpressionName = reader.ReadFName();
        }
    }

    protected int WriteExpressionInput(AssetBinaryWriter writer, bool includeHeader)
    {
        int totalSize = 0;
        if (writer.Asset.GetCustomVersion<FCoreObjectVersion>() >= FCoreObjectVersion.MaterialInputNativeSerialize) {
            writer.Write(OutputIndex); totalSize += sizeof(int);
            if (writer.Asset.GetCustomVersion<FFrameworkObjectVersion>() >= FFrameworkObjectVersion.PinsStoreFName) {
                writer.Write(new FName(InputName)); totalSize += sizeof(int) * 2;
            } else {
                totalSize += writer.Write(InputName);
            }
            writer.Write(Mask);
            writer.Write(MaskR);
            writer.Write(MaskG);
            writer.Write(MaskB);
            writer.Write(MaskA);
            totalSize += 20;
            writer.Write(ExpressionName); totalSize += sizeof(int) * 2;
        }
        return totalSize;

    }

    protected override void HandleCloned(PropertyData res)
    {
        MaterialInputPropertyData<T> cloningProperty = (MaterialInputPropertyData<T>)res;
        cloningProperty.InputName = (FString)this.InputName.Clone();
        cloningProperty.ExpressionName = (FName)this.ExpressionName.Clone();
        cloningProperty.Mask = this.Mask;
        cloningProperty.MaskR = this.MaskR;
        cloningProperty.MaskG = this.MaskG;
        cloningProperty.MaskB = this.MaskB;
        cloningProperty.MaskA = this.MaskA;

    }
}

public class ExpressionInputPropertyData : MaterialInputPropertyData<int>
{
    public ExpressionInputPropertyData(FName name) : base(name)
    {

    }

    public ExpressionInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("ExpressionInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        return WriteExpressionInput(writer, false);
    }
}

public class MaterialAttributesInputPropertyData : MaterialInputPropertyData<int>
{
    public MaterialAttributesInputPropertyData(FName name) : base(name)
    {

    }

    public MaterialAttributesInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("MaterialAttributesInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        return WriteExpressionInput(writer, false);
    }
}

public class ColorMaterialInputPropertyData : MaterialInputPropertyData<ColorPropertyData>
{
    public ColorMaterialInputPropertyData(FName name) : base(name)
    {

    }

    public ColorMaterialInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("ColorMaterialInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
        reader.ReadInt32(); // always 0
        Value = new ColorPropertyData(Name);
        Value.Read(reader, false, 0);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int expLength = WriteExpressionInput(writer, false);
        writer.Write((int)0);
        return expLength + Value.Write(writer, false) + sizeof(int);
    }
}

public class ScalarMaterialInputPropertyData : MaterialInputPropertyData<float>
{
    public ScalarMaterialInputPropertyData(FName name) : base(name)
    {

    }

    public ScalarMaterialInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("ScalarMaterialInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
        reader.ReadInt32(); // always 0
        Value = reader.ReadSingle();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int expLength = WriteExpressionInput(writer, false);
        writer.Write((int)0);
        writer.Write(Value);
        return expLength + sizeof(float) + sizeof(int);
    }
}

public class ShadingModelMaterialInputPropertyData : MaterialInputPropertyData<uint>
{
    public ShadingModelMaterialInputPropertyData(FName name) : base(name)
    {

    }

    public ShadingModelMaterialInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("ShadingModelMaterialInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
        reader.ReadInt32(); // always 0
        Value = reader.ReadUInt32();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int expLength = WriteExpressionInput(writer, false);
        writer.Write((int)0);
        writer.Write(Value);
        return expLength + sizeof(uint) + sizeof(int);
    }
}

public class VectorMaterialInputPropertyData : MaterialInputPropertyData<VectorPropertyData>
{
    public VectorMaterialInputPropertyData(FName name) : base(name)
    {

    }

    public VectorMaterialInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("VectorMaterialInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
        reader.ReadInt32(); // always 0
        Value = new VectorPropertyData(Name);
        Value.Read(reader, false, 0);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int expLength = WriteExpressionInput(writer, false);
        writer.Write((int)0);
        return expLength + Value.Write(writer, false) + sizeof(int);
    }
}

public class Vector2MaterialInputPropertyData : MaterialInputPropertyData<Vector2DPropertyData>
{
    public Vector2MaterialInputPropertyData(FName name) : base(name)
    {

    }

    public Vector2MaterialInputPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("Vector2MaterialInput");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        ReadExpressionInput(reader, false, 0);
        reader.ReadInt32(); // always 0
        Value = new Vector2DPropertyData(Name);
        Value.Read(reader, false, 0);
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int expLength = WriteExpressionInput(writer, false);
        writer.Write((int)0);
        return expLength + Value.Write(writer, false) + sizeof(int);
    }
}
