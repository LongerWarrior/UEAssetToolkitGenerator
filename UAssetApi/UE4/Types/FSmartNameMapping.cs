namespace UAssetAPI;

public class FSmartNameMapping
{
    public Dictionary<FName, Guid> GuidMap;
    public Dictionary<ushort, FName> UidMap;
    public Dictionary<FName, FCurveMetaData> CurveMetaDataMap;

    public void Read(AssetBinaryReader reader)
    {
        var length = reader.ReadInt32();
        if (length > 0)
        {
            CurveMetaDataMap = new Dictionary<FName, FCurveMetaData>(length);
            for (var i = 0; i < length; i++)
            {
                var name = reader.ReadFName();
                var curve = new FCurveMetaData();
                curve.Read(reader);
                CurveMetaDataMap[name] = curve;
            }
        }
    }
}