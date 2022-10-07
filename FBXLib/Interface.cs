using System.Runtime.InteropServices;

namespace FBXLib;

public class Interface
{
    [DllImport("FBX_Wrapper.dll", CallingConvention=CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
    public static extern void CreateFBXManager();
    [DllImport("FBX_Wrapper.dll", CallingConvention=CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
    public static extern void CreateFBXImporter();
}