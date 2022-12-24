# UEAssetToolkitGenerator 
This is a work-in-progress .NET API/GUI to generate [UEAssetToolkit](https://github.com/Buckminsterfullerene02/UEAssetToolkit-Fixes) compatible JSON files. You must have .NET 6 installed to use. Also known as Cooked Asset Serializer (CAS).

**WARNING:** This tool combined with UEAssetToolkit is only guaranteed to work for games UE4.25-4.27! It is likely you will have to make modifications to either tools to work for other versions!

## Usage
Please follow the [wiki](https://github.com/LongerWarrior/UEAssetToolkitGenerator/wiki) for detailed instructions on how to use this tool.

## Examples
These two example videos are for games that full modkits have already been generated for (click the images): 
[![DRG Example](https://img.youtube.com/vi/aiI_SotvoT0/0.jpg)](https://www.youtube.com/watch?v=aiI_SotvoT0)
[![cyubeVR Example](https://img.youtube.com/vi/D3JoJlCRUEE/0.jpg)](https://youtu.be/D3JoJlCRUEE)

## Credits
- LongerWarrior for almost all of the serialization code and modifying UAAPI/CUE4Parse to add new export types. 
- Archengius for developing the UEAssetToolkit in the first place.
- atenfyr for developing UAAPI/GUI which CAS builds on.
- CUE4Parse for having support for most asset types which CAS modifies for the UAAPI format - we hope in the future to drop UAAPI and purely base on CUE4Parse.
- Buckminsterfullerene for CAS GUI and various fixes/improvements.
- Narknon for bits and bobs with the parse tree and GUI changes, & helping out with testing. 

*We are not responsible for any misuse of this tool.*
