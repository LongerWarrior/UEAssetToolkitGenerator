using System.Runtime.InteropServices;

namespace UAssetAPI;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FCompressedSegment
{
    public int StartFrame;
    public int NumFrames;
    public int ByteStreamOffset;
    public AnimationCompressionFormat TranslationCompressionFormat;
    public AnimationCompressionFormat RotationCompressionFormat;
    public AnimationCompressionFormat ScaleCompressionFormat;
}
