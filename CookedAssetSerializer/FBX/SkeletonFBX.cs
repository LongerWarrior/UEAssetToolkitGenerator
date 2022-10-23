using System.Runtime.InteropServices;

namespace CookedAssetSerializer.FBX;

public class SkeletonFBX
{
    public struct FSkeletonStruct {
        public FReferenceSkeleton Skeleton;
    }
    
    public SkeletonFBX(FSkeletonStruct meshStruct, string path, bool bExportAsText, ref string error)
    {
        string json = JsonConvert.SerializeObject(meshStruct);

        // output json as file at path
        //File.WriteAllText(Path.ChangeExtension(path, "json"), json);
        ExportSkeletonIntoFbxFile(json, path, bExportAsText, ref error);
    }

    [DllImport(@"F:\Github Projects\Other\UEAssetToolkitGenerator\FBX-Wrapper\cmake-build-release\FBX_Wrapper.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void ExportSkeletonIntoFbxFile(string JSONSkeletonData, string OutFileName,
        bool bExportAsText, ref string OutErrorMessage);
}