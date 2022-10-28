using System.Drawing;

namespace UAssetAPI;

public class ColorJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Color[]) || objectType == typeof(Color);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is Color[])
        {
            List<int[]> colorList = new List<int[]>();
            foreach (Color color in (Color[])value)
            {
                int[] colorInts = new int[3];
                colorInts[0] = color.R;
                colorInts[1] = color.G;
                colorInts[2] = color.B;
                colorList.Add(colorInts);
            }
            writer.WriteStartArray();
            foreach (int[] color in colorList)
            {
                writer.WriteStartArray();
                foreach (int colorInt in color)
                {
                    writer.WriteValue(colorInt);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
        else
        {
            int[] colorInts = new int[3];
            Color color = (Color)value;
            colorInts[0] = color.R;
            colorInts[1] = color.G;
            colorInts[2] = color.B;
            writer.WriteStartArray();
            foreach (int colorInt in colorInts)
            {
                writer.WriteValue(colorInt);
            }
            writer.WriteEndArray();
        }
    }
    
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value;
    }
}