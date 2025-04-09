using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorAI.Shared.Responses;

public record CreatePlanCommand(
    [Required(ErrorMessage = "Назва плану обов’язкова")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Назва плану не повинна містити спеціальних символів")]
    string PlanName,

    [Required, Range(0, double.MaxValue, ErrorMessage = "Ціна повинна бути не менше 0")]
    decimal Price,

    [Required, Range(0, int.MaxValue, ErrorMessage = "Тривалість повинна бути не менше 0")]
    int DurationDays,

    [Required] bool AccessToCharts,
    [Required] bool AccessToAISupportChat,

    [Required, Range(0, int.MaxValue, ErrorMessage = "Бонусні монети повинні бути не менше 0")]
    int BonusCoins
) : IRequest<ResultResponse<string>>;