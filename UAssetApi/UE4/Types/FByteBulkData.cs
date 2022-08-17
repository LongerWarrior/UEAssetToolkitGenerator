namespace UAssetAPI;

[Flags]
public enum EBulkDataFlags : uint {
    BULKDATA_PayloadAtEndOfFile = 0x0001,               // bulk data stored at the end of this file, data offset added to global data offset in package
    BULKDATA_CompressedZlib = 0x0002,                   // the same value as for UE3
    BULKDATA_Unused = 0x0020,                           // the same value as for UE3
    BULKDATA_ForceInlinePayload = 0x0040,               // bulk data stored immediately after header
    BULKDATA_PayloadInSeperateFile = 0x0100,            // data stored in .ubulk file near the asset (UE4.12+)
    BULKDATA_SerializeCompressedBitWindow = 0x0200,     // use platform-specific compression
    BULKDATA_OptionalPayload = 0x0800,                  // same as BULKDATA_PayloadInSeperateFile, but stored with .uptnl extension (UE4.20+)
    BULKDATA_Size64Bit = 0x2000,                        // 64-bit size fields, UE4.22+
    BULKDATA_BadDataVersion = 0x8000,                   // I really don't know one ushort before the data
    BULKDATA_NoOffsetFixUp = 0x10000                    // do not add Summary.BulkDataStartOffset to bulk location, UE4.26
}

public struct FByteBulkDataHeader {
    public EBulkDataFlags BulkDataFlags;
    public int ElementCount;
    public uint SizeOnDisk;
    public long OffsetInFile;

    public FByteBulkDataHeader(AssetBinaryReader reader) {
        BulkDataFlags = (EBulkDataFlags)reader.ReadUInt32();
        ElementCount = BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit) ? (int)reader.ReadInt64() : reader.ReadInt32();
        SizeOnDisk = BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit) ? (uint)reader.ReadUInt64() : reader.ReadUInt32();
        OffsetInFile = reader.ReadInt64();
        if (!BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_NoOffsetFixUp)) // UE4.26 flag
        {
            OffsetInFile += reader.Asset.BulkDataStartOffset;
        }

        if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_BadDataVersion)) {
            reader.BaseStream.Position += sizeof(ushort);
            BulkDataFlags &= ~EBulkDataFlags.BULKDATA_BadDataVersion;
        }
    }
}

public class FByteBulkData {
    public FByteBulkDataHeader Header;
    public EBulkDataFlags BulkDataFlags;
    public byte[] Data;

    public FByteBulkData(AssetBinaryReader reader) {
        Header = new FByteBulkDataHeader(reader);
        BulkDataFlags = Header.BulkDataFlags;
        Data = new byte[Header.ElementCount];

        if (Header.ElementCount == 0) {
            // Nothing to do here
        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Unused)) {
            Console.WriteLine("Bulk with no data");
        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_ForceInlinePayload)) {
            Data = reader.ReadBytes(Header.ElementCount);
        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_OptionalPayload)) {

            MemoryStream bulkStream = new MemoryStream();
            var targetFile = Path.ChangeExtension(reader.Asset.FilePath, "uptnl");
            if (File.Exists(targetFile)) {
                using (FileStream newStream = File.Open(targetFile, FileMode.Open)) {
                    newStream.CopyTo(bulkStream);
                }
            } else {
                return;
            }
            AssetBinaryReader bulkreader = new AssetBinaryReader(bulkStream);
            bulkreader.BaseStream.Position = Header.OffsetInFile;
            Data = bulkreader.ReadBytes(Header.ElementCount);

        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadInSeperateFile)) {
            //MemoryStream bulkStream = new MemoryStream();
            var targetFile = Path.ChangeExtension(reader.Asset.FilePath, "ubulk");
            if (File.Exists(targetFile)) {
                using (FileStream newStream = File.Open(targetFile, FileMode.Open)) {
                    newStream.Position = Header.OffsetInFile;
                    var length = newStream.Read(Data, 0, Header.ElementCount);
                    if (length != Header.ElementCount) {
                        throw new NotImplementedException("ubulk bad read result");
                    }
                    newStream.Close();
                    return;

                    //newStream.CopyTo(bulkStream);
                }
            } else {
                return;
            }
            
            
            //AssetBinaryReader bulkreader = new AssetBinaryReader(bulkStream);
            //bulkreader.BaseStream.Position = Header.OffsetInFile;
            //Data = bulkreader.ReadBytes(Header.ElementCount);
        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadAtEndOfFile)) {
            //stored in same file, but at different position
            //save archive position
            var savePos = reader.BaseStream.Position;
            if (Header.OffsetInFile + Header.ElementCount <=  reader.BaseStream.Length) {
                reader.BaseStream.Position = Header.OffsetInFile;
                Data = reader.ReadBytes(Header.ElementCount);
            } else throw new NotImplementedException($"Failed to read PayloadAtEndOfFile, {Header.OffsetInFile} is out of range");

            reader.BaseStream.Position = savePos;
        } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_CompressedZlib)) {
            throw new NotImplementedException("EBulkDataFlags.BULKDATA_CompressedZlib");
            //throw new ParserException(Ar, "TODO: CompressedZlib");
        }
    }

    public void Write(AssetBinaryWriter writer) {

    }

    protected FByteBulkData(AssetBinaryReader reader, bool skip = false) {
        Header = new FByteBulkDataHeader(reader);
        var bulkDataFlags = Header.BulkDataFlags;

        if (bulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Unused | EBulkDataFlags.BULKDATA_PayloadInSeperateFile | EBulkDataFlags.BULKDATA_PayloadAtEndOfFile)) {
            return;
        }

        if (bulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_ForceInlinePayload) || Header.OffsetInFile == reader.BaseStream.Position) {
            reader.BaseStream.Position += Header.SizeOnDisk;
        }
    }
}
