namespace UAssetAPI;

public struct FStripDataFlags {
    public byte GlobalStripFlags;
    public byte ClassStripFlags;


    public FStripDataFlags(AssetBinaryReader reader) {
        GlobalStripFlags = reader.ReadByte();
        ClassStripFlags = reader.ReadByte();
    }

    public FStripDataFlags(AssetBinaryReader reader, bool iscompatible) {
        if (iscompatible) {
            GlobalStripFlags = reader.ReadByte();
            ClassStripFlags = reader.ReadByte();
        } else {
            GlobalStripFlags = ClassStripFlags = 0;
        }
    }

    public void Write(AssetBinaryWriter writer) {
        writer.Write(GlobalStripFlags);
        writer.Write(ClassStripFlags);
    }

    public bool IsEditorDataStripped() => (GlobalStripFlags & 1) != 0;
    public bool IsDataStrippedForServer() => (GlobalStripFlags & 2) != 0;
    public bool IsClassDataStripped(byte flag) => (ClassStripFlags & flag) != 0;
}
