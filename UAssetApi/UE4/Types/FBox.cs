namespace UAssetAPI;
public struct FBox : IUStruct {
    /// <summary>
    /// Holds the box's minimum point.
    /// </summary>
    public FVector Min;

    /// <summary>
    /// Holds the box's maximum point.
    /// </summary>
    public FVector Max;

    /// <summary>
    /// Holds a flag indicating whether this box is valid.
    /// </summary>
    public bool IsValid; // It's a bool

    /// <summary>
    /// Creates and initializes a new box from the specified extents.
    /// </summary>
    /// <param name="min">The box's minimum point.</param>
    /// <param name="max">The box's maximum point.</param>
    public FBox(FVector min, FVector max, bool isValid = true) {
        Min = min;
        Max = max;
        IsValid = isValid;
    }

    //public FBox(FVector[] points) {
    //    Min = new FVector(0f, 0f, 0f);
    //    Max = new FVector(0f, 0f, 0f);
    //    IsValid = 0;
    //    foreach (var it in points) {
    //        this += it;
    //    }
    //}

    public FBox(AssetBinaryReader reader) {
        Min = reader.ReadVector();
        Max = reader.ReadVector();
        IsValid = reader.ReadBoolean();
    }

    public FBox(FBox box) {
        Min = box.Min;
        Max = box.Max;
        IsValid = box.IsValid;
    }

}
