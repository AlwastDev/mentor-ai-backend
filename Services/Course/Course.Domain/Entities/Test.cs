using MentorAI.Shared;

namespace Course.Domain.Entities;

public class Test : EntityBase
{
    public string TestName { get; set; }
    public string? Description { get; set; }
    public bool IsEntryTest { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<LearningMaterial> LearningMaterials { get; set; }
    public ICollection<TestAttempt> TestAttempts { get; set; }
}