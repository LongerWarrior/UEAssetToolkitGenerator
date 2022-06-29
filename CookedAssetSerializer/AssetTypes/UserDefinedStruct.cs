using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using UAssetAPI;
using static CookedAssetSerializer.Utils;
using static CookedAssetSerializer.SerializationUtils;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CookedAssetSerializer {
    public partial class Serializers {
        public static void SerializeUserDefinedStruct() {
            if (!SetupSerialization(out var name, out var gamePath, out var path1)) return;
            var ja = new JObject();

            if (Exports[Asset.mainExport - 1] is not UserDefinedStructExport mainObject) return;
            ja.Add("AssetClass", "UserDefinedStruct");
            ja.Add("AssetPackage", gamePath);
            ja.Add("AssetName", name);
            var asData = new JObject();

            ja.Add("AssetSerializedData", asData);

            asData.Add("SuperStruct", Index(mainObject.SuperIndex.Index));
            RefObjects.Add(Index(mainObject.SuperIndex.Index));
            var children = new JArray();
            foreach (var package in mainObject.Children)
                if (Exports[package.Index - 1] is FunctionExport func)
                    children.Add(SerializeFunction(func));
            asData.Add("Children", children);
            var childProperties = new JArray();

            foreach (var property in mainObject.LoadedProperties) childProperties.Add(SerializeProperty(property));
            asData.Add("ChildProperties", childProperties);
            asData.Add("StructFlags", (uint)mainObject.StructFlags);

            var bGuid = false;
            if (FindPropertyData(mainObject, "Guid", out var prop)) {
                var _guid = (StructPropertyData)prop;
                if (FindPropertyData(_guid.Value, "Guid", out PropertyData _prop)) {
                    var realGuid = (GuidPropertyData)_prop;
                    var guid = realGuid.Value.ToUnsignedInts();
                    asData.Add("Guid", guid.Select(x => x.ToString("X8")).Aggregate((a, b) => a + b));
                    bGuid = true;
                }
            }

            if (!bGuid) asData.Add("Guid", new Guid("00000000000000000000000000000000"));

            asData.Add("StructDefaultInstance", SerializaListOfProperties(mainObject.DefaultStructInstance));
            asData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct<int>()));

            ja.Add(ObjectHierarchy(Asset));
            File.WriteAllText(path1, ja.ToString());
        }
    }
}
