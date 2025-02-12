using System.Text.Json;
using System.Text.Json.Serialization;

namespace MentorAI.Shared.Serialization;

public class GuidConverter : JsonConverter<Guid?>
{
    public override Guid? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var inputString = reader.GetString();

        if (string.IsNullOrEmpty(inputString))
        {
            return null;
        }

        return Guid.Parse(inputString);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Guid? value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value?.ToString());
}