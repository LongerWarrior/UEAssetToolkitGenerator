namespace UAssetAPI;

public struct FOptTexturePlatformData {
    public uint ExtData;
    public uint NumMipsInTail;

    public FOptTexturePlatformData(AssetBinaryReader reader) {
        ExtData = reader.ReadUInt32();
        NumMipsInTail = reader.ReadUInt32();
    }
}

public class FTexture2DMipMap {
    public bool cooked;
    public FByteBulkData Data;
    public int SizeX;
    public int SizeY;
    public int SizeZ;

    public FTexture2DMipMap(AssetBinaryReader reader) {
        cooked = reader.Ver >= UE4Version.TEXTURE_SOURCE_ART_REFACTOR ? reader.ReadIntBoolean() : reader.Asset.PackageFlags.HasFlag(EPackageFlags.PKG_FilterEditorOnly);

        Data = new FByteBulkData(reader);

        SizeX = reader.ReadInt32();
        SizeY = reader.ReadInt32();
        SizeZ = reader.Ver >= UE4Version.VER_UE4_20 ? reader.ReadInt32() : 1;

        if (reader.Ver >= UE4Version.TEXTURE_DERIVED_DATA2 && !cooked) {
            var derivedDataKey = reader.ReadFString();
        }
    }
}

public enum EVirtualTextureCodec : byte {
    Black,			//Special case codec, always outputs black pixels 0,0,0,0
    OpaqueBlack,	//Special case codec, always outputs opaque black pixels 0,0,0,255
    White,			//Special case codec, always outputs white pixels 255,255,255,255
    Flat,			//Special case codec, always outputs 128,125,255,255 (flat normal map)
    RawGPU,			//Uncompressed data in an GPU-ready format (e.g R8G8B8A8, BC7, ASTC, ...)
    ZippedGPU,		//Same as RawGPU but with the data zipped
    Crunch,			//Use the Crunch library to compress data
    Max,			// Add new codecs before this entry
};

public class FVirtualTextureDataChunk {
    public FByteBulkData BulkData;
    public uint SizeInBytes;
    public uint CodecPayloadSize;
    public ushort[] CodecPayloadOffset;
    public EVirtualTextureCodec[] CodecType;

    public FVirtualTextureDataChunk(AssetBinaryReader reader, uint numLayers) {
        CodecType = new EVirtualTextureCodec[numLayers];
        CodecPayloadOffset = new ushort[numLayers];

        SizeInBytes = reader.ReadUInt32();
        CodecPayloadSize = reader.ReadUInt32();
        for (uint layerIndex = 0u; layerIndex < numLayers; ++layerIndex) {
            CodecType[layerIndex] = (EVirtualTextureCodec)reader.ReadByte();
            CodecPayloadOffset[layerIndex] = reader.ReadUInt16();
        }
        BulkData = new FByteBulkData(reader);
    }
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

    public FVirtualTextureBuiltData(AssetBinaryReader reader, int firstMip) {
        bool bStripMips = firstMip > 0;
        var bCooked = reader.ReadIntBoolean();

        NumLayers = reader.ReadUInt32();
        WidthInBlocks = reader.ReadUInt32();
        HeightInBlocks = reader.ReadUInt32();
        TileSize = reader.ReadUInt32();
        TileBorderSize = reader.ReadUInt32();
        if (!bStripMips) {
            NumMips = reader.ReadUInt32();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            TileIndexPerChunk = reader.ReadArray(()=> reader.ReadUInt32());
            TileIndexPerMip = reader.ReadArray(() => reader.ReadUInt32());
            TileOffsetInChunk = reader.ReadArray(() => reader.ReadUInt32());
        }

        LayerTypes = reader.ReadArray((int)NumLayers, () => (EPixelFormat)Enum.Parse(typeof(EPixelFormat), reader.ReadFString().Value));
        Chunks = reader.ReadArray(() => new FVirtualTextureDataChunk(reader, NumLayers));
    }
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

        if ((PackedData & BitMask_HasOptData) == BitMask_HasOptData) {
            OptData = new FOptTexturePlatformData(reader);
        }

        FirstMipToSerialize = reader.ReadInt32(); // only for cooked, but we don't read FTexturePlatformData for non-cooked textures

        var mipCount = reader.ReadInt32();
        Mips = new FTexture2DMipMap[mipCount];
        for (var i = 0; i < Mips.Length; i++) {
            Mips[i] = new FTexture2DMipMap(reader);
        }

        if (reader["VirtualTextures"]) {
            var bIsVirtual = reader.ReadIntBoolean();
            if (bIsVirtual) {
                VTData = new FVirtualTextureBuiltData(reader, FirstMipToSerialize);
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
    public int PackedData { get; private set; } // important only while UTextureCube4 is derived from UTexture2D in out implementation
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

        int idk = reader.ReadInt32();
        stripFlags = new FStripDataFlags(reader);
        stripDataFlags = new FStripDataFlags(reader);
        var bCooked = reader.Ver >= UE4Version.ADD_COOKED_TO_TEXTURE2D && reader.ReadIntBoolean();

        if (bCooked) {
            pixelFormatEnum = reader.ReadFName();
            while (!pixelFormatEnum.IsNone) {
                skipOffset = reader.Ver switch {
                    //>= EGame.GAME_UE5_0 => Ar.AbsolutePosition + Ar.Read<long>(),
                    >= UE4Version.VER_UE4_20 => reader.ReadInt64(),
                    _ => reader.ReadInt32()
                };

                Enum.TryParse(pixelFormatEnum.ToName(), out pixelFormat);

                if (Format == EPixelFormat.PF_Unknown) {

                    PlatformData = new FTexturePlatformData(reader);

                    if (reader.BaseStream.Position != skipOffset) {
                        Console.WriteLine(reader.BaseStream.Position +" "+ skipOffset);
                    }

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
