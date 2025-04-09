using MentorAI.Shared;

namespace Course.Domain.Entities;

public class Roadmap : EntityBase
{
    public string StudentId { get; set; }
    public int LearningMaterialId { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDate { get; set; }

    public LearningMaterial LearningMaterial { get; set; }
}