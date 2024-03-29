﻿using static Textures.BC.DetexCompressedTextureFormatIndex;
using static Textures.BC.DetexPixelFormat;

namespace Textures.BC;

public enum DetexTextureFormat : uint
{
    DETEX_TEXTURE_FORMAT_PIXEL_FORMAT_MASK = 0x0000FFFF,
    DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT = 0x00800000,

    DETEX_TEXTURE_FORMAT_BC1 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BC1 << 24) |
        DETEX_PIXEL_FORMAT_RGBX8
    ),

    DETEX_TEXTURE_FORMAT_BC1A = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BC1A << 24) |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_BC2 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BC2 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_BC3 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BC3 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_RGTC1 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_RGTC1 << 24) |
        DETEX_PIXEL_FORMAT_R8
    ),

    DETEX_TEXTURE_FORMAT_SIGNED_RGTC1 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_SIGNED_RGTC1 << 24) |
        DETEX_PIXEL_FORMAT_SIGNED_R16
    ),

    DETEX_TEXTURE_FORMAT_RGTC2 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_RGTC2 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RG8
    ),

    DETEX_TEXTURE_FORMAT_SIGNED_RGTC2 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_SIGNED_RGTC2 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_SIGNED_RG16
    ),

    DETEX_TEXTURE_FORMAT_BPTC_FLOAT = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BPTC_FLOAT << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_FLOAT_RGBX16
    ),

    DETEX_TEXTURE_FORMAT_BPTC_SIGNED_FLOAT = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BPTC_SIGNED_FLOAT << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_SIGNED_FLOAT_RGBX16
    ),

    DETEX_TEXTURE_FORMAT_BPTC = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_BPTC << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_ETC1 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_ETC1 << 24) |
        DETEX_PIXEL_FORMAT_RGBX8
    ),

    DETEX_TEXTURE_FORMAT_ETC2 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_ETC2 << 24) |
        DETEX_PIXEL_FORMAT_RGBX8
    ),

    DETEX_TEXTURE_FORMAT_ETC2_PUNCHTHROUGH = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_ETC2_PUNCHTHROUGH << 24) |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_ETC2_EAC = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_ETC2_EAC << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RGBA8
    ),

    DETEX_TEXTURE_FORMAT_EAC_R11 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_EAC_R11 << 24) |
        DETEX_PIXEL_FORMAT_R16
    ),

    DETEX_TEXTURE_FORMAT_EAC_SIGNED_R11 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_EAC_SIGNED_R11 << 24) |
        DETEX_PIXEL_FORMAT_SIGNED_R16
    ),

    DETEX_TEXTURE_FORMAT_EAC_RG11 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_EAC_RG11 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RG16
    ),

    DETEX_TEXTURE_FORMAT_EAC_SIGNED_RG11 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_EAC_SIGNED_RG11 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_SIGNED_RG16
    ),

    DETEX_TEXTURE_FORMAT_ASTC_4X4 = (
        (DETEX_COMPRESSED_TEXTURE_FORMAT_INDEX_ASTC_4X4 << 24) |
        DETEX_TEXTURE_FORMAT_128BIT_BLOCK_BIT |
        DETEX_PIXEL_FORMAT_RGBA8
    ),
}
