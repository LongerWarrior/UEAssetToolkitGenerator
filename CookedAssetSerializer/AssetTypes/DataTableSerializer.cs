using Newtonsoft.Json.Linq;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using static CookedAssetSerializer.SerializationUtils;

namespace CookedAssetSerializer.AssetTypes
{
    public class DataTableSerializer : SimpleAssetSerializer<DataTableExport>
    {
        public DataTableSerializer(Settings settings, UAsset asset) : base(settings, asset)
        {
            Setup(true);
            SerializeAsset(null, SerializeDataTable(ClassExport.Table));
        }

        private JProperty SerializeDataTable(UDataTable table) 
        {
            var rowData = new JProperty("RowData");
            if (table.Data.Count > 0) 
            {
                var propertyData = new JObject();
                foreach (PropertyData property in table.Data) 
                {
                    propertyData.Add(SerializePropertyData(property, AssetInfo, ref RefObjects));
                }
                rowData.Value = propertyData;
            } 
            else rowData.Value = -1;
            
            return rowData;
        }
    }
}