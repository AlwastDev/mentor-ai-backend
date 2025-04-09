namespace Subscription.Application.Dto;

public class StudentSubscriptionDto
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public SubscriptionPlanDto Plan { get; set; }
}