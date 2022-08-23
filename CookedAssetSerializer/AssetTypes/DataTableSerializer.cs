namespace CookedAssetSerializer.AssetTypes;

public class DataTableSerializer : SimpleAssetSerializer<DataTableExport>
{
    public DataTableSerializer(JSONSettings settings, UAsset asset) : base(settings, asset)
    {
        if (!Setup()) return;
        SerializeAsset(null, SerializeDataTable(ClassExport.Table), null, 
            null, false, true);
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