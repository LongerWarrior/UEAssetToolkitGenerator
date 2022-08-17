namespace UAssetAPI.StructTypes;

public class NiagaraVariablePropertyData : StructPropertyData 
{
    [JsonProperty]
    public FName VariableName;
    public int VariableOffset;


    public NiagaraVariablePropertyData(FName name) : base(name)
    {
        Value = new List<PropertyData>();
    }

    public NiagaraVariablePropertyData(FName name, FName forcedType) : base(name)
    {
        StructType = forcedType;
        Value = new List<PropertyData>();
    }

    public NiagaraVariablePropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("NiagaraVariable");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }



    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {

        VariableName = reader.ReadFName();
        List<PropertyData> resultingList = new List<PropertyData>();
        PropertyData data = null;
        while ((data = MainSerializer.Read(reader, true)) != null) {
            resultingList.Add(data);
        }

        Value = resultingList;

        VariableOffset = reader.ReadInt32();
    }


    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        int here = (int)writer.BaseStream.Position;
        writer.XFERNAME(VariableName);
        
        if (Value != null) {
            foreach (var t in Value) {
                MainSerializer.Write(t, writer, true);
            }
        }
        writer.Write(new FName("None"));
        writer.Write(VariableOffset);
        return (int)writer.BaseStream.Position - here;
    }

}
