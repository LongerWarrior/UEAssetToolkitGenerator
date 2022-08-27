namespace UAssetAPI.PropertyTypes;

/// <summary>
/// Describes a reference variable to another object which may be null, and may become valid or invalid at any point. Synonym for <see cref="SoftObjectPropertyData"/>.
/// </summary>
public class AssetObjectPropertyData : PropertyData<FString>
{
    [JsonProperty]
    public uint ID = 0;

    public AssetObjectPropertyData(FName name) : base(name)
    {

    }

    public AssetObjectPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("AssetObjectProperty");
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        Value = reader.ReadFString();
    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }

        return writer.Write(Value);
    }

    public override string ToString()
    {
        return "(" + Value + ", " + ID + ")";
    }

    public override void FromString(string[] d, UAsset asset)
    {
        asset.AddNameReference(FString.FromString(d[0]));
        Value = FString.FromString(d[0]);
    }
}

public  struct FSoftObjectPath {
    /** Asset path, path to a top level object in a package. This is /package/path.assetname */
    public FName AssetPathName;
    /** Optional FString for subobject within an asset. This is the sub path after the : */
    public FString SubPathString;

    public FSoftObjectPath(FName assetPathName, FString subPathString) {
        AssetPathName = assetPathName;
        SubPathString = subPathString;
    }

    public JToken ToJson() {
        if (SubPathString != null) {
            return AssetPathName.ToName() + ":" + SubPathString.ToString();
        } else {
            return AssetPathName.ToName();
        }
        
    }
}



    /// <summary>
    /// Describes a reference variable to another object which may be null, and may become valid or invalid at any point. Synonym for <see cref="AssetObjectPropertyData"/>.
    /// </summary>
public class SoftObjectPropertyData : PropertyData<FSoftObjectPath>
{


    public SoftObjectPropertyData(FName name) : base(name)
    {

    }

    public SoftObjectPropertyData()
    {

    }

    private static readonly FName CurrentPropertyType = new FName("SoftObjectProperty");
    public override FName PropertyType { get { return CurrentPropertyType; } }

    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0)
    {
        if (includeHeader)
        {
            PropertyGuid = reader.ReadPropertyGuid();
        }


        Value = new FSoftObjectPath(reader.ReadFName(), reader.ReadFString());


    }

    public override int Write(AssetBinaryWriter writer, bool includeHeader)
    {
        if (includeHeader)
        {
            writer.WritePropertyGuid(PropertyGuid);
        }
        int here = (int)writer.BaseStream.Position;
        writer.Write(Value.AssetPathName);
        writer.Write(Value.SubPathString);
        return (int)writer.BaseStream.Position-here;
    }

    public override JToken ToJson() {
        return Value.ToJson();
    }

}