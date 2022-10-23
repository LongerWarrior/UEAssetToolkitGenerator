using System.Runtime.InteropServices;

namespace CookedAssetSerializer.FBX;

public class SkeletonFBX
{
    public struct FSkeletonStruct {
        public FReferenceSkeleton Skeleton;
    }
    
    public SkeletonFBX(FSkeletonStruct meshStruct, string path, bool bExportAsText, ref string error, ref bool tooLarge)
    {
        string json = JsonConvert.SerializeObject(meshStruct, Formatting.Indented);
        // if json string is longer than 20 million characters (roughly 3.5mins completetion time), skip it
        if (json.Length > 20000000) // TODO: Make this a setting
        {
            error = $"Skeleton is too large to export to FBX. " +
                    $"Estimated time to complete using this method: {ConvertLengthToTime(json.Length)} minutes. " +
                    $"Use another method to export this mesh.";
            tooLarge = true;
            return;
        }
        
        // output json as file at path
        //File.WriteAllText(Path.ChangeExtension(path, "json"), json);
        ExportSkeletonIntoFbxFile(json, path, bExportAsText, ref error);
    }

    public float ConvertLengthToTime(int length)
    {
        // length of 100 million takes 17.5 minutes
        // so 1 million is 10.5 seconds
        return (float)(length / 1000000 * 10.5) / 60;
    }
    
    [DllImport(@"F:\Github Projects\Other\UEAssetToolkitGenerator\FBX-Wrapper\cmake-build-release\FBX_Wrapper.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void ExportSkeletonIntoFbxFile(string JSONSkeletonData, string OutFileName,
        bool bExportAsText, ref string OutErrorMessage);
}