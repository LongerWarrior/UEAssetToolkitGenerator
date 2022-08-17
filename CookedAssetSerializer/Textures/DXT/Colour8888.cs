using System.Runtime.InteropServices;

namespace Textures.DXT;

[StructLayout(LayoutKind.Sequential)]
public struct Colour8888
{
    public byte Red;
    public byte Green;
    public byte Blue;
    public byte Alpha;
}
