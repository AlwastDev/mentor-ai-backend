using Subscription.Application.Contracts.Persistence;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;
using Subscription.Application.Dto;

namespace Subscription.Application.Features.SubscriptionPlan.CreatePlan;

public class CreatePlanHandler : IRequestHandler<CreatePlanCommand, ResultResponse<string>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public CreatePlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<ResultResponse<string>> Handle(CreatePlanCommand request,
        CancellationToken cancellationToken)
    {
        var planDto = new SubscriptionPlanDto
        {
            PlanName = request.PlanName,
            Price = request.Price,
            DurationDays = request.DurationDays,
            AccessToCharts = request.AccessToCharts,
            AccessToAISupportChat = request.AccessToAISupportChat,
            BonusCoins = request.BonusCoins
        };

        var createdPlan = await _subscriptionPlanRepository.AddAsync(planDto);

        return new(ResultCode.Created, createdPlan.Id.ToString());
    }
}