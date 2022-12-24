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
    
    public StaticMeshFBX(FStaticMeshStruct meshStruct, string path, bool bExportAsText, ref string error, ref bool tooLarge)
    {
        string json = null;
        try
        {
            json = JsonConvert.SerializeObject(meshStruct);
        }
        catch (Exception e)
        {
            Log.Error($"[Static Mesh FBX]: Failed to create JSON string! {e}");
            if (e.GetType().IsAssignableFrom(typeof(OutOfMemoryException))) tooLarge = true;
            return;
        }
        
        // if json string is longer than 20 million characters (roughly 3.5mins completetion time), skip it
        if (json.Length > 20000000) // TODO: Make this a setting
        {
            error = $"Mesh is too large to export to FBX. " +
                    $"Estimated time to complete using this method: {ConvertLengthToTime(json.Length)} minutes. " +
                    $"Use another method to export this mesh.";
            tooLarge = true;
            return;
        }
        
        //File.WriteAllText(Path.ChangeExtension(path, "json"), json); // This is for debugging
        
        try
        {
            ExportStaticMeshIntoFbxFile(json, path, bExportAsText, ref error);
        }
        catch (Exception e)
        {
            Log.Error($"[Static Mesh FBX]: Failed to export SM! {e}");
            return;
        }
    }

    public float ConvertLengthToTime(int length)
    {
        // length of 100 million takes 17.5 minutes
        // so 1 million is 10.5 seconds
        return (float)(length / 1000000 * 10.5) / 60;
    }

    [DllImport(@"FBX_Wrapper", CallingConvention = CallingConvention.Cdecl)]
    static extern void ExportStaticMeshIntoFbxFile(string JSONStaticMeshData, string OutFileName,
        bool bExportAsText, ref string OutErrorMessage);
}