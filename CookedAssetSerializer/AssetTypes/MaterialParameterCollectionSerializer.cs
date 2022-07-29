using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class MaterialParameterCollectionSerializer : SimpleAssetSerializer<NormalExport>
    {
        public MaterialParameterCollectionSerializer(Settings settings) : base(settings)
        {
            Setup();
            SerializeAsset();
        }
    }
}