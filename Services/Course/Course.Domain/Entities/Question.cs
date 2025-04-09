using MentorAI.Shared;

namespace Course.Domain.Entities;

public class Question : EntityBase
{
    public string TestId { get; set; }
    public string QuestionText { get; set; }

    public Test Test { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<StudentAnswer> StudentAnswers { get; set; }
}