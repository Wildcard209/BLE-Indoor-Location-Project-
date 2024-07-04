using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebClient.Converter
{
    public class StringTupleJsonConverter : JsonConverter<Tuple<string, string>>
    {
        public override Tuple<string, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;
                string item1 = root.GetProperty("item1").GetString();
                string item2 = root.GetProperty("item2").GetString();
                return new Tuple<string, string>(item1, item2);
            }
        }

        public override void Write(Utf8JsonWriter writer, Tuple<string, string> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
