# Introduction

## What is this?
This is a work-in-progress .NET API/GUI to generate [UEAssetToolkit](https://github.com/Buckminsterfullerene02/UEAssetToolkit-Fixes) compatible JSON files. 

**WARNING:** This tool is *NOT* easy to use! Same goes for UEAssetToolkit. Don't expect anything to work first try, and expect plenty of editor crashes. You *must* know what you are doing before you attempt to use this tool! It usually takes ~5-10 hours to generate content for a small game with ~2-5k assets. 

At the time of writing, it can serialize all of the cooked data for the following asset types:
- Blueprint
- Widget Blueprint
- Animation Blueprint
- Data Table
- String Table
- User Defined Struct
- User Defined Enum
- Texture2D
- Material (excluding the data in the shadercache files)
- Font
- FontFace
- Blendspace Base
- Curve Base
- Skeleton
- Static Mesh (including .FBX file)
- Skeletal Mesh (excluding .FBX file, due to skeletons being buggered in the FBX tool)
- Anim Montage
- Material Instance Constant
- Material Parameter Collection
- Physical Material
- Sound Cue
- Simple Assets

"Simple Assets" are either:
1. An asset with only properties and values/references, for example, data asset or sound class.
2. A game's custom asset that the tool will serialize any available data by "guessing" its format. These assets can be specified in the simple assets entry box - more on this in the [asset settings]() section.

It also has the ability to dummy any Blueprints/UserDefinedEnums (NOT UserDefinedStructs) based on the games' nativized asset config settings. This is vital, because otherwise you will encounter many editor crashes when generating references for assets that don't exist. For the UserDefinedStructs, you will have to either manually dummy these, or use another tool to get their data. [UE4SS 2.0]() may have this feature soon.

For most other asset types, that currently cannot be serialized (or don't need to be, e.g. sound wave), you will need to use the "Asset Utilities" tab to copy their cooked files into the UE project. More on this in the [asset utilities settings]() section.

## Using this tool

link to GUI.md

link to GeneratingFBX.md

## Using the Asset Generator tool

[Asset Generator REAME](https://github.com/Buckminsterfullerene02/UEAssetToolkit-Fixes)

link to GameNotes.md