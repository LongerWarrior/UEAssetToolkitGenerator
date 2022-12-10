namespace CookedAssetSerializer.AssetTypes;

public class SimpleAssetSerializer<T> : Serializer<T> where T : NormalExport
{
    public SimpleAssetSerializer(Settings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
    }

    public bool Setup(bool isInheritor = true, bool isSimple = true)
    {
        if (!SetupSerialization()) return false;

        if (!SetupAssetInfo()) return false;
        
        // Case for DataTable, Uncategorized
        if (isSimple && !isInheritor) ClassName = "SimpleAsset";

        SerializeHeaders();
        
        // Case for DataTable, SubsurfaceProfile, Uncategorized
        if (!isInheritor) AssetData.Add("AssetClass", GetFullName(ClassExport.ClassIndex.Index, Asset));

        return true;
    }

    public void SerializeAsset(JProperty assetClass = null, JProperty extraAssetData = null, 
        JProperty extraAssetObjectData = null, JProperty extraProperties = null, bool skipRefs = false, 
        bool skipAOD = false, bool checkBoneTreeIndex = false)
    {
        // Case for CurveBase, SKM, SoundCue
        if (assetClass != null) AssetData.Add(assetClass);

        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);

        // Case for AnimSequence
        if (checkBoneTreeIndex)
        {
            var mapTable = properties["TrackToSkeletonMapTable"];
            if (mapTable != null)
            {
                var sortedArray = mapTable.OrderBy(x => x["BoneTreeIndex"].Value<int>());
                mapTable.Replace(new JArray(sortedArray));
            }
        }

        PaperSpriteSerializer(ref properties);
        
        // Case for DataTable
        if (extraAssetData != null) AssetData.Add(extraAssetData);
        
        // Case for MaterialInstanceConstant
        if (extraProperties != null) properties.Add(extraProperties);
        
        if (!skipRefs) properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        
        // Case for DataTable
        if (skipAOD)
        {
            AssetData.Add(SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects).Properties());
            AssetData.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        }
        else AssetData.Add("AssetObjectData", properties);

        // Case for FileMediaSource
        if (extraAssetObjectData != null) AssetData.Add(extraAssetObjectData);
        
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }

    private void PaperSpriteSerializer(ref JObject properties)
    {
        // TODO: Move into own class perhaps
        if (GetFullName(ClassExport.ClassIndex.Index, Asset) == "/Script/Paper2D.PaperSprite") 
        {
            if (FindPropertyData(ClassExport, "BakedSourceTexture", out var _source))
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
    }
}