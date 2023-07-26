namespace CookedAssetSerializer.AssetTypes;

public class SimpleAssetSerializer<T> : Serializer<T> where T : NormalExport
{
    public SimpleAssetSerializer(JSONSettings assetSettings, UAsset asset)
    {
        Settings = assetSettings;
        Asset = asset;
    }

    public bool Setup(bool isInheritor = true, bool isSimple = true, string overrideClassName = null)
    {
        if (!SetupSerialization()) return false;

        if (!SetupAssetInfo()) return false;
        
        // Case for DataTable, Uncategorized
        if (isSimple && !isInheritor) ClassName = "SimpleAsset";

        // Case for AnimationSequence
        if (overrideClassName != null) ClassName = overrideClassName;

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
            // TODO: Fix edge case where anim sequence has missing bone tree indexes in track to skeleton map table
            /*var mapTable = properties["TrackToSkeletonMapTable"];
            if (mapTable != null)
            {
                // Sometimes, the bone tree index misses some numbers, so we need to make sure that it never misses any numbers
                // Make a temporary sorted array. Then loop through the array, and if the current index is not equal to the current value, then we need to add a new value
                var sortedArray = mapTable.OrderBy(x => x["BoneTreeIndex"].Value<int>());
                var newValues = new List<JObject>();
                var currentIndex = 0;
                foreach (var value in sortedArray)
                {
                    if (value["BoneTreeIndex"].Value<int>() != currentIndex)
                    {
                        var newValue = new JObject();
                        newValue.Add("BoneTreeIndex", currentIndex);
                        newValues.Add(newValue);
                    }
                    currentIndex++;
                }

                // Add the new values to the map table
                foreach (var value in newValues)
                {
                    mapTable = (JToken)mapTable.Append(value);
                }
            }*/
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