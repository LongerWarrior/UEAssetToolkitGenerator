using System;
using System.IO;
using UAssetAPI.StructTypes;


namespace UAssetAPI {
    public struct FStripDataFlags {
        public byte GlobalStripFlags;
        public byte ClassStripFlags;


        public FStripDataFlags(AssetBinaryReader reader) {
            GlobalStripFlags = reader.ReadByte();
            ClassStripFlags = reader.ReadByte();
        }

        public void Write(AssetBinaryWriter writer) {
            writer.Write(GlobalStripFlags);
            writer.Write(ClassStripFlags);
        }

        public bool IsEditorDataStripped() {
            return (GlobalStripFlags & 1) != 0;
        }

        public bool IsDataStrippedForServer() {
            return (GlobalStripFlags & 2) != 0;
        }

        public bool IsClassDataStripped(byte flag) {
            return (ClassStripFlags & flag) != 0;
        }
    }

    public struct FOptTexturePlatformData {
        public uint ExtData;
        public uint NumMipsInTail;

        public FOptTexturePlatformData(AssetBinaryReader reader) {
            ExtData = reader.ReadUInt32();
            NumMipsInTail = reader.ReadUInt32();
        }
    }

    [Flags]
    public enum EBulkDataFlags : uint {
        BULKDATA_PayloadAtEndOfFile =
            0x0001, // bulk data stored at the end of this file, data offset added to global data offset in package
        BULKDATA_CompressedZlib = 0x0002, // the same value as for UE3
        BULKDATA_Unused = 0x0020, // the same value as for UE3
        BULKDATA_ForceInlinePayload = 0x0040, // bulk data stored immediately after header
        BULKDATA_PayloadInSeperateFile = 0x0100, // data stored in .ubulk file near the asset (UE4.12+)
        BULKDATA_SerializeCompressedBitWindow = 0x0200, // use platform-specific compression

        BULKDATA_OptionalPayload =
            0x0800, // same as BULKDATA_PayloadInSeperateFile, but stored with .uptnl extension (UE4.20+)
        BULKDATA_Size64Bit = 0x2000, // 64-bit size fields, UE4.22+
        BULKDATA_BadDataVersion = 0x8000, // I really don't know one ushort before the data
        BULKDATA_NoOffsetFixUp = 0x10000 // do not add Summary.BulkDataStartOffset to bulk location, UE4.26
    }

    public struct FByteBulkDataHeader {
        public EBulkDataFlags BulkDataFlags;
        public int ElementCount;
        public uint SizeOnDisk;
        public long OffsetInFile;

        public FByteBulkDataHeader(AssetBinaryReader reader) {
            BulkDataFlags = (EBulkDataFlags)reader.ReadUInt32();
            ElementCount = BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit)
                ? (int)reader.ReadInt64()
                : reader.ReadInt32();
            SizeOnDisk = BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_Size64Bit)
                ? (uint)reader.ReadUInt64()
                : reader.ReadUInt32();
            OffsetInFile = reader.ReadInt64();
            if (!BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_NoOffsetFixUp)) // UE4.26 flag
                OffsetInFile += reader.Asset.BulkDataStartOffset;

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
                var bulkStream = new MemoryStream();
                var targetFile = Path.ChangeExtension(reader.Asset.FilePath, "uptnl");
                if (File.Exists(targetFile)) {
                    using var newStream = File.Open(targetFile, FileMode.Open);
                    newStream.CopyTo(bulkStream);
                } else return;

                var bulkreader = new AssetBinaryReader(bulkStream);
                bulkreader.BaseStream.Position = Header.OffsetInFile;
                Data = bulkreader.ReadBytes(Header.ElementCount);
            } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadInSeperateFile)) {
                // BufferedStream bulkStream = new BufferedStream(new MemoryStream());
                var targetFile = Path.ChangeExtension(reader.Asset.FilePath, "ubulk");
                if (File.Exists(targetFile)) {
                    using var newStream = File.Open(targetFile, FileMode.Open);
                    newStream.Position = Header.OffsetInFile;
                    var length = newStream.Read(Data, 0, Header.ElementCount);
                    if (length != Header.ElementCount) throw new NotImplementedException("ubulk bad read result");
                    newStream.Close();
                    return;

                    //newStream.CopyTo(bulkStream);
                } else {
                    return;
                }
                /*AssetBinaryReader bulkreader = new AssetBinaryReader(bulkStream);
                bulkreader.BaseStream.Position = Header.OffsetInFile;
                Data = bulkreader.ReadBytes(Header.ElementCount);*/
            } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_PayloadAtEndOfFile)) {
                //stored in same file, but at different position
                //save archive position
                var savePos = reader.BaseStream.Position;
                if (Header.OffsetInFile + Header.ElementCount <= reader.BaseStream.Length) {
                    reader.BaseStream.Position = Header.OffsetInFile;
                    Data = reader.ReadBytes(Header.ElementCount);
                } else {
                    throw new NotImplementedException(
                        $"Failed to read PayloadAtEndOfFile, {Header.OffsetInFile} is out of range");
                }

                reader.BaseStream.Position = savePos;
            } else if (BulkDataFlags.HasFlag(EBulkDataFlags.BULKDATA_CompressedZlib)) {
                throw new NotImplementedException("EBulkDataFlags.BULKDATA_CompressedZlib");
                //throw new ParserException(Ar, "TODO: CompressedZlib");
            }
        }

        public void Write(AssetBinaryWriter writer) {
        }

        /*protected FByteBulkData(FAssetArchive Ar, bool skip = false) {
            Header = new FByteBulkDataHeader(Ar);
            var bulkDataFlags = Header.BulkDataFlags;

            if (bulkDataFlags.HasFlag(BULKDATA_Unused | BULKDATA_PayloadInSeperateFile | BULKDATA_PayloadAtEndOfFile)) {
                return;
            }

            if (bulkDataFlags.HasFlag(BULKDATA_ForceInlinePayload) || Header.OffsetInFile == Ar.Position) {
                Ar.Position += Header.SizeOnDisk;
            }
        }*/
    }


    public class FTexture2DMipMap {
        public bool cooked;
        public FByteBulkData Data;
        public int SizeX;
        public int SizeY;
        public int SizeZ;

        public FTexture2DMipMap(AssetBinaryReader reader) {
            cooked = reader.ReadInt32() != 0;

            Data = new FByteBulkData(reader);

            SizeX = reader.ReadInt32();
            SizeY = reader.ReadInt32();
            SizeZ = reader.Asset.EngineVersion >= UE4Version.VER_UE4_20 ? reader.ReadInt32() : 1;

            //if (Ar.Ver >= EUnrealEngineObjectUE4Version.TEXTURE_DERIVED_DATA2 && !cooked) {
            //    var derivedDataKey = Ar.ReadFString();
            //}
        }
    }

    public enum EVirtualTextureCodec : byte {
        Black, //Special case codec, always outputs black pixels 0,0,0,0
        OpaqueBlack, //Special case codec, always outputs opaque black pixels 0,0,0,255
        White, //Special case codec, always outputs white pixels 255,255,255,255
        Flat, //Special case codec, always outputs 128,125,255,255 (flat normal map)
        RawGPU, //Uncompressed data in an GPU-ready format (e.g R8G8B8A8, BC7, ASTC, ...)
        ZippedGPU, //Same as RawGPU but with the data zipped
        Crunch, //Use the Crunch library to compress data
        Max // Add new codecs before this entry
    };

    public class FVirtualTextureDataChunk {
        public FByteBulkData BulkData;
        public uint SizeInBytes;
        public uint CodecPayloadSize;
        public ushort[] CodecPayloadOffset;
        public EVirtualTextureCodec[] CodecType;

        /*public FVirtualTextureDataChunk(FAssetArchive Ar, uint numLayers) {
            CodecType = new EVirtualTextureCodec[numLayers];
            CodecPayloadOffset = new ushort[numLayers];

            SizeInBytes = Ar.Read<uint>();
            CodecPayloadSize = Ar.Read<uint>();
            for (uint layerIndex = 0u; layerIndex < numLayers; ++layerIndex) {
                CodecType[layerIndex] = Ar.Read<EVirtualTextureCodec>();
                CodecPayloadOffset[layerIndex] = Ar.Read<ushort>();
            }
            BulkData = new FByteBulkData(Ar);
        }*/
    }

    public class FVirtualTextureBuiltData {
        public uint NumLayers;
        public uint? NumMips;
        public uint? Width;
        public uint? Height;
        public uint WidthInBlocks;
        public uint HeightInBlocks;
        public uint TileSize;
        public uint TileBorderSize;
        public EPixelFormat[] LayerTypes;
        public FVirtualTextureDataChunk[] Chunks;
        public uint[]? TileIndexPerChunk;
        public uint[]? TileIndexPerMip;
        public uint[]? TileOffsetInChunk;

        /*public FVirtualTextureBuiltData(FAssetArchive Ar, int firstMip) {
            bool bStripMips = firstMip > 0;
            var bCooked = Ar.ReadBoolean();

            NumLayers = Ar.Read<uint>();
            WidthInBlocks = Ar.Read<uint>();
            HeightInBlocks = Ar.Read<uint>();
            TileSize = Ar.Read<uint>();
            TileBorderSize = Ar.Read<uint>();
            if (!bStripMips) {
                NumMips = Ar.Read<uint>();
                Width = Ar.Read<uint>();
                Height = Ar.Read<uint>();
                TileIndexPerChunk = Ar.ReadArray<uint>();
                TileIndexPerMip = Ar.ReadArray<uint>();
                TileOffsetInChunk = Ar.ReadArray<uint>();
            }

            LayerTypes = Ar.ReadArray((int)NumLayers, () => (EPixelFormat)Enum.Parse(typeof(EPixelFormat), Ar.ReadFString()));
            Chunks = Ar.ReadArray(() => new FVirtualTextureDataChunk(Ar, NumLayers));
        }*/
    }

    public class FTexturePlatformData {
        private const uint BitMask_CubeMap = 1u << 31;
        private const uint BitMask_HasOptData = 1u << 30;
        private const uint BitMask_NumSlices = BitMask_HasOptData - 1u;

        public int SizeX;
        public int SizeY;
        public int PackedData; // NumSlices: 1 for simple texture, 6 for cubemap - 6 textures are joined into one
        public FString PixelFormat;
        public FOptTexturePlatformData OptData;
        public int FirstMipToSerialize;
        public FTexture2DMipMap[] Mips;
        public FVirtualTextureBuiltData VTData;

        public FTexturePlatformData(AssetBinaryReader reader) {
            SizeX = reader.ReadInt32();
            SizeY = reader.ReadInt32();
            PackedData = reader.ReadInt32();

            PixelFormat = reader.ReadFString();

            if ((PackedData & BitMask_HasOptData) == BitMask_HasOptData) OptData = new FOptTexturePlatformData(reader);

            FirstMipToSerialize =
                reader.ReadInt32(); // only for cooked, but we don't read FTexturePlatformData for non-cooked textures

            var mipCount = reader.ReadInt32();
            Mips = new FTexture2DMipMap[mipCount];
            for (var i = 0; i < Mips.Length; i++) Mips[i] = new FTexture2DMipMap(reader);

            if (reader.Asset.EngineVersion >= UE4Version.VER_UE4_23) {
                var bIsVirtual = reader.ReadInt32() != 0;
                if (bIsVirtual) {
                    //VTData = new FVirtualTextureBuiltData(reader, FirstMipToSerialize);
                }
            }
        }
    }


    public class Texture2DExport : NormalExport {
        public FStripDataFlags stripFlags;
        public FStripDataFlags stripDataFlags;
        public bool bCooked;
        public FName pixelFormatEnum;
        public long skipOffset;
        public EPixelFormat pixelFormat = EPixelFormat.PF_Unknown;
        public EPixelFormat Format = EPixelFormat.PF_Unknown;
        public FTexturePlatformData PlatformData;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public int
            PackedData {
            get;
            private set;
        } // important only while UTextureCube4 is derived from UTexture2D in out implementation

        public int FirstMipToSerialize { get; private set; }
        public FOptTexturePlatformData OptData { get; private set; }
        public FTexture2DMipMap[] Mips { get; private set; }
        public FVirtualTextureBuiltData? VTData { get; private set; }
        public bool IsVirtual => VTData != null;
        public bool bRenderNearestNeighbor { get; private set; }
        public bool isNormalMap = false;


        public Texture2DExport(Export super) : base(super) {
        }

        public Texture2DExport() {
        }

        public override void Read(AssetBinaryReader reader, int nextStarting) {
            base.Read(reader, nextStarting);

            var idk = reader.ReadInt32();
            stripFlags = new FStripDataFlags(reader);
            stripDataFlags = new FStripDataFlags(reader);
            //var bCooked = Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_COOKED_TO_TEXTURE2D && Ar.ReadBoolean();
            var bCooked = reader.ReadInt32() != 0;

            if (bCooked) {
                pixelFormatEnum = reader.ReadFName();
                while (!pixelFormatEnum.IsNone) {
                    skipOffset = reader.Asset.EngineVersion switch {
                        //>= EGame.GAME_UE5_0 => Ar.AbsolutePosition + Ar.Read<long>(),
                        >= UE4Version.VER_UE4_20 => reader.ReadInt64(),
                        _ => reader.ReadInt32()
                    };

                    Enum.TryParse(pixelFormatEnum.ToName(), out pixelFormat);

                    if (Format == EPixelFormat.PF_Unknown) {
                        PlatformData = new FTexturePlatformData(reader);

                        if (reader.BaseStream.Position != skipOffset)
                            Console.WriteLine(reader.BaseStream.Position + " " + skipOffset);

                        // copy data to UTexture2D
                        SizeX = PlatformData.SizeX;
                        SizeY = PlatformData.SizeY;
                        PackedData = PlatformData.PackedData;
                        Format = pixelFormat;
                        OptData = PlatformData.OptData;
                        FirstMipToSerialize = PlatformData.FirstMipToSerialize;
                        Mips = PlatformData.Mips;
                        VTData = PlatformData.VTData;
                    } else {
                        //Ar.SeekAbsolute(skipOffset, SeekOrigin.Begin);
                    }

                    // read next format name
                    pixelFormatEnum = reader.ReadFName();
                    if (!pixelFormatEnum.IsNone) Console.WriteLine("More then one pixel format??");
                }
            }


            //ReferenceSkeleton = new FReferenceSkeleton();
            //ReferenceSkeleton.Read(reader);

            //int numOfRetargetSources = reader.ReadInt32();
            //AnimRetargetSources = new Dictionary<FName, FReferencePose>(numOfRetargetSources);
            //for (var i = 0; i < numOfRetargetSources; i++) {
            //    FName name = reader.ReadFName();
            //    FReferencePose pose = new FReferencePose();
            //    pose.Read(reader);

            //    if (pose.ReferencePose != null) ReferenceSkeleton.AdjustBoneScales(pose.ReferencePose);
            //    AnimRetargetSources[reader.ReadFName()] = pose;
            //}

            //Guid = new Guid(reader.ReadBytes(16));

            //int mapLength = reader.ReadInt32();
            //NameMappings = new Dictionary<FName, FSmartNameMapping>(mapLength);
            //for (var i = 0; i < mapLength; i++) {
            //    FName name = reader.ReadFName();
            //    FSmartNameMapping mapping = new FSmartNameMapping();
            //    mapping.Read(reader);

            //    NameMappings[name] = mapping;
            //}

            //byte GlobalStripFlags = reader.ReadByte();
            //byte ClassStripFlags = reader.ReadByte();

            //if (!((GlobalStripFlags & 1) != 0)) {
            //    //ExistingMarkerNames = Ar.ReadArray(Ar.ReadFName);
            //    throw new NotImplementedException("ExistingMarkerNames not implemented");
            //}
        }

        public override void Write(AssetBinaryWriter writer) {
            base.Write(writer);
            writer.Write((int)0);

            //writer.Write(CollisionDisableTable.Count);
            //foreach (KeyValuePair<FRigidBodyIndexPair, bool> entry in CollisionDisableTable) {
            //    writer.Write(entry.Key.Indices[0]);
            //    writer.Write(entry.Key.Indices[1]);
            //    writer.Write(entry.Value ? 1 : 0);
            //}
        }
    }
}
