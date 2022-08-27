namespace UAssetAPI.StructTypes;

public enum EFontHinting : byte {
    /** Use the default hinting specified in the font. */
    Default,
    /** Force the use of an automatic hinting algorithm. */
    Auto,
    /** Force the use of an automatic light hinting algorithm, optimized for non-monochrome displays. */
    AutoLight,
    /** Force the use of an automatic hinting algorithm optimized for monochrome displays. */
    Monochrome,
    /** Do not use hinting. */
    None,
}
