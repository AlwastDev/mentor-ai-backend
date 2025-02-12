using System.Buffers;
using System.Text;
using System.Text.Json;
using MentorAI.Shared.Responses;

namespace MentorAI.Shared.Serialization;

internal static class JsonSerializer
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    internal static string SerializeObject<T>(T obj) => System.Text.Json.JsonSerializer.Serialize(obj, _serializerOptions);

    internal static string? SerializeObject(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return null;
        }

        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer, new() { Indented = true });

        writer.WriteStartObject();

        writer.WriteString(nameof(message), message);

        writer.WriteEndObject();

        writer.Flush();

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }

    internal static string SerializeObject<T>(ResponseArray<T> obj, string name)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer, new() { Indented = true });

        writer.WriteStartObject();

        writer.WritePropertyName($"{char.ToLower(name[0])}{name.AsSpan(1)}");

        writer.WriteStartArray();

        foreach (var item in obj.Items!)
        {
            writer.WriteRawValue(System.Text.Json.JsonSerializer.Serialize(item, _serializerOptions));
        }

        writer.WriteEndArray();

        writer.WriteEndObject();

        writer.Flush();

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }
}