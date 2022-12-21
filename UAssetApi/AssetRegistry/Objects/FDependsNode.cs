using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UAssetAPI.AssetRegistry;

public class FDependsNode
{
    private const int PackageFlagWidth = 3;
    private const int PackageFlagSetWidth = 5; // FPropertyCombinationPack3::StorageBitCount
    private const int ManageFlagWidth = 1;
    private const int ManageFlagSetWidth = 1; // TPropertyCombinationSet<1>::StorageBitCount

    public FAssetIdentifier Identifier;
    public List<FDependsNode> PackageDependencies;
    public List<FDependsNode> NameDependencies;
    public List<FDependsNode> ManageDependencies;
    public List<FDependsNode> Referencers;
    public BitArray? PackageFlags;
    public BitArray? ManageFlags;

    internal int _index;

    public FDependsNode(int index)
    {
        _index = index;
    }

    public void SerializeLoad(FAssetRegistryArchive Ar, FDependsNode[] preallocatedDependsNodeDataBuffer)
    {
        Identifier = new FAssetIdentifier(Ar);

        void ReadDependencies(ref List<FDependsNode> outDependencies, ref BitArray? outFlagBits, int flagSetWidth)
        {
            var sortIndexes = new List<int>();
            var pointerDependencies = new List<FDependsNode>();

            var inDependencies = Ar.ReadArray<int>();
            var numDependencies = inDependencies.Length;
            var numFlagBits = flagSetWidth * numDependencies;
            var numFlagWords = (numFlagBits + 31) / 32;//.DivideAndRoundUp(32);
            var inFlagBits = numFlagWords != 0 ? new BitArray(Ar.ReadArray<int>(numFlagWords)) : new BitArray(0);

            foreach (var serializeIndex in inDependencies)
            {
                if (serializeIndex < 0 || preallocatedDependsNodeDataBuffer.Length <= serializeIndex)
                    throw new Exception($"Index {serializeIndex} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
                var dependsNode = preallocatedDependsNodeDataBuffer[serializeIndex];
                pointerDependencies.Add(dependsNode);
            }

            for (var i = 0; i < numDependencies; i++)
            {
                sortIndexes.Add(i);
            }

            sortIndexes.Sort((a, b) => pointerDependencies[a]._index - pointerDependencies[b]._index);

            outDependencies = new List<FDependsNode>(numDependencies);
            foreach (var index in sortIndexes)
            {
                outDependencies.Add(pointerDependencies[index]);
            }

            outFlagBits = new BitArray(numFlagBits);
            for (var writeIndex = 0; writeIndex < numDependencies; writeIndex++)
            {
                var readIndex = sortIndexes[writeIndex];
                outFlagBits.SetRangeFromRange(writeIndex * flagSetWidth, flagSetWidth, inFlagBits, readIndex * flagSetWidth);
            }
        }

        void ReadDependenciesNoFlags(ref List<FDependsNode> outDependencies)
        {
            var sortIndexes = new List<int>();
            var pointerDependencies = new List<FDependsNode>();

            var inDependencies = Ar.ReadArray<int>();
            var numDependencies = inDependencies.Length;

            foreach (var serializeIndex in inDependencies)
            {
                if (serializeIndex < 0 || preallocatedDependsNodeDataBuffer.Length <= serializeIndex)
                    throw new Exception($"Index {serializeIndex} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
                var dependsNode = preallocatedDependsNodeDataBuffer[serializeIndex];
                pointerDependencies.Add(dependsNode);
            }

            for (var i = 0; i < numDependencies; i++)
            {
                sortIndexes.Add(i);
            }

            sortIndexes.Sort((a, b) => pointerDependencies[a]._index - pointerDependencies[b]._index);

            outDependencies = new List<FDependsNode>(numDependencies);
            foreach (var index in sortIndexes)
            {
                outDependencies.Add(pointerDependencies[index]);
            }
        }

        ReadDependencies(ref PackageDependencies, ref PackageFlags, PackageFlagSetWidth);
        ReadDependenciesNoFlags(ref NameDependencies);
        ReadDependencies(ref ManageDependencies, ref ManageFlags, ManageFlagSetWidth);
        ReadDependenciesNoFlags(ref Referencers);
    }

    public void SerializeLoad_BeforeFlags(FAssetRegistryArchive Ar, FDependsNode[] preallocatedDependsNodeDataBuffer)
    {
        Identifier = new FAssetIdentifier(Ar);

        var numHard = Ar.ReadInt32();
        var numSoft = Ar.ReadInt32();
        var numName = Ar.ReadInt32();
        var numSoftManage = Ar.ReadInt32();
        var numHardManage = Ar.Version >= FAssetRegistryVersionType.AddedHardManage ? Ar.ReadInt32() : 0;
        var numReferencers = Ar.ReadInt32();

        PackageDependencies = new List<FDependsNode>(numHard + numSoft);
        NameDependencies = new List<FDependsNode>(numName);
        ManageDependencies = new List<FDependsNode>(numSoftManage + numHardManage);
        Referencers = new List<FDependsNode>(numReferencers);

        void SerializeNodeArray(int num, ref List<FDependsNode> outNodes)
        {
            for (var dependencyIndex = 0; dependencyIndex < num; ++dependencyIndex)
            {
                var index = Ar.ReadInt32();
                if (index < 0 || index >= preallocatedDependsNodeDataBuffer.Length)
                    throw new Exception($"Index {index} doesn't exist in 'PreallocatedDependsNodeDataBuffers'");
                var dependsNode = preallocatedDependsNodeDataBuffer[index];
                outNodes.Add(dependsNode);
            }
        }

        SerializeNodeArray(numHard, ref PackageDependencies);
        SerializeNodeArray(numSoft, ref PackageDependencies);
        SerializeNodeArray(numName, ref NameDependencies);
        SerializeNodeArray(numSoftManage, ref ManageDependencies);
        SerializeNodeArray(numHardManage, ref ManageDependencies);
        SerializeNodeArray(numReferencers, ref Referencers);
    }
}

public static class ArrayUtils {
    public static byte[] SubByteArray(this byte[] byteArray, int len) {
        byte[] tmp = new byte[len];
        Array.Copy(byteArray, tmp, len);

        return tmp;
    }

    public static bool Contains(this BitArray array, bool search) {
        for (var i = 0; i < array.Count; i++) {
            if (array[i])
                return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetOrFalse(this BitArray array, int index) =>
        index >= 0 && index < array.Length && array[index];

    public static void SetRangeFromRange(this BitArray array, int index, int numBitsToSet, BitArray readBits, int readOffsetBits = 0) {
        Trace.Assert(index >= 0 && numBitsToSet >= 0 && index + numBitsToSet <= array.Length);
        Trace.Assert(0 <= readOffsetBits && readOffsetBits + numBitsToSet <= readBits.Length);
        for (var i = 0; i < numBitsToSet; i++) {
            array.Set(index + i, readBits.Get(readOffsetBits + i));
        }
    }
}