using Subscription.Application.Dto;

namespace Subscription.Application.Contracts.Persistence;

public interface ISubscriptionPlanRepository
{
    Task<SubscriptionPlanDto> AddAsync(SubscriptionPlanDto dto);
    Task<List<SubscriptionPlanDto>> GetAllPlansAsync();
    Task<SubscriptionPlanDto?> GetPlanByIdAsync(Guid id);
    Task<bool> PlanExistsAsync(Guid id);
    Task<SubscriptionPlanDto?> UpdateAsync(SubscriptionPlanDto dto);
    Task<bool> DeleteAsync(Guid id);
}