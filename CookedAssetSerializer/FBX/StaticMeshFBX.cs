using System.Runtime.InteropServices;
using UAssetAPI;
using UAssetAPI.StructTypes.StaticMesh;

namespace CookedAssetSerializer.FBX;

public class StaticMeshFBX
{
    public struct FStaticMeshStruct
    {
        public string Name;
        public FStaticMeshRenderData RenderData;
        public FStaticMaterial[] StaticMaterials;
    }

    public StaticMeshFBX(FStaticMeshStruct meshStruct, string path, bool bExportAsText, ref string error)
    {
        string json = JsonConvert.SerializeObject(meshStruct, Formatting.Indented);
        // output json as file at path
        //File.WriteAllText(Path.ChangeExtension(path, "json"), json);
        ExportStaticMeshIntoFbxFile(json, path, bExportAsText, ref error);
    }

    [DllImport(@"F:\Github Projects\Other\UEAssetToolkitGenerator\FBX-Wrapper\cmake-build-debug\FBX_Wrapper.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void ExportStaticMeshIntoFbxFile(string JSONStaticMeshData, string OutFileName,
        bool bExportAsText, ref string OutErrorMessage); // TODO: Probably need to get error message seperately, don't worry about this right now
}