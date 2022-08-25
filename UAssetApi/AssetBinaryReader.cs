using System.Text;
using UAssetAPI.Kismet.Bytecode;

namespace UAssetAPI;

public class AssetBinaryReader : BinaryReader
{
    public UAsset Asset;
    public UE4Version UEVersion;

    public AssetBinaryReader(Stream stream) : base(stream) {
    }
    public AssetBinaryReader(Stream stream, UE4Version ver) : base(stream) {
        UEVersion = ver;
    }

    public AssetBinaryReader(Stream stream, UAsset asset) : base(stream)
    {
        Asset = asset;
    }

    private byte[] ReverseIfBigEndian(byte[] data)
    {
        if (!BitConverter.IsLittleEndian) Array.Reverse(data);
        return data;
    }

    public override char ReadChar() {
        return BitConverter.ToChar(ReverseIfBigEndian(base.ReadBytes(2)), 0);
    }

    public override short ReadInt16()
    {
        return BitConverter.ToInt16(ReverseIfBigEndian(base.ReadBytes(2)), 0);
    }

    public override ushort ReadUInt16()
    {
        return BitConverter.ToUInt16(ReverseIfBigEndian(base.ReadBytes(2)), 0);
    }

    public override int ReadInt32()
    {
        return BitConverter.ToInt32(ReverseIfBigEndian(base.ReadBytes(4)), 0);
    }

    public override uint ReadUInt32()
    {
        return BitConverter.ToUInt32(ReverseIfBigEndian(base.ReadBytes(4)), 0);
    }

    public override long ReadInt64()
    {
        return BitConverter.ToInt64(ReverseIfBigEndian(base.ReadBytes(8)), 0);
    }

    public override ulong ReadUInt64()
    {
        return BitConverter.ToUInt64(ReverseIfBigEndian(base.ReadBytes(8)), 0);
    }

    public override float ReadSingle()
    {
        return BitConverter.ToSingle(ReverseIfBigEndian(base.ReadBytes(4)), 0);
    }

    public override double ReadDouble()
    {
        return BitConverter.ToDouble(ReverseIfBigEndian(base.ReadBytes(8)), 0);
    }

    public override string ReadString()
    {
        return ReadFString()?.Value;
    }

    public virtual Guid? ReadPropertyGuid()
    {
        if (Asset.EngineVersion >= UE4Version.VER_UE4_PROPERTY_GUID_IN_PROPERTY_TAG)
        {
            bool hasPropertyGuid = ReadBoolean();
            if (hasPropertyGuid) return new Guid(ReadBytes(16));
        }
        return null;
    }

    public virtual Guid? ReadPropertyGuid1() {
        if (Asset.EngineVersion >= UE4Version.VER_UE4_PROPERTY_GUID_IN_PROPERTY_TAG) {
            return new Guid(ReadBytes(16));
        }
        return null;
    }

    public virtual FString ReadFString()
    {
        int length = this.ReadInt32();
        switch (length)
        {
            case 0:
                return null;
            default:
                if (length < 0)
                {
                    byte[] data = this.ReadBytes(-length * 2);
                    return new FString(Encoding.Unicode.GetString(data, 0, data.Length - 2), Encoding.Unicode);
                }
                else
                {
                    byte[] data = this.ReadBytes(length);
                    return new FString(Encoding.ASCII.GetString(data, 0, data.Length - 1), Encoding.ASCII);
                }
        }
    }

    public virtual FString ReadNameMapString(out uint hashes)
    {
        FString str = this.ReadFString();
        hashes = 0;

        if (Asset.EngineVersion >= UE4Version.VER_UE4_NAME_HASHES_SERIALIZED && !string.IsNullOrEmpty(str.Value))
        {
            hashes = this.ReadUInt32();
        }
        return str;
    }

    public virtual FName ReadFName()
    {
        int nameMapPointer = this.ReadInt32();
        int number = this.ReadInt32();
        return new FName(Asset.GetNameReference(nameMapPointer), number);
    }

    public string XFERSTRING()
    {
        List<byte> readData = new List<byte>();
        while (true)
        {
            byte newVal = this.ReadByte();
            if (newVal == 0) break;
            readData.Add(newVal);
        }
        return Encoding.ASCII.GetString(readData.ToArray());
    }

    public string XFERUNICODESTRING()
    {
        List<byte> readData = new List<byte>();
        while (true)
        {
            byte newVal1 = this.ReadByte();
            byte newVal2 = this.ReadByte();
            if (newVal1 == 0 && newVal2 == 0) break;
            readData.Add(newVal1);
            readData.Add(newVal2);
        }
        return Encoding.Unicode.GetString(readData.ToArray());
    }

    public void XFERTEXT()
    {

    }

    public FName XFERNAME()
    {
        return this.ReadFName();
    }

    public FName XFER_FUNC_NAME()
    {
        return this.XFERNAME();
    }

    public FPackageIndex XFERPTR()
    {
        return new FPackageIndex(this.ReadInt32());
    }

    public FPackageIndex XFER_FUNC_POINTER()
    {
        return this.XFERPTR();
    }

