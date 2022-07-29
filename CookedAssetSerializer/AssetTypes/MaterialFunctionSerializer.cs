using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class MaterialFunctionSerializer : SimpleAssetSerializer<NormalExport>
    {
        public MaterialFunctionSerializer(Settings settings) : base(settings)
        {
            Setup();
            SerializeAsset();
        }
    }
}