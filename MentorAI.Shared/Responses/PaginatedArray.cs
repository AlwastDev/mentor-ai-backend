namespace MentorAI.Shared.Responses;

public record PaginatedArray<TItems> : ResponseArray<TItems>
{
    public int TotalCount { get; set; }
}