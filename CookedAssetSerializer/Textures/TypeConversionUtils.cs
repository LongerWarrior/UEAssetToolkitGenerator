using static System.BitConverter;

namespace Utils;

public static class TypeConversionUtils
{
    public static float HalfToFloat(ushort fp16)
    {
        const uint shiftedExp = 0x7c00 << 13;		// exponent mask after shift
        var magic = Int32BitsToSingle(113 << 23);

        var fp32 = (fp16 & 0x7fff) << 13;			// exponent/mantissa bits
        var exp = shiftedExp & fp32;				// just the exponent
        fp32 += (127 - 15) << 23;					// exponent adjust

        // handle exponent special cases
        if (exp == shiftedExp)						// Inf/NaN?
        {
            fp32 += (128 - 16) << 23;				// extra exp adjust
        }
        else if (exp == 0)							// Zero/Denormal?
        {
            fp32 += 1 << 23;						// extra exp adjust
            fp32 = SingleToInt32Bits(Int32BitsToSingle(fp32) - magic); // renormalize
        }

        fp32 |= (fp16 & 0x8000) << 16;				// sign bit
        var halfToFloat = Int32BitsToSingle(fp32);
        return halfToFloat;
    }

    public static byte Requantize16to8(int value16) {
        if (value16 is < 0 or > 65535) {
            throw new ArgumentException(nameof(value16));
        }

        // Dequantize x from 16 bit (Value16/65535.f)
        // then requantize to 8 bit with rounding (GPU convention UNorm)

        // matches exactly with :
        //  (int)( (Value16/65535.f) * 255.f + 0.5f );
        var value8 = (value16 * 255 + 32895) >> 16;
        return (byte)value8;
    }

    public static int Clamp(this int i, int min, int max) => i < min ? min : i < max ? i : max;
    public static float Clamp(this float f, float min, float max) => f < min ? min : f < max ? f : max;
    public static int FloorToInt(this float f) => Math.Floor(f).TruncToInt();
    public static int TruncToInt(this float f) => (int)f;
    public static int TruncToInt(this double f) => (int)f;
}