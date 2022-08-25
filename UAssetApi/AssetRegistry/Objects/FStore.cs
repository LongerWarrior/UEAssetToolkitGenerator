using System.Text;

namespace UAssetAPI.AssetRegistry;

public class FStore
{
    private const uint _OLD_BEGIN_MAGIC = 0x12345678u;
    private const uint _BEGIN_MAGIC = 0x12345679u;
    private const uint _END_MAGIC = 0x87654321u;

    public readonly FNumberedPair[] Pairs;
    public readonly FNumberlessPair[] NumberlessPairs;
    public readonly uint[] AnsiStringOffsets;
    public readonly byte[] AnsiStrings;
    public readonly uint[] WideStringOffsets;
    public readonly byte[] WideStrings;
    public readonly uint[] NumberlessNames;
    public readonly FName[] Names;
    public readonly FNumberlessExportPath[] NumberlessExportPaths;
    public readonly FAssetRegistryExportPath[] ExportPaths;
    public readonly string[] Texts;
    
    public readonly FNameEntrySerialized[] NameMap;
    
    public FStore(FAssetRegistryReader ARreader)
    {
        NameMap = ARreader.NameMap;
        var magic = ARreader.ReadUInt32();
        var order = GetLoadOrder(magic);
        var nums = ARreader.ReadArray(11, () => ARreader.ReadInt32());

        if (order == ELoadOrder.TextFirst)
        {
            ARreader.BaseStream.Position += 4;//Length
            Texts = ARreader.ReadArray(nums[4], () => ARreader.ReadFString().ToString());
        }

        NumberlessNames = ARreader.ReadArray(nums[0], () => ARreader.ReadUInt32());
        Names = ARreader.ReadArray(nums[1], ARreader.ReadFName);
        NumberlessExportPaths = ARreader.ReadArray(nums[2], () => new FNumberlessExportPath(ARreader));
        ExportPaths = ARreader.ReadArray(nums[3], () => new FAssetRegistryExportPath(ARreader));

        if (order == ELoadOrder.Member)
        {
            Texts = ARreader.ReadArray(nums[4], () => ARreader.ReadFString().ToString());
        }

        AnsiStringOffsets = ARreader.ReadArray(nums[5], () => ARreader.ReadUInt32());
        WideStringOffsets = ARreader.ReadArray(nums[6], () => ARreader.ReadUInt32());
        AnsiStrings = ARreader.ReadBytes(nums[7]);
        WideStrings = ARreader.ReadBytes(nums[8] * 2);
        
        NumberlessPairs = ARreader.ReadArray(nums[9], () => new FNumberlessPair(ARreader));
        Pairs = ARreader.ReadArray(nums[10], () => new FNumberedPair(ARreader));

        ARreader.BaseStream.Position += 4; // _END_MAGIC
    }

    public string GetAnsiString(int index)
    {
        var offset = AnsiStringOffsets[index];
        var length = 0;
        while (AnsiStrings[offset + length] != 0) ++length;
        return Encoding.UTF8.GetString(AnsiStrings, (int)offset, length);
    }
    
    public string GetWideString(int index)
    {
        var offset = WideStringOffsets[index];
        var length = 0;
        while (WideStrings[offset + length] != 0 && WideStrings[offset + length + 1] != 0) length += 2;
        return Encoding.Unicode.GetString(WideStrings, (int)offset, length);
    }

    private ELoadOrder GetLoadOrder(uint magic)
    {
        return magic switch
        {
            _OLD_BEGIN_MAGIC => ELoadOrder.Member,
            _BEGIN_MAGIC => ELoadOrder.TextFirst,
            _ => throw new Exception("Asset registry has bad magic number")
        };
    }
}

public static class FPartialMapHandle
{
    public static FMapHandle MakeFullHandle(FStore store, ulong mapSize)
    {
        return new (mapSize >> 63 > 0u, store, (ushort)(mapSize >> 32), (uint)mapSize);
    }
}

public class FMapHandle
{
    public readonly bool bHasNumberlessKeys;
    public readonly FStore Store;
    public readonly ushort Num;
    public readonly uint PairBegin;
    
    public FMapHandle(bool hasNumberlessKeys, FStore store, ushort num, uint pairBegin)
    {
        bHasNumberlessKeys = hasNumberlessKeys;
        Store = store;
        Num = num;
        PairBegin = pairBegin;
    }

    public IEnumerable<FNumberedPair> GetEnumerable()
    {
        if (bHasNumberlessKeys)
        {
            foreach (var pair in GetNumberlessView())
            {
                yield return new FNumberedPair(new FName(Store.NameMap[pair.Key].Name), pair.Value);
            }
        }
        else
        {
            foreach (var pair in GetNumberView())
            {
                yield return pair;
            }
        }
    }

    private IEnumerable<FNumberedPair> GetNumberView()
    {
        return new ArraySegment<FNumberedPair>(Store.Pairs, (int) PairBegin, Num);
    }

    private IEnumerable<FNumberlessPair> GetNumberlessView()
    {
        return new ArraySegment<FNumberlessPair>(Store.NumberlessPairs, (int) PairBegin, Num);
    }
}