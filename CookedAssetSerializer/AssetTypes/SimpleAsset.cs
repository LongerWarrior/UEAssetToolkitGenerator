using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.Kismet;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer {

    public partial class Serializers {

		public static void SerializeSimpleAsset(bool issimple = true, bool skipdependencies = false) {
			if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
			JObject ja = new JObject();
			NormalExport simple = Exports[Asset.mainExport - 1] as NormalExport;

			if (simple != null) {

				if (issimple) {
					ja.Add("AssetClass", "SimpleAsset");
				} else {
					ja.Add("AssetClass", simple.ClassIndex.ToImport(Asset).ObjectName.ToName());
				}


				ja.Add("AssetPackage", gamepath);
				ja.Add("AssetName", name);
				JObject asdata = new JObject();
				if (issimple) {
					asdata.Add("AssetClass", GetFullName(simple.ClassIndex.Index));
					if (CircularDependency.Contains( GetFullName(simple.ClassIndex.Index))) {
						skipdependencies = true;
					}
					asdata.Add("SkipDependecies", skipdependencies);
				}

				JObject jdata = SerializaListOfProperties(simple.Data);

				if (GetFullName(simple.ClassIndex.Index) == "/Script/Paper2D.PaperSprite") {

					if (KismetSerializer.FindPropertyData(simple, "BakedSourceTexture", out PropertyData _source)){
						ObjectPropertyData source = (ObjectPropertyData)_source;
						jdata.Add(new JProperty("SourceTexture",GetFullName(source.Value.Index)));
					}
                    for (int i = 0; i < jdata.Properties().Count(); i++) {
                        var jprop = jdata.Properties().ElementAt(i);
                        if (jprop.Name == "BakedSourceDimension") {
                            jdata.Add("SourceDimension", jprop.Value);
                        }
                        if (jprop.Name == "BakedRenderData") {
                            jdata.Add("RenderData", jprop.Value);
                        }
                    }
                }

				jdata.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

				asdata.Add("AssetObjectData", jdata);
				ja.Add("AssetSerializedData", asdata);
				ja.Add(ObjectHierarchy(Asset));
				File.WriteAllText(path1, ja.ToString());

			}
		}

    }


}