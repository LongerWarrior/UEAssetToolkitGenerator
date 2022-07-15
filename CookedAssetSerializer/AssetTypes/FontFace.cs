using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeFontFace() {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			FontFaceExport fontface = Exports[Asset.mainExport - 1] as FontFaceExport;
			var path2 = Path.ChangeExtension(path1,"ttf");

			if (fontface != null) {

				ja.Add("AssetClass", "FontFace");
				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				JObject aodata = new JObject();


				ja.Add("AssetSerializedData", asdata);
				asdata.Add("AssetObjectData", SerializaListOfProperties(fontface.Data));

				long length = 0;
				if (fontface.bSaveInlineData) {
					File.WriteAllBytes(path2, fontface.FontFaceData.Data);
					length = fontface.FontFaceData.Data.Length;
				} else {
					var targetFile = Path.ChangeExtension(Asset.FilePath, "ufont");
					if (File.Exists(targetFile)) {
						File.Copy(targetFile, path2,true);
						length = new FileInfo(targetFile).Length;
					}
				}

				using (var md5 = MD5.Create()) {
					using (var stream1 = File.OpenRead(path2)) {
						string hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
						hash += length.ToString("x2");
						asdata.Add("FontPayloadHash", hash);
					}
				}

				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}
	}


}