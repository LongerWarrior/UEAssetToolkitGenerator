using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UAssetAPI;
using UAssetAPI.Kismet;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class SimpleAssetSerializer : Serializer<NormalExport>
    {
        public SimpleAssetSerializer(Settings assetSettings)
        {
            Settings = assetSettings;
        }

        public void SerializeAsset(bool isSimple = true)
        {
            if (!SetupSerialization()) return;

            if (!SetupAssetInfo()) return;
            
            if (isSimple)
            {
                ClassName = "SimpleAsset";
            }
            
            SerializeHeaders();
            
            var properties = SerializaListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
            if (GetFullName(ClassExport.ClassIndex.Index, Asset) == "/Script/Paper2D.PaperSprite") 
            {
                if (KismetSerializer.FindPropertyData(ClassExport, "BakedSourceTexture", out var _source))
                {
                    var source = (ObjectPropertyData)_source;
                    properties.Add(new JProperty("SourceTexture", GetFullName(source.Value.Index, Asset)));
                }
                for (var i = 0; i < properties.Properties().Count(); i++) 
                {
                    var props = properties.Properties().ElementAt(i);
                    switch (props.Name)
                    {
                        case "BakedSourceDimension":
                            properties.Add("SourceDimension", props.Value);
                            break;
                        case "BakedRenderData":
                            properties.Add("RenderData", props.Value);
                            break;
                    }
                }
            }
            properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
            AssetData.Add("AssetObjectData", properties);
            
            AssignAssetSerializedData();

            WriteJSONOut(ObjectHierarchy(AssetInfo, ref RefObjects));
        }
    }
}