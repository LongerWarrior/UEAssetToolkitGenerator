namespace UAssetAPI.StructTypes;

public class NiagaraVariableWithOffsetPropertyData : NiagaraVariablePropertyData {

    public NiagaraVariableWithOffsetPropertyData(FName name) : base(name) {
        Value = new List<PropertyData>();
    }

    public NiagaraVariableWithOffsetPropertyData(FName name, FName forcedType) : base(name) {
        StructType = forcedType;
        Value = new List<PropertyData>();
    }

    public NiagaraVariableWithOffsetPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("NiagaraVariableWithOffset");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }



    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {

        base.Read(reader, includeHeader, leng1);
    }


    public override int Write(AssetBinaryWriter writer, bool includeHeader) {

        return base.Write(writer,includeHeader);
    }

}