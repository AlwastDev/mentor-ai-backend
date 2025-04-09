using MentorAI.Shared;

namespace Subscription.Domain.Entities;

public class StudentSubscription : EntityBase
{
    public string StudentId { get; set; }

    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public SubscriptionPlan Plan { get; set; }
}