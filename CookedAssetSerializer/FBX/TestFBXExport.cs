using System.Drawing;
using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Shading;
using Aspose.ThreeD.Utilities;
using Textures;
using UAssetAPI.StructTypes.StaticMesh;
using FVector4 = UAssetAPI.FVector4;

namespace CookedAssetSerializer.FBX;

public class TestFBXExport
{
    private Scene GetSceneInfo(Scene scene)
    {
        scene.AssetInfo.Title = "FBX Exporter";
        scene.AssetInfo.Author = "Buckminsterfullerene";
        scene.AssetInfo.Comment = "We do not take any responsibility for misuse of this wrapper.";
        scene.AssetInfo.ApplicationName = "UEAssetToolkitGenerator";
        
        scene.AssetInfo.UpVector = Axis.ZAxis;
        scene.AssetInfo.CoordinatedSystem = CoordinatedSystem.LeftHanded;
        scene.AssetInfo.UnitName = "cm";
        scene.AssetInfo.UnitScaleFactor = 1.0f;
        
        return scene;
    }
    
    private void ExportDummyMaterialIntoFbxScene(string materialSlotName, ref Node node)
    {
        Material material = new LambertMaterial();
        material.Name = materialSlotName;
        node.Material = material;
    }

    private void ExportStaticMeshResources(FStaticMeshVertexBuffer vertexBuffer,
        FPositionVertexBuffer positionVertexBuffer, FColorVertexBuffer colorVertexBuffer, ref Mesh mesh)
    {
        // Initialize vertices first
        int vertexCount = positionVertexBuffer.NumVertices;
        mesh.ControlPoints.AddRange(new Vector4[vertexCount]);
        for (int i = 0; i < vertexCount; i++)
        {
            FVector srcPosition = positionVertexBuffer.Verts[i];
            mesh.ControlPoints[i] = FBXDataConverter.ConvertFVectorToVector4(srcPosition);
        }
        
        // Initialize vertex colors (if we have any)
        if (colorVertexBuffer.NumVertices > 0)
        {
            if (colorVertexBuffer.NumVertices != vertexCount)
                throw new Exception("Vertex color count does not match vertex count!");
            
            VertexElementVertexColor colorElement = mesh.CreateElement(
                VertexElementType.VertexColor, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementVertexColor;
            Vector4[] colors = new Vector4[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = FBXDataConverter.ConvertColorToVector4(colorVertexBuffer.Data[i]);
            }
            colorElement.SetData(colors); // TODO: Supposedly, this is referencing mesh, so this should work?
        }
        
        // Initialise normals
        VertexElementNormal normalElement = mesh.CreateElement(
            VertexElementType.Normal, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementNormal;
        for (int i = 0; i < vertexCount; i++)
        {
            FVector4 srcNormal = vertexBuffer.UV[i].Normal[2]; // VertexTangentZ
            normalElement.Data.Add(FBXDataConverter.ConvertFVectorToVector4(srcNormal));
        }
        
        // Initialise tangents
        VertexElementTangent tangentElement = mesh.CreateElement(
            VertexElementType.Tangent, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementTangent;
        for (int i = 0; i < vertexCount; i++)
        {
            FVector4 srcNormal = vertexBuffer.UV[i].Normal[0]; // VertexTangentX
            tangentElement.Data.Add(FBXDataConverter.ConvertFVectorToVector4(srcNormal));
        }
        
        // Initialise binormals
        VertexElementBinormal binormalElement = mesh.CreateElement(
            VertexElementType.Binormal, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementBinormal;
        for (int i = 0; i < vertexCount; i++)
        {
            FVector4 srcNormal = vertexBuffer.UV[i].Normal[1]; // VertexTangentY
            binormalElement.Data.Add(FBXDataConverter.ConvertFVectorToVector4(srcNormal));
        }
        
        // Initialise UVs
        VertexElementUV uvElement = mesh.CreateElement(
            VertexElementType.UV, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementUV;
        for (int i = 0; i < vertexBuffer.NumVertices; i++)
        {
            // TODO: proper names, can know if texture is lightmap by checking lightmap tex coord index from static mesh
            uvElement.Name = i == 0 ? "uv" : "uv" + i;
            for (int j = 0; j < vertexBuffer.NumTexCoords; j++)
            {
                FMeshUVFloat srcUV = vertexBuffer.UV[i].UV[j];
                uvElement.Data.Add(FBXDataConverter.ConvertUVToVector4(srcUV));
            }
        }
    }

    private Mesh ExportStaticMesh(StaticMeshFBX.FStaticMeshStruct Data, bool bGenSmothingGroups, Node meshNode)
    {
        Mesh mesh = new Mesh();
        
        // Create basic static mesh buffers
        // TODO: Should we have an option for creating a new SM for every LOD?
        FRawStaticIndexBuffer indexBuffer = Data.RenderData.LODs[0].IndexBuffer;
        ExportStaticMeshResources(Data.RenderData.LODs[0].VertexBuffer, Data.RenderData.LODs[0].PositionVertexBuffer,
            Data.RenderData.LODs[0].ColorVertexBuffer, ref mesh);
        
        // Create sections and initialize dummy materials
        foreach (var meshSection in Data.RenderData.LODs[0].Sections)
        {
            int numTriangles = meshSection.NumTriangles;
            int startIndex = meshSection.FirstIndex;
            
            // Create dummy material for this section
            string materialSlotName = Data.StaticMaterials[meshSection.MaterialIndex].MaterialSlotName.ToString();
            ExportDummyMaterialIntoFbxScene(materialSlotName, ref meshNode);
            
            // Add all triangles associated with this section
            // TODO: Will this work to create the triangles of the polygons rather than commiting the indices directly?
            bool bIs32Bit = indexBuffer.Indices16.Length == 0;
            if (bIs32Bit) mesh.CreatePolygon(FBXDataConverter.ConvertUIntToInt(indexBuffer.Indices32));
            else mesh.CreatePolygon(FBXDataConverter.ConvertUShortToInt(indexBuffer.Indices16));
        }

        if (bGenSmothingGroups)
        {
            VertexElementSmoothingGroup smoothingGroup = mesh.CreateElement(
                VertexElementType.SmoothingGroup, MappingMode.ControlPoint, ReferenceMode.Direct) as VertexElementSmoothingGroup;
            // TODO
        }
        
        return mesh;
    }

    public void ExportStaticMeshIntoFbxFile(StaticMeshFBX.FStaticMeshStruct Data, bool bExportAsText, bool bGenSmothingGroups, 
        string fileName)
    {
        Scene scene = GetSceneInfo(new Scene());

        Node meshNode = new Node(Data.Name);
        meshNode.Entity = ExportStaticMesh(Data, bGenSmothingGroups ,meshNode);
        scene.RootNode.AddChildNode(meshNode);

        // Export the scnee into fbx file
        if (bExportAsText) scene.Save(fileName, FileFormat.FBX7700ASCII);
        else scene.Save(fileName, FileFormat.FBX7700Binary);
    }
}