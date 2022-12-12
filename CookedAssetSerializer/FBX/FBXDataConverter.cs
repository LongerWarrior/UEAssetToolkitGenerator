using System.Drawing;
using Aspose.ThreeD.Utilities;
using UAssetAPI.StructTypes.StaticMesh;
using FVector4 = UAssetAPI.FVector4;

namespace CookedAssetSerializer.FBX;

public class FBXDataConverter
{
    public static Vector4 ConvertFVectorToVector4(FVector vector)
    {
        return new Vector4(vector.X, -vector.Y, vector.Z, 1);
    }
    
    public static Vector4 ConvertFVectorToVector4(FVector4 vector)
    {
        return new Vector4(vector.X, -vector.Y, vector.Z, 1);
    }

    public static Vector4 ConvertColorToVector4(Color color)
    {
        return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }
    
    public static Vector4 ConvertUVToVector4(FMeshUVFloat uv)
    {
        return new Vector4(uv.U, 1 - uv.V, 0.0, 0.0);
    }
    
    public static int[] ConvertUShortToInt(ushort[] array)
    {
        int[] result = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = array[i];
        }
        return result;
    }

    public static int[] ConvertUIntToInt(uint[] array)
    {
        int[] result = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = (int)array[i];
        }
        return result;
    }
}