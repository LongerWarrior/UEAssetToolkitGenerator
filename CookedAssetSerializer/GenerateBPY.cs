namespace CookedAssetSerializer;

public class GenerateBPY
{
    public GenerateBPY()
    {
        GenerateSM();
    }

    private void GenerateSM()
    {
        var InputPath = @"F:\Railgrade Modding\UnrealPacker4.27\JSON\AnimStuff\SMs\";
        var outputPath = @"F:\Railgrade Modding\UnrealPacker4.27\JSON\Assets\Game\";
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

        File.WriteAllText(@"F:\Railgrade Modding\UnrealPacker4.27\JSON\AnimStuff\BlenderCommands\commands.py", command);
    }
}