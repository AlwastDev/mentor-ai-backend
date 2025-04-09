using MentorAI.Shared;

namespace Course.Domain.Entities;

public class Answer : EntityBase
{
    public string QuestionId { get; set; }
    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }

    public Question Question { get; set; }
}