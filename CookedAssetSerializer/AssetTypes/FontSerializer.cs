namespace CookedAssetSerializer.AssetTypes;

public class FontSerializer : Serializer<FontExport>
{
	public FontSerializer(JSONSettings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
	    if (!SetupSerialization()) return;
	    
	    if (!SetupAssetInfo()) return;
	    
	    SerializeHeaders();
	    
        var bOffline = FindPropertyData(ClassExport, "FontCacheType", out var prop);
        if (bOffline) 
        {
			var fontCacheType = (EnumPropertyData)prop;
			if (fontCacheType.Value.ToName() == "EFontCacheType::Runtime") 
			{
				AssetData.Add("IsRuntimeFont", true);
				List<FontDataPropertyData> allFonts = new();
				List<string> allFontsRef = new();

				if (FindPropertyData(ClassExport, "CompositeFont", out PropertyData _cfont)) 
				{
					var cfont = (StructPropertyData)_cfont;
					MakeAllFonts(cfont, ref allFonts, "DefaultTypeface");
					MakeAllFonts(cfont, ref allFonts, "FallbackTypeface");
					MakeSubTypeFonts(cfont, ref allFonts);
				}

				foreach (var fontRef in allFonts) 
				{
					if (fontRef.Value.LocalFontFaceAsset.Index < 0) 
					{
						allFontsRef.Add(GetParentName(fontRef.Value.LocalFontFaceAsset.Index, Asset));
					}
					else Console.WriteLine("exportfontface");
				} 

				AssetData.Add("ReferencedFontFacePackages", JArray.FromObject(allFontsRef.Distinct()));
			} else AssetData.Add("IsOfflineFont", true);
        }/* else {
		asdata.Add("IsOfflineFont", true);
		}*/
		
		AssignAssetSerializedData();
		
		var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
		AssetData.Add("AssetObjectData", properties);
		properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
		
		WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }

    private void MakeAllFonts(StructPropertyData cfont, ref List<FontDataPropertyData> allFonts, string propName)
    {
	    if (FindPropertyData(cfont.Value, propName, out PropertyData _fontFace)) 
	    {
	        var defFontFace = (StructPropertyData)_fontFace;
	        if (FindPropertyData(defFontFace.Value, "Fonts", out PropertyData _fonts)) 
	        {
		        var fonts = (ArrayPropertyData)_fonts;
		        foreach (var fontVal in fonts.Value)
		        {
			        var font = (StructPropertyData)fontVal;
			        if (!FindPropertyData(font.Value, "Font", out PropertyData _fontData)) continue;
			        var fontData = (StructPropertyData)_fontData;
			        allFonts.Add((FontDataPropertyData)fontData.Value[0]);
		        }
	        }
	    }
    }

    private void MakeSubTypeFonts(StructPropertyData cfont, ref List<FontDataPropertyData> allFonts)
    {
	    if (FindPropertyData(cfont.Value, "SubTypefaces", out PropertyData _subTypeFaces)) 
	    {
	        var subTypeFaces = (ArrayPropertyData)_subTypeFaces;
	        foreach (var subTypeFace in subTypeFaces.Value)
	        {
		        var typeFaces = (StructPropertyData)subTypeFace;
		        if (FindPropertyData(typeFaces.Value, "Typeface", out PropertyData _typeFace)) 
		        {
			        var typeface = (StructPropertyData)_typeFace;
			        if (FindPropertyData(typeface.Value, "Fonts", out PropertyData _fonts)) 
			        {
				        var fonts = (ArrayPropertyData)_fonts;
				        foreach (var fontVal in fonts.Value)
				        {
					        var font = (StructPropertyData)fontVal;
					        if (FindPropertyData(font.Value, "Font", out PropertyData _fontData)) 
					        {
						        var fontData = (StructPropertyData)_fontData;
						        allFonts.Add((FontDataPropertyData)fontData.Value[0]);
					        }
				        }
			        }
		        }
	        }
	    }
    }
}