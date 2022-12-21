namespace CookedAssetSerializer.AssetTypes;

public class MaterialSerializer : Serializer<NormalExport>
{
    public MaterialSerializer(JSONSettings settings, UAsset asset)
    {
        Settings = settings;
        Asset = asset;
        SerializeAsset();
    }

    private void SerializeAsset()
    {
        if (!SetupSerialization()) return;
            
        DisableGeneration.Add("Metallic");
        DisableGeneration.Add("Specular");
        DisableGeneration.Add("Anisotropy");
        DisableGeneration.Add("Normal");
        DisableGeneration.Add("Tangent");
        DisableGeneration.Add("EmissiveColor");
        DisableGeneration.Add("WorldPositionOffset");
        DisableGeneration.Add("Refraction");
        DisableGeneration.Add("PixelDepthOffset");
        DisableGeneration.Add("ShadingModelFromMaterialExpression");
        DisableGeneration.Add("MaterialAttributes");
        DisableGeneration.Add("FunctionInfos");
        DisableGeneration.Add("DefaultLayers");
        DisableGeneration.Add("DefaultLayerBlends");

        if (!SetupAssetInfo()) return;

        SerializeHeaders();
            
        PopulateCachedExpressionDataEntries(ref ClassExport.Data);
        SerializeReferencedFunctions(ClassExport.Data, out var functions);
            
        var properties = SerializeListOfProperties(ClassExport.Data, AssetInfo, ref RefObjects);
        properties.Add("$ReferencedObjects", JArray.FromObject(RefObjects.Distinct()));
        RefObjects = new List<int>();
        AssetData.Add("AssetObjectData", properties);
            
        AssetData.Add(functions);
            
        AssignAssetSerializedData();

        WriteJsonOut(ObjectHierarchy(AssetInfo, ref RefObjects));
    }
        
    private void SerializeReferencedFunctions(List<PropertyData> data, out JProperty[] _functions) 
    {
    _functions = new JProperty[3];
	JArray referencedFunctions = new();
	JArray materialLayers = new();
	JArray materialLayersBlends = new();

	if (FindPropertyData(data, "CachedExpressionData", out PropertyData cedata)) 
	{
		if (FindPropertyData(((StructPropertyData)cedata).Value, "FunctionInfos", out PropertyData _parameters)) 
		{
			var functionsInfo = ((ArrayPropertyData)_parameters).Value;
			foreach (var propertyData in functionsInfo) 
			{
				if (FindPropertyData(((StructPropertyData)propertyData).Value, "Function", out PropertyData func)) 
				{
					referencedFunctions.Add(GetFullName(((ObjectPropertyData)func).Value.Index, Asset));
				}
			}
		}

		if (FindPropertyData(((StructPropertyData)cedata).Value, "MaterialLayers", out PropertyData _layers)) 
		{
			var layers = ((ArrayPropertyData)_layers).Value;
			foreach (var layer in layers)
			{
				var func = (ObjectPropertyData)layer;
				materialLayers.Add(GetFullName(func.Value.Index, Asset));
			}
		}

		if (FindPropertyData(((StructPropertyData)cedata).Value, "MaterialLayerBlends", out PropertyData _layersblend)) 
		{
			var layersBlends = ((ArrayPropertyData)_layersblend).Value;
			foreach (var layersBlend in layersBlends)
			{
				var func = (ObjectPropertyData) layersBlend;
				materialLayersBlends.Add(GetFullName(func.Value.Index, Asset));
			}
		}
	}

	_functions[0] = new JProperty("ReferencedFunctions", referencedFunctions.Distinct());
	_functions[1] = new JProperty("MaterialLayers", materialLayers.Distinct());
	_functions[2] = new JProperty("MaterialLayerBlends", materialLayersBlends.Distinct());
    }

    private void PopulateCachedExpressionDataEntries(ref List<PropertyData> data, int v = 5) {
	var entriesName = "Entries";
	if (Asset.EngineVersion >= UE4Version.VER_UE4_26) 
	{
		entriesName = "RuntimeEntries";
	}

	if (FindPropertyData(data, "CachedExpressionData", out PropertyData cedata)) 
	{
		if (FindPropertyData(((StructPropertyData)cedata).Value, "Parameters", out PropertyData _parameters)) 
		{
			var parameters = ((StructPropertyData)_parameters).Value;
			var fullEntries = Enumerable.Range(0, v).ToList();
			var entries = (from t in parameters where t.Name.ToName() == entriesName select t.DuplicationIndex).ToList();

			if (entries.Count > 0) 
			{
				var missing = fullEntries.Except(entries).ToList();
				parameters.AddRange(missing.Select(missed => 
					new StructPropertyData(new FName(entriesName), 
					new FName("MaterialCachedParameterEntry"), missed)
					{
						Value = new List<PropertyData>
						{
							new ArrayPropertyData(new FName("NameHashes")), 
							new ArrayPropertyData(new FName("ParameterInfos")), 
							new ArrayPropertyData(new FName("ExpressionGuids")), 
							new ArrayPropertyData(new FName("Overrides"))
						}
					}));
				parameters.Sort((x, y) => 
				{
					var ret = x.Name.ToName().CompareTo(y.Name.ToName());
					if (ret == 0) ret = x.DuplicationIndex.CompareTo(y.DuplicationIndex);
					return ret;
				});
			}
		}
	}
    }
}