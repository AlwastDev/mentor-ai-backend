using MentorAI.Shared;

namespace Course.Domain.Entities;

public class TestAttempt : EntityBase
{
    public string StudentId { get; set; }
    public string TestId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
    public float? Score { get; set; }

    public Test Test { get; set; }
    public ICollection<StudentAnswer> StudentAnswers { get; set; }
}