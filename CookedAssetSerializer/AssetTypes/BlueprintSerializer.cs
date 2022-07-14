using System.Collections.Generic;
using UAssetAPI;
using static CookedAssetSerializer.Utils;

namespace CookedAssetSerializer.AssetTypes
{
    public class BlueprintSerializer : Serializer<ClassExport>
    {
        public void SerializeAsset(bool dummy)
        {
            if (!SetupSerialization(out string name, out string gamepath, out string path1)) return;
        }
    }
}