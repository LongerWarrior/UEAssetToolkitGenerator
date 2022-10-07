using System.Runtime.InteropServices;

namespace FBXLib;

public class Interface
{
    [DllImport("FBX_Wrapper.dll", EntryPoint = "CreateFBXManager")]
    public static extern IntPtr CreateFBXManager();
    [DllImport("FBX_Wrapper.dll", EntryPoint = "CreateFBXImporter")]
    public static extern IntPtr CreateFBXImporter(IntPtr manager, string fileName);
    
    
}