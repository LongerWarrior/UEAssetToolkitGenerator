using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using System;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeDummy() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not { } simple) return;
            ja.Add("AssetClass", "SimpleAsset");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject {
                { "AssetClass", GetFullName(simple.ClassIndex.Index) },
                { "AssetObjectData", new JObject(new JProperty("$ReferencedObjects", new JArray())) }
            };

            ja.Add("AssetSerializedData", asData);
            ja.Add(new JProperty("ObjectHierarchy", new JArray()));
            File.WriteAllText(path1, ja.ToString());
        }

        public static List<string> WaveProps = new();

        public static void SerializeDummy(bool withProperties) {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is NormalExport simple) {
                ja.Add("AssetClass", "SimpleAsset");
                ja.Add("AssetPackage", gamePath);
                ja.Add("AssetName", name);
                var asData = new JObject { { "AssetClass", GetFullName(simple.ClassIndex.Index) } };
                var jData = SerializaListOfProperties(simple.Data);

                foreach (var prop in jData.Properties()) if (!WaveProps.Contains(prop.Name)) WaveProps.Add(prop.Name);
                jData.Add("$ReferencedObjects", new JArray());
                asData.Add("AssetObjectData", jData);
                ja.Add("AssetSerializedData", asData);
                if (RefObjects.Count > 0) {
                    Console.WriteLine(Asset.FilePath);
                    ja.Add(ObjectHierarchy(Asset));
                } else {
                    ja.Add(new JProperty("ObjectHierarchy", new JArray()));
                }

                File.WriteAllText(path1, ja.ToString());
            }
        }
    }
}
