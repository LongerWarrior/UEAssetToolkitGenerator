namespace UAssetAPI;

/// <summary>
/// Represents an interface that a UClass (<see cref="ClassExport"/>) implements.
/// </summary>
public struct SerializedInterfaceReference
{
    public int Class;
    public int PointerOffset;
    public bool bImplementedByK2;

    public SerializedInterfaceReference(int @class, int pointerOffset, bool bImplementedByK2)
    {
        Class = @class;
        PointerOffset = pointerOffset;
        this.bImplementedByK2 = bImplementedByK2;
    }
}