    public KismetPropertyPointer XFER_PROP_POINTER()
    {
        if (Asset.EngineVersion >= KismetPropertyPointer.XFER_PROP_POINTER_SWITCH_TO_SERIALIZING_AS_FIELD_PATH_VERSION)
        {
            int numEntries = this.ReadInt32();
            FName[] allNames = new FName[numEntries];
            for (int i = 0; i < numEntries; i++)
            {
                allNames[i] = this.ReadFName();
            }
            FPackageIndex owner = this.XFER_OBJECT_POINTER();
            return new KismetPropertyPointer(new FFieldPath(allNames, owner));
        }
        else
        {
            return new KismetPropertyPointer(this.XFERPTR());
        }
    }

    public FPackageIndex XFER_OBJECT_POINTER()
    {
        return this.XFERPTR();
    }

    public KismetExpression[] ReadExpressionArray(EExprToken endToken)
    {
        List<KismetExpression> newData = new List<KismetExpression>();
        KismetExpression currExpression = null;
        //while (currExpression == null || currExpression.Token != endToken)
        while (currExpression == null || currExpression.Token != endToken)
        {
            if (currExpression != null) newData.Add(currExpression);
            currExpression = ExpressionSerializer.ReadExpression(this);
        }
        
        return newData.ToArray();
    }

    public virtual FVector ReadVector() {
        return new FVector(ReadSingle(), ReadSingle(), ReadSingle());
    }
    public virtual FQuat ReadQuat() {
        return new FQuat(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }

    public bool ReadIntBoolean() {
        var i = ReadInt32();
        return i switch {
            0 => false,
            1 => true,
            _ => throw new Exception($"Invalid bool value ({i})")
        };
    }

    public virtual T[] ReadArray<T>() where T : struct {
        var length = ReadInt32();
        return length > 0 ? ReadArray<T>(length) : Array.Empty<T>();
    }

    public virtual T[] ReadArray<T>(int length) {
        var result = new T[length];
        for (var i = 0; i < length; i++) {
            result[i] = (T)Activator.CreateInstance(typeof(T), this);
        }
        return result;
    }

    public virtual T[] ReadArray<T>(Func<T> getter) {
        var length = ReadInt32();
        return ReadArray(length, getter);
    }

    public void ReadArray<T>(T[] array, Func<T> getter) {
        // array is a reference type
        for (int i = 0; i < array.Length; i++) {
            array[i] = getter();
        }
    }

    public T[] ReadArray<T>(int length, Func<T> getter) {
        var result = new T[length];

        if (length == 0) {
            //return result;
            return Array.Empty<T>();
        }

        ReadArray(result, getter);

        return result;
    }

    public T[] ReadBulkArray<T>(Func<T> getter) {
        var elementSize = ReadInt32();
        var elementCount = ReadInt32();
        return ReadBulkArray(elementSize, elementCount, getter);
    }

    public T[] ReadBulkArray<T>(int elementSize, int elementCount, Func<T> getter) {
        var pos = BaseStream.Position;
        T[] array = ReadArray(elementCount, getter);
        if (BaseStream.Position != pos + array.Length * elementSize)
            throw new Exception($"RawArray item size mismatch: expected {elementSize}, serialized {(BaseStream.Position - pos) / array.Length}");
        return array;
    }

    public void SkipBulkArrayData() {
        var elementSize = ReadInt32();
        var elementCount = ReadInt32();
        BaseStream.Position += elementSize * elementCount;
    }


    public bool this[string optionKey] {
        get => GetOption(optionKey);
    }

    private bool GetOption(string optionKey) {

        if (Singleton.GetOption(optionKey, out bool value)) {
            return value;
        }

        switch (optionKey) {
            case "RawIndexBuffer.HasShouldExpandTo32Bit":  return Ver >=  UE4Version.VER_UE4_25;
            //case "ShaderMap.UseNewCookedFormat":  return Asset.EngineVersion >=  UE4Version.VER_UE5_0;
            case "SkeletalMesh.KeepMobileMinLODSettingOnDesktop":  return Ver >=  UE4Version.VER_UE4_27;
            case "SkeletalMesh.UseNewCookedFormat":  return Ver >=  UE4Version.VER_UE4_24;
            case "SkeletalMesh.HasRayTracingData":  return Ver >=  UE4Version.VER_UE4_27;
            case "StaticMesh.HasLODsShareStaticLighting":  return Ver <  UE4Version.VER_UE4_15 || Ver >= UE4Version.VER_UE4_24;
            case "StaticMesh.HasRayTracingGeometry":  return Ver >=  UE4Version.VER_UE4_25;
            case "StaticMesh.HasVisibleInRayTracing":  return Ver >=  UE4Version.VER_UE4_26;
            case "StaticMesh.KeepMobileMinLODSettingOnDesktop":  return Ver >=  UE4Version.VER_UE4_27;
            case "StaticMesh.UseNewCookedFormat":  return Ver >=  UE4Version.VER_UE4_23;
            case "VirtualTextures":  return Ver >=  UE4Version.VER_UE4_23;
            default: 
                throw new Exception("Not valid Option");
        }

    }

    public UE4Version Ver => Asset is null ? UEVersion : Asset.EngineVersion;
    public void SkipFixedArray(int size = -1) {
        var num = ReadInt32();
        BaseStream.Position += num * size;
    }
}
