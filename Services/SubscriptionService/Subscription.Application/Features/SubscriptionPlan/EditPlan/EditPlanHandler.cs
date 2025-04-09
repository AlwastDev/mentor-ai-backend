using Subscription.Application.Contracts.Persistence;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;

namespace Subscription.Application.Features.SubscriptionPlan.EditPlan;

public class EditPlanHandler : IRequestHandler<EditPlanCommand, ResultResponse<string>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public EditPlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<ResultResponse<string>> Handle(EditPlanCommand request,
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

        existingPlan.PlanName = request.PlanName;
        existingPlan.Price = request.Price;
        existingPlan.DurationDays = request.DurationDays;
        existingPlan.AccessToCharts = request.AccessToCharts;
        existingPlan.AccessToAISupportChat = request.AccessToAISupportChat;
        existingPlan.BonusCoins = request.BonusCoins;

        var updated = await _subscriptionPlanRepository.UpdateAsync(existingPlan);

        return updated is not null
            ? new(ResultCode.Ok, "Subscription plan updated successfully")
            : new(ResultCode.InternalServerError, "Failed to update subscription plan");
    }
}