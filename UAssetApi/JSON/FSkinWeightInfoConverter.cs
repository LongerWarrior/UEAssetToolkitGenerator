namespace UAssetAPI;

public class FSkinWeightInfoConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(byte[]);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        byte[] bytes = (byte[])value;
        writer.WriteStartArray();
        for (int i = 0; i < bytes.Length; i++)
        {
            writer.WriteValue(bytes[i]);
        }
        writer.WriteEndArray();
    }
    
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value;
    }
}