namespace MentorAI.Shared;

public abstract class EntityBase
{
    public string Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}