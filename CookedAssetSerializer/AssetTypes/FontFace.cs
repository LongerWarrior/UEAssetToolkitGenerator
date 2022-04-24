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

		public static void SerializeFontFace() {
			SetupSerialization(out string name, out string gamepath, out string path1);
			JObject ja = new JObject();
			FontFaceExport fontface = exports[asset.mainExport - 1] as FontFaceExport;
			var path2 = Path.ChangeExtension(path1,"ttf");

			if (fontface != null) {

				ja.Add("AssetClass", "FontFace");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				JObject aodata = new JObject();


				ja.Add("AssetSerializedData", asdata);
				asdata.Add("AssetObjectData", SerializaListOfProperties(fontface.Data));

				if (fontface.bSaveInlineData) {
					File.WriteAllBytes(path2, fontface.FontFaceData.Data);
				} else {
					var targetFile = Path.ChangeExtension(asset.FilePath, "ufont");
					if (File.Exists(targetFile)) {
						File.Copy(targetFile, path2,true);
					}
				}

				using (var md5 = MD5.Create()) {
					using (var stream1 = File.OpenRead(path2)) {
						string hash = md5.ComputeHash(stream1).Select(x => x.ToString("X2")).Aggregate((a, b) => a + b);
						asdata.Add("FontPayloadHash", hash);
					}
				}

				ja.Add(ObjectHierarchy(asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}