using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using SkiaSharp;
using Textures.ASTC;
using Textures.BC;
using Textures.DXT;
using static Utils.TypeConversionUtils;
using static System.MathF;
using System.Runtime.InteropServices;

namespace Textures;
public static class TextureDecoder
{

    public static SKBitmap Decode(this Texture2DExport texture, FTexture2DMipMap mip, int slices,out string hash,bool srgb,bool iscube = false) {
        hash = "";
        if (!texture.IsVirtual && mip != null) {
            byte[] data;
            SKColorType colorType;

            DecodeTexture(mip, texture.Format, texture.isNormalMap, out data, out colorType,srgb);

            if (colorType == SKColorType.Rgba8888 || colorType == SKColorType.Rgb888x) {
                for (int i = 0; i < data.Length / 4; i++) {
                    var temp = data[i * 4];
                    data[i * 4] = data[i * 4 + 2];
                    data[i * 4 + 2] = temp;
                    if (iscube) {
                        data[i * 4 + 3] = 255;
                    }
                    if (texture.isNormalMap) {
                        data[i * 4] = 0;
                    }
                }
                

                var md5 = MD5.Create();
                hash = md5.ComputeHash(data, 0, mip.SizeX * mip.SizeY * 4).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);

                for (int i = 0; i < data.Length / 4; i++) {
                    var temp = data[i * 4];
                    data[i * 4] = data[i * 4 + 2];
                    data[i * 4 + 2] = temp;
                }
            }
            if (colorType == SKColorType.Bgra8888) {
                for (int i = 0; i < data.Length / 4; i++) {
                    if (iscube) {
                        data[i * 4 + 3] = 255;
                    }
                    if (texture.isNormalMap) {
                        data[i * 4] = 0;
                    }
                }
                var md5 = MD5.Create();
                hash = md5.ComputeHash(data, 0, mip.SizeX * mip.SizeY * 4).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            }

            if (colorType == SKColorType.Rgb565) {
                var md5 = MD5.Create();
                hash = md5.ComputeHash(data, 0, mip.SizeX * mip.SizeY*2).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            }

            if (colorType == SKColorType.Gray8) {
                var md5 = MD5.Create();
                hash = md5.ComputeHash(data, 0, mip.SizeX * mip.SizeY).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            }

            var width = mip.SizeX;
            var height = mip.SizeY;
            
            var info = new SKImageInfo(width, height, colorType, SKAlphaType.Unpremul);
            var bitmap = new SKBitmap(info);

            unsafe {
                var pixelsPtr = NativeMemory.Alloc((nuint)data.Length);
                fixed (byte* p = data) {
                    Unsafe.CopyBlockUnaligned(pixelsPtr, p, (uint)data.Length);
                }

                bitmap.InstallPixels(info, new IntPtr(pixelsPtr), info.RowBytes, (address, _) => NativeMemory.Free(address.ToPointer()));
            }

            if (!texture.bRenderNearestNeighbor) {
                return bitmap;
            }

            var resized = bitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.None);
            bitmap.Dispose();
            return resized;
        }
        return null;
    }

    public static void DecodeTexture(FTexture2DMipMap mip, EPixelFormat format, bool isNormalMap, out byte[] data, out SKColorType colorType, bool srgb)
    {
        switch (format)
        {
            case EPixelFormat.PF_DXT1:
                data = DXTDecoder.DXT1(mip.Data.Data, mip.SizeX, mip.SizeY, mip.SizeZ);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_DXT5:
                data = DXTDecoder.DXT5(mip.Data.Data, mip.SizeX, mip.SizeY, mip.SizeZ);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_ASTC_4x4:
            case EPixelFormat.PF_ASTC_6x6:
            case EPixelFormat.PF_ASTC_8x8:
            case EPixelFormat.PF_ASTC_10x10:
            case EPixelFormat.PF_ASTC_12x12:
                data = ASTCDecoder.RGBA8888(
                    mip.Data.Data,
                    FormatHelper.GetBlockWidth(format),
                    FormatHelper.GetBlockHeight(format),
                    FormatHelper.GetBlockDepth(format),
                    mip.SizeX, mip.SizeY, mip.SizeZ);
                colorType = SKColorType.Rgba8888;

                if (isNormalMap)
                {
                    // UE4 drops blue channel for normal maps before encoding, restore it
                    unsafe
                    {
                        var offset = 0;
                        fixed (byte* d = data)
                        {
                            for (var i = 0; i < mip.SizeX * mip.SizeY; i++)
                            {
                                d[offset + 2] = BCDecoder.GetZNormal(d[offset], d[offset + 1]);
                                offset += 4;
                            }
                        }
                    }
                }

                break;
            case EPixelFormat.PF_BC4:
                data = BCDecoder.BC4(mip.Data.Data, mip.SizeX, mip.SizeY);
                colorType = SKColorType.Rgb888x;
                break;
            case EPixelFormat.PF_BC5:
                data = BCDecoder.BC5(mip.Data.Data, mip.SizeX, mip.SizeY);
                colorType = SKColorType.Rgb888x;
                break;
            case EPixelFormat.PF_BC6H:
                // BC6H doesn't work no matter the pixel format, the closest we can get is either
                // Rgb565 DETEX_PIXEL_FORMAT_FLOAT_RGBX16 or Rgb565 DETEX_PIXEL_FORMAT_FLOAT_BGRX16

                data = Detex.DecodeDetexLinear(mip.Data.Data, mip.SizeX, mip.SizeY, true,
                    DetexTextureFormat.DETEX_TEXTURE_FORMAT_BPTC_FLOAT,
                    DetexPixelFormat.DETEX_PIXEL_FORMAT_FLOAT_RGBX16);
                colorType = SKColorType.Rgb565;
                break;
            case EPixelFormat.PF_BC7:
                data = Detex.DecodeDetexLinear(mip.Data.Data, mip.SizeX, mip.SizeY, false,
                    DetexTextureFormat.DETEX_TEXTURE_FORMAT_BPTC,
                    DetexPixelFormat.DETEX_PIXEL_FORMAT_RGBA8);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_ETC1:
                data = Detex.DecodeDetexLinear(mip.Data.Data, mip.SizeX, mip.SizeY, false,
                    DetexTextureFormat.DETEX_TEXTURE_FORMAT_ETC1,
                    DetexPixelFormat.DETEX_PIXEL_FORMAT_RGBA8);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_ETC2_RGB:
                data = Detex.DecodeDetexLinear(mip.Data.Data, mip.SizeX, mip.SizeY, false,
                    DetexTextureFormat.DETEX_TEXTURE_FORMAT_ETC2,
                    DetexPixelFormat.DETEX_PIXEL_FORMAT_RGBA8);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_ETC2_RGBA:
                data = Detex.DecodeDetexLinear(mip.Data.Data, mip.SizeX, mip.SizeY, false,
                    DetexTextureFormat.DETEX_TEXTURE_FORMAT_ETC2_EAC,
                    DetexPixelFormat.DETEX_PIXEL_FORMAT_RGBA8);
                colorType = SKColorType.Rgba8888;
                break;
            case EPixelFormat.PF_R16F:
            case EPixelFormat.PF_R16F_FILTER:
            case EPixelFormat.PF_G16:
                unsafe
                {
                    fixed (byte* d = mip.Data.Data)
                    {
                        data = ConvertRawR16DataToRGB888X(mip.SizeX, mip.SizeY, d, mip.SizeX * 2); // 2 BPP
                    }
                }

                colorType = SKColorType.Rgb888x;
                break;
            case EPixelFormat.PF_B8G8R8A8:
                data = mip.Data.Data;
                colorType = SKColorType.Bgra8888;
                break;
            case EPixelFormat.PF_G8:
                data = mip.Data.Data;
                colorType = SKColorType.Gray8;
                break;
            case EPixelFormat.PF_FloatRGBA:
                unsafe
                {
                    fixed (byte* d = mip.Data.Data)
                    {
                        data = ConvertRawR16G16B16A16FDataToRGBA8888(mip.SizeX, mip.SizeY, d, mip.SizeX * 8, true, srgb); // 8 BPP
                    }
                }

                colorType = SKColorType.Rgba8888;
                break;
            default: throw new NotImplementedException($"Unknown pixel format: {format}");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe byte[] ConvertRawR16DataToRGB888X(int width, int height, byte* inp, int srcPitch)
    {
        // e.g. shadow maps
        var ret = new byte[width * height * 4];
        for (int y = 0; y < height; y++)
        {
            var srcPtr = (ushort*) (inp + y * srcPitch);
            var destPtr = y * width * 4;
            for (int x = 0; x < width; x++)
            {
                var value16 = *srcPtr++;
                var value = Requantize16to8(value16);

                ret[destPtr++] = value;
                ret[destPtr++] = value;
                ret[destPtr++] = value;
                ret[destPtr++] = 255;
            }
        }

        return ret;
    }

    public readonly struct FColor  {
        public readonly byte B;
        public readonly byte G;
        public readonly byte R;
        public readonly byte A;


        public FColor(byte r, byte g, byte b, byte a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
    public struct FLinearColor  {
        public float R;
        public float G;
        public float B;
        public float A;


        public FLinearColor(float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public FColor ToFColor(bool sRGB) {
            var floatR = R.Clamp(0.0f, 1.0f);
            var floatG = G.Clamp(0.0f, 1.0f);
            var floatB = B.Clamp(0.0f, 1.0f);
            var floatA = A.Clamp(0.0f, 1.0f);

            if (sRGB) {
                floatR = floatR <= 0.0031308f ? floatR * 12.92f : Pow(floatR, 1.0f / 2.4f) * 1.055f - 0.055f;
                floatG = floatG <= 0.0031308f ? floatG * 12.92f : Pow(floatG, 1.0f / 2.4f) * 1.055f - 0.055f;
                floatB = floatB <= 0.0031308f ? floatB * 12.92f : Pow(floatB, 1.0f / 2.4f) * 1.055f - 0.055f;
            }

            var intA = (floatA * 255.999f).FloorToInt();
            var intR = (floatR * 255.999f).FloorToInt();
            var intG = (floatG * 255.999f).FloorToInt();
            var intB = (floatB * 255.999f).FloorToInt();

            return new FColor((byte)intR, (byte)intG, (byte)intB, (byte)intA);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe byte[] ConvertRawR16G16B16A16FDataToRGBA8888(int width, int height, byte* inp, int srcPitch, bool linearToGamma, bool srgb)
    {
        float minR = 0.0f, minG = 0.0f, minB = 0.0f, minA = 0.0f;
        float maxR = 1.0f, maxG = 1.0f, maxB = 1.0f, maxA = 1.0f;

        for (int y = 0; y < height; y++)
        {
            var srcPtr = (ushort*) (inp + y * srcPitch);

            for (int x = 0; x < width; x++)
            {
                minR = Min(HalfToFloat(srcPtr[0]), minR);
                minG = Min(HalfToFloat(srcPtr[1]), minG);
                minB = Min(HalfToFloat(srcPtr[2]), minB);
                minA = Min(HalfToFloat(srcPtr[3]), minA);
                maxR = Max(HalfToFloat(srcPtr[0]), maxR);
                maxG = Max(HalfToFloat(srcPtr[1]), maxG);
                maxB = Max(HalfToFloat(srcPtr[2]), maxB);
                maxA = Max(HalfToFloat(srcPtr[3]), maxA);
                srcPtr += 4;
            }
        }

        var ret = new byte[width * height * 4];
        for (int y = 0; y < height; y++)
        {
            var srcPtr = (ushort*) (inp + y * srcPitch);
            var destPtr = y * width * 4;

            for (int x = 0; x < width; x++)
            {
                var color = new FLinearColor(
                    (HalfToFloat(*srcPtr++) - minR) / (maxR - minR),
                    (HalfToFloat(*srcPtr++) - minG) / (maxG - minG),
                    (HalfToFloat(*srcPtr++) - minB) / (maxB - minB),
                    (HalfToFloat(*srcPtr++) - minA) / (maxA - minA)
                ).ToFColor(linearToGamma);
                if (srgb) {
                    ret[destPtr++] = (byte)(2 * color.R);
                    ret[destPtr++] = (byte)(2 * color.G);
                    ret[destPtr++] = (byte)(2 * color.B);
                    ret[destPtr++] = color.A;
                } else {
                    ret[destPtr++] = color.R;
                    ret[destPtr++] = color.G;
                    ret[destPtr++] = color.B;
                    ret[destPtr++] = color.A;
                }

            }
        }

        return ret;
    }
}
