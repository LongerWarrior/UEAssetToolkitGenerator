# Using the GUI
You should understand exactly how the GUI works before you attempt to use this tool. The following sections should be loosely followed in order.

## Setting up the main paths and configs
The first thing you need to do, is setup your main settings to tell the tool where to look for files and the engine version.

![MainSettingsNumbered](..\Wiki\Images\MainSettingsNumbered.png)

The labels to the right of the text boxes are actually buttons, so if you click them it will open a file/directory dialog box.

1. Path to unpacked Content folder.
2. Path to AssetRegistry.bin file in unpacked files.
3. Path to DefaultGame.ini config file in unpacked files.
4. Path to where you want the serialized JSON files to end up. It will make a new folder called `Game` and put the files inside of that.
5. Path to where you want your log/AssetTypes/AllTypes files to be stored.
6. A collapsable tree to select the exact assets/directories within path 1 that you would like to parse. If you want to ignore this, you can simply tick all of the top level directory.
7. The UE version that your game uses.
8. Whether or not you want to overwrite your existing JSON files with new ones.

## General settings/operations
These widgets are to do with loading/saving your configs, and other quality of life things.

![GeneralSettingsNumbered](..\Wiki\Images\GeneralSettingsNumbered.png)

1. Check this if you want the program to automatically load up with your most recently saved configuration profile (if it exists).
2. Check this if you don't want it to show the "Do you want to save before exit" prompt when you close the program.
3. Opens the AllTypes.txt file, which is generated after you scan your assets. It is stored within your Info Dir.
4. Opens the AssetTypes.json file, which is generated after you scan your assets. It is stored within your Info Dir.
5. Save your current configs profile.
6. Load a new configs profile.
7. Open the output_logs.txt file, if it exists. It is stored within your Info Dir.
8. Clear the output_logs.txt file, if it exists. It is stored within your Info Dir.

## Serialization settings
These widgets control which asset types you would like to serialize, which ones you want to skip, simple assets, etc.

![SerializationSettingsNumbered](..\Wiki\Images\AssetSettingsNumbered.png)

1. Once you have scanned the assets (see step 1. below in running the program section), you will get a list of all of the asset types in the AllTypes.txt file. You need to use this list, as well as the given defaults, to construct your list of simple assets. The general rule of thumb, is that any custom assets from the game (i.e. ones that start with `/Script/<GameName>.`) should be included in this list. This is because any **data assets** should be in the simple asset list, which are usually the custom game ones. You are able to just copy and paste directly from the list to the text box, as the program will handle the formatting automatically.
2. The assets with a circular dependancy are, generally, `/Script/Engine.SoundClass`, `/Script/Engine.SoundSubmix`, `/Script/Engine.EndpointSubmix`, **and any custom game assets that you put into the simple assets box**. You will know if you have missed any because when running the Asset Generator tool, you may find it crashing on "circular dependancy on `<asset names>` found" or similar.
3. Here you can select which asset types you which to *skip* serializing. I recommend that you skip at least all of the types selected by default, as they currently cannot be serialized accurately enough to not crash the editor. **However**, as I explain in the [generating animation files]() section, once you have your `.FBX` files for animation sequence and skeletal mesh, you can turn these back on (i.e., unselecting them from this list). The `Uncategorized` asset type is for any simple/misc asset types not in this list.
4. This box allows you to select any asset types that you wish to "dummy" - i.e. generate JSON with just the basic header info, that will make the asset generator generate as empty assets. The default is none.
5. Check this if you want the selected asset types to dummy with properties, which is basically where the tool "guesses" what should be in it, as if the asset type is being generated as a "simple" asset.

## Running the program
These widgets are to do with actually running the program.

![RunSettingsNumbered](..\Wiki\Images\RunSettingsNumbered.png)

1. Scan the selected assets from the tree view. You **must** do this at the start, as it will produce the AllTypes and AssetTypes files that give comprehensive lists on what asset types there are. You will need these for the simple asset list as described later.
2. Serialize native assets into JSON, if the game has any. It reads from the DefaultGame.ini file and produces a dummy enum/BP depending on the nativization setting.
3. Serialize the cooked assets into JSON. This will probably take a while, depending on the number of assets to parse. Static meshes and textures take the longest amount of time, depending on their sizes. 
4. A live counter for any of these 3 operations. The first number is the number of assets it has scanned/serialized, and the second is the total number of assets that will be scanned/serialized.
5. The program's runtime output box. It will print info about what you've done, and also any stack traces if an error occurs. **If any error occurs, and you don't know how to fix it, please submit it as part of an issue using the bug report template**.

