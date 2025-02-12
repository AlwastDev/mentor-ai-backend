namespace MentorAI.Shared.Responses;

public record ResponseArray<TItems>
{
    [System.Text.Json.Serialization.JsonIgnore]
    public string? Name { get; init; }

    public required IEnumerable<TItems>? Items { get; set; }
}