using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthenticationService.Utility
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string _format = "dd-MM-yyyy"; // Define the format

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (DateTime.TryParseExact(value, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            throw new JsonException($"Invalid date format. Expected format: {_format}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
