using UAssetAPI;

namespace CookedAssetSerializer.AssetTypes
{
    public class PhysicalMaterialSerializer : SimpleAssetSerializer<NormalExport>
    {
        public PhysicalMaterialSerializer(Settings settings) : base(settings)
        {
            Setup();
            SerializeAsset();
        }
    }
}