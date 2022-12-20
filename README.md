# UEAssetToolkitGenerator 
This is a work-in-progress .NET API/GUI to generate [UEAssetToolkit](https://github.com/Buckminsterfullerene02/UEAssetToolkit-Fixes) compatible JSON files. You must have .NET 6 installed to use. Also known as Cooked Asset Serializer (CAS).

## Usage
1. Use the Settings table below to configure the generator (or load a config settings file).
2. Click the `Scan Assets` button to generate the AllTypes and AssetTypes files that helps you to fill in the `Simple assets` and `Cooked assets to copy` fields.
3. Click the `Move Cooked Assets` button to copy your specified cooked assets.
4. Click the `Serialize Assets` button to generate the JSON files.
5. Save your settings by clicking the `Save Config Settings` button if you have not done so already.

If you aren't satisfied with just copying the cooked Static Mesh (which can still be dragged into the level), the current (temporary) process of obtaining the serialized static meshes is as follows:
1. Download [UModel](https://www.gildor.org/en/projects/umodel)
2. Edit and run [this](https://gist.github.com/Buckminsterfullerene02/a9c9a19ddb573fcff78b2e31586383ad) in command prompt 
3. Edit and run [this](https://gist.github.com/Buckminsterfullerene02/a4a0e62066d09a17315a5191b4e41186) simple C# application to generate the blender script
4. Open blender and paste the script into the script editor, and run it
5. Run CAS to serialize the FBX and cooked assets into JSON

## Credits
- LongerWarrior for almost all of the serialization code and modifying UAAPI/CUE4Parse to add new export types. 
- Archengius for developing the UEAssetToolkit in the first place.
- atenfyr for developing UAAPI/GUI which CAS builds on.
- CUE4Parse for having support for most asset types which CAS modifies for the UAAPI format - we hope in the future to drop UAAPI and purely base on CUE4Parse.
- Buckminsterfullerene for CAS GUI and various fixes/improvements.
- Narknon for bits and bobs with the parse tree and GUI changes, & helping out with testing. 

*We are not responsible for any misuse of this tool.*