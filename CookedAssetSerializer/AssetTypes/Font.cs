using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using Textures;
using SkiaSharp;
using System.Security.Cryptography;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeFont() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			FontExport font = exports[asset.mainExport-1] as FontExport;

			if (font != null) {

				ja.Add("AssetClass", "Font");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				JObject aodata = new JObject();

				FindPropertyData(font, "FontCacheType", out PropertyData prop);
				EnumPropertyData fontcachetype = (EnumPropertyData)prop;

				if (fontcachetype.Value.ToName() == "EFontCacheType::Runtime") {
					asdata.Add("IsRuntimeFont", true);
					asdata.Add("ReferencedFontFacePackages", new JArray()); 
					//TO DO
				}

				ja.Add("AssetSerializedData", asdata);
				asdata.Add("AssetObjectData", SerializaListOfProperties(font.Data));

				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}