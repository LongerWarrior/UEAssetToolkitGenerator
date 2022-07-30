using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class MaterialFunctionSerializer : SimpleAssetSerializer<NormalExport>
    {
        public MaterialFunctionSerializer(Settings settings, UAsset asset) : base(settings, asset)
        {
            Setup(true);
            SerializeAsset();
        }
    }
}