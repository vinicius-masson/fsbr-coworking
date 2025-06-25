using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Coworking.Web.Extensions
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TryGetDateTime(out var date))
                    return date;

                var dateString = reader.GetString();
                if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    return date;

                if (DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out date))
                    return date;
            }
            catch
            {
                return DateTime.MinValue;
            }

            throw new JsonException($"Formato de data inválido: {reader.GetString()}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
    }
}
