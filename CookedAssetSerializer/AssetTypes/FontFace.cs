using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using System.Security.Cryptography;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeFontFace() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();
            var fontface = Exports[Asset.mainExport - 1] as FontFaceExport;
            var path2 = Path.ChangeExtension(path1, "ttf");

            if (fontface == null) return;
            ja.Add("AssetClass", "FontFace");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();
            var aoData = new JObject();

            ja.Add("AssetSerializedData", asData);
            asData.Add("AssetObjectData", SerializaListOfProperties(fontface.Data));

            long length = 0;
            if (fontface.bSaveInlineData) {
                File.WriteAllBytes(path2, fontface.FontFaceData.Data);
                length = fontface.FontFaceData.Data.Length;
            } else {
                var targetFile = Path.ChangeExtension(Asset.FilePath, "ufont");
                if (File.Exists(targetFile)) {
                    if (targetFile != null) {
                        File.Copy(targetFile, path2, true);
                        length = new FileInfo(targetFile).Length;
                    }
                }
            }

            using (var md5 = MD5.Create()) {
                using (var stream1 = File.OpenRead(path2)) {
                    var hash = md5.ComputeHash(stream1).Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
                    hash += length.ToString("x2");
                    asData.Add("FontPayloadHash", hash);
                }
            }

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
