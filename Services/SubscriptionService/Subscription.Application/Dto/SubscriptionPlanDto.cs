namespace Subscription.Application.Dto;

public class SubscriptionPlanDto
{
    public Guid Id { get; set; }
    public string PlanName { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public bool AccessToCharts { get; set; }
    public bool AccessToAISupportChat { get; set; }
    public int BonusCoins { get; set; }
    
    public IList<StudentSubscriptionDto> StudentSubscriptions { get; set; }
}