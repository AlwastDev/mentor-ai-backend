using Subscription.Application.Contracts.Persistence;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Subscription.Application.Features.SubscriptionPlan.DeletePlan;

public class DeletePlanHandler : IRequestHandler<DeletePlanCommand, ResultResponse<string>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public DeletePlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<ResultResponse<string>> Handle(DeletePlanCommand request,
        CancellationToken cancellationToken)
    {
        var existingPlan = await _subscriptionPlanRepository.GetPlanByIdAsync(request.PlanId);
        if (existingPlan is null)
        {
            return new(ResultCode.NotFound, "Subscription plan not found");
        }

        if (existingPlan.StudentSubscriptions.Count > 0)
        {
            return new(ResultCode.BadRequest, "Subscription plan already has student subscriptions");
        }

        var deleted = await _subscriptionPlanRepository.DeleteAsync(existingPlan.Id);

        return deleted
            ? new(ResultCode.Ok, "Subscription plan deleted successfully")
            : new(ResultCode.InternalServerError, "Failed to delete subscription plan");
    }
}