using MediatR;
using MentorAI.Shared.Attributes;
using MentorAI.Shared.Responses;

namespace Subscription.Application.Features.SubscriptionPlan.DeletePlan;

public record DeletePlanCommand(
    [RequiredGuid] Guid PlanId
) : IRequest<ResultResponse<string>>;