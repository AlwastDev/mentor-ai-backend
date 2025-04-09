using MentorAI.Shared;

namespace Course.Domain.Entities;

public class StudentAnswer : EntityBase
{
    public string TestAttemptId { get; set; }
    public string QuestionId { get; set; }
    public string AnswerId { get; set; }

    public TestAttempt TestAttempt { get; set; }
    public Question Question { get; set; }
    public Answer Answer { get; set; }
}
