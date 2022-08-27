using System.ComponentModel;
using System.Globalization;

namespace UAssetAPI;

public class FStringTypeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        return new FString(Convert.ToString(value));
    }
}
public class FStringJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(FString);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue((value as FString)?.Value);
    }

    public override bool CanRead
    {
        get { return true; }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null) return null;
        return new FString(Convert.ToString(reader.Value));
    }

}
