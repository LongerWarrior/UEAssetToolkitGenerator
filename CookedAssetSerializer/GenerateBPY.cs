namespace CookedAssetSerializer;

public class GenerateBPY
{
    public GenerateBPY()
    {
        GenerateSM();
    }

    private void GenerateSM()
    {
        var InputPath = @"F:\DRG Modding\DRGPacker\JSON\SMs\";
        var outputPath = @"F:\DRG Modding\DRGPacker\JSON\Assets\Game\";
        var allfiles = Directory.GetFiles(InputPath, "*.gltf", SearchOption.AllDirectories);

        var command = "import bpy\n\n";
        for (var i = 0; i < allfiles.Length; i++)
        {
            // model already converted?
            if (File.Exists((outputPath + allfiles[i].Remove(0, InputPath.Length)).Replace(".gltf", ".fbx"))) continue;
            var InputFile = allfiles[i];
            var OutputFile = (outputPath + allfiles[i].Remove(0, InputPath.Length)).Replace(".gltf", ".fbx");
            var folder = Path.GetDirectoryName(outputPath + allfiles[i].Remove(0, InputPath.Length));
            // create output directory
            Directory.CreateDirectory(folder);

            command += "bpy.ops.import_scene.gltf(filepath = \"" + InputFile.Replace("\\", "\\\\") +
                       "\", files =[{ \"name\":\"" + Path.GetFileName(InputFile) + "\", \"name\":\"" +
                       Path.GetFileName(InputFile) + "\"}], loglevel = 50)\n";
            command += "bpy.ops.export_scene.fbx(filepath=\"" + OutputFile.Replace("\\", "\\\\") + "\")\n";
            command += "bpy.ops.object.select_all(action='SELECT')\n";
            command += "bpy.ops.object.delete(use_global=False, confirm=False)\n\n";
        }

        File.WriteAllText(@"F:\DRG Modding\DRGPacker\JSON\BlenderCommands\commands.py", command);
    }

    private void GenerateAnims()
    {
        var inputPath = @"F:\DRG Modding\DRGPacker\JSON\Anims\";
        var outputPath = @"F:\DRG Modding\DRGPacker\JSON\Assets\Game\";
        
        // We can use the props.txt for each anim to get the names of the skeletons, which are serialized
        // Then use the serialized skeleton to find its skeletal mesh and map the anim to that mesh
        // Then export data to JSON ready for import into whatever is using it
    }
}