using MentorAI.Shared;

namespace Subscription.Domain.Entities;

public class SubscriptionPlan : EntityBase
{
    public string PlanName { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public bool AccessToCharts { get; set; }
    public bool AccessToAISupportChat { get; set; }
    public int BonusCoins { get; set; }
    
    public ICollection<StudentSubscription> StudentSubscriptions { get; set; }
}