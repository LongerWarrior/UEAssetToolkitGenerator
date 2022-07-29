using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UAssetAPI;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class DummySerializer : Serializer<Export>
    {
        public DummySerializer(Settings assetSettings)
        {
            Settings = assetSettings;
            SerializeAsset();
        }
        
        private void SerializeAsset()
        {
            if (!SetupSerialization()) return;

            if (!SetupAssetInfo()) return;
            
            SerializeHeaders();

            AssetData.Add("AssetObjectData", new JObject(new JProperty("$ReferencedObjects", new JArray())));
            
            AssignAssetSerializedData();

            WriteJSONOut(new JProperty("ObjectHierarchy", new JArray()));
        }
    }
    
    public class DummyWithProps : Serializer<NormalExport>
    {
        public DummyWithProps(Settings assetSettings)
        {
            Settings = assetSettings;
            SerializeAsset();
        }
        
        private void SerializeAsset()
        {
            if (!SetupSerialization()) return;

            if (!SetupAssetInfo()) return;
            
            SerializeHeaders();
            
            var properties = SerializaListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
            List<string> waveProps = new();
            foreach (var prop in properties.Properties()) 
            {
                if (!waveProps.Contains(prop.Name)) 
                {
                    waveProps.Add(prop.Name);
                }
            }
            
            properties.Add("$ReferencedObjects", new JArray());

            AssetData.Add("AssetObjectData", properties);
            
            AssignAssetSerializedData();
            
            if (RefObjects.Count > 0) 
            {
                Console.WriteLine(Asset.FilePath);
                WriteJSONOut(ObjectHierarchy(AssetInfo, ref RefObjects));
            } 
            else WriteJSONOut(new JProperty("ObjectHierarchy", new JArray()));

            WriteJSONOut(new JProperty("ObjectHierarchy", new JArray()));
        }
    }
}