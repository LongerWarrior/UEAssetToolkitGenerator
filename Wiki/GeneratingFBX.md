# Generating FBX files
For asset types requiring `.fbx` files, CAS has two parts:
1. Reads the cooked asset and converts it into a `.fbx` file
2. Reads the cooked asset and the generated `.fbx` file to create the serialized JSON
The asset generator will then use the model hash from the JSON to import the FBX file.

The following asset types need `.fbx` files to be generated to work:
- Static Mesh
- Skeletal Mesh
- Animation Sequence

Currently, only the static meshes are converted directly from `.uasset` -> `.fbx` by my [FBX-Wrapper](https://github.com/Buckminsterfullerene02/FBX-Wrapper) tool. Although code for skeletons and skeletal meshes is there, the skeletons are messed up, at the time of writing. Hopefully at some point they can be fixed so that skeletal meshes can be generated directly like this too.

## Generating SKM/AnimSeq FBX
To obtain skeletal meshes and animation sequences, you will need to follow a more complex process.

1. Use the animation part from [this](https://gist.github.com/Buckminsterfullerene02/789fb38a2f1ccd2ef55262a90be578d9) UModel command to extract all `.psk`/`.psa` (skeletal mesh/animation sequence) files
2. Use 3DS Max (*cough* probably has to be obtained in a *cough* non-official way *cough*) to convert `.psk`/`.psa` to `.fbx` - can semi-automate using [this](https://gist.github.com/Buckminsterfullerene02/12947999641c6a290f2cbbaf4e0ee313) batch export script
3. Copy `.fbx` files into JSON output and serialize skeletal meshes & animation sequences so that they get model hash 

**Warning:** At the time of writing, the animation sequence

## Backup method for generating SM FBX
Sometimes, some static meshes are generated improperly by my FBX Wrapper tool (for varying reasons), so there is a backup method for bulk exporting SMs too.

1. Use the static mesh part from [this](https://gist.github.com/Buckminsterfullerene02/789fb38a2f1ccd2ef55262a90be578d9) UModel command to extract all `.glTF` files
2. Run [this](https://gist.github.com/Buckminsterfullerene02/6b49374b8a8da0d992e73a22c9e0d7dc) C# program (can easily be converted into Python or similar if you want) to generate Blender script for each of the extracted `.glTF` files
3. Open the Blender script in Blender and run it (Blender will freeze for a bit)

