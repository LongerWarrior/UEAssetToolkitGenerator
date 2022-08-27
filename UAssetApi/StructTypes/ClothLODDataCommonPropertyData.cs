namespace UAssetAPI.StructTypes;

public struct FMeshToMeshVertData {
    // Barycentric coords and distance along normal for the position of the final vert
    FVector4 PositionBaryCoordsAndDist;

    // Barycentric coords and distance along normal for the location of the unit normal endpoint
    // Actual normal = ResolvedNormalPosition - ResolvedPosition
    FVector4 NormalBaryCoordsAndDist;

    // Barycentric coords and distance along normal for the location of the unit Tangent endpoint
    // Actual normal = ResolvedNormalPosition - ResolvedPosition
    FVector4 TangentBaryCoordsAndDist;

    // Contains the 3 indices for verts in the source mesh forming a triangle, the last element
    // is a flag to decide how the skinning works, 0xffff uses no simulation, and just normal
    // skinning, anything else uses the source mesh and the above skin data to get the final position
    ushort[] SourceMeshVertIndices;

    // For weighted averaging of multiple triangle influences
    float Weight = 0.0f;

    // Dummy for alignment
    uint Padding;

    public FMeshToMeshVertData(AssetBinaryReader reader) {
        PositionBaryCoordsAndDist = new FVector4(reader);
        NormalBaryCoordsAndDist = new FVector4(reader);
        TangentBaryCoordsAndDist = new FVector4(reader);
        SourceMeshVertIndices = reader.ReadArray(4,()=> reader.ReadUInt16());

        if (reader.Asset.GetCustomVersion<FReleaseObjectVersion>() < FReleaseObjectVersion.WeightFMeshToMeshVertData) {
            // Old version had "uint32 Padding[2]"
            reader.ReadUInt32();
            Padding = reader.ReadUInt32();
        } else {
            // New version has "float Weight and "uint32 Padding"

            Weight = reader.ReadSingle();
            Padding = reader.ReadUInt32();
        }
    }
}

public class MeshToMeshVertDataPropertyData : PropertyData<FMeshToMeshVertData[]> {

    public MeshToMeshVertDataPropertyData(FName name, AssetBinaryReader reader) : base(name) {
        Value = reader.ReadArray(() => new FMeshToMeshVertData(reader));
    }

    public MeshToMeshVertDataPropertyData() {
        Value = Array.Empty<FMeshToMeshVertData>();
    }

}


public class ClothLODDataCommonPropertyData : StructPropertyData {

    public ClothLODDataCommonPropertyData(FName name) : base(name) {
        Value = new List<PropertyData>();
    }

    public ClothLODDataCommonPropertyData(FName name, FName forcedType) : base(name) {
        StructType = forcedType;
        Value = new List<PropertyData>();
    }

    public ClothLODDataCommonPropertyData() {

    }

    private static readonly FName CurrentPropertyType = new FName("ClothLODDataCommon");
    public override bool HasCustomStructSerialization { get { return true; } }
    public override FName PropertyType { get { return CurrentPropertyType; } }


    public override void Read(AssetBinaryReader reader, bool includeHeader, long leng1, long leng2 = 0) {
        if (includeHeader) {
            PropertyGuid = reader.ReadPropertyGuid();
        }

        List<PropertyData> resultingList = new List<PropertyData>();
        PropertyData data = null;
        var breachkernelradius = false;

        while ((data = MainSerializer.Read(reader, true)) != null) {
            resultingList.Add(data);
        }
        resultingList.Add(new MeshToMeshVertDataPropertyData(new FName("TransitionUpSkinData"),reader));
        resultingList.Add(new MeshToMeshVertDataPropertyData(new FName("TransitionDownSkinData"),reader));

        Value = resultingList;
    }


    public override int Write(AssetBinaryWriter writer, bool includeHeader) {
        if (includeHeader) {
            writer.WritePropertyGuid(PropertyGuid);
        }

        int here = (int)writer.BaseStream.Position;

        //if (Value != null) {
        //    foreach (var t in Value) {
        //        if (t.Name.ToName() == "TypeName") {
        //            writer.Write(((StrPropertyData)t).Value);
        //        } else { MainSerializer.Write(t, writer, true); }
        //    }

        //    if (((StrPropertyData)Value[0]).Value != null) {
        //        writer.Write(new FName("None"));
        //    }
        //}

        return (int)writer.BaseStream.Position - here;
    }

}