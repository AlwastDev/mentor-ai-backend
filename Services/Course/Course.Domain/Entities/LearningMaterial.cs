using MentorAI.Shared;

namespace Course.Domain.Entities;

public class LearningMaterial : EntityBase
{
    public string TestId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Test Test { get; set; }
}