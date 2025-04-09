using Subscription.Application.Dto;

namespace Subscription.Application.Contracts.Persistence;

public interface IStudentSubscriptionRepository
{
    Task<bool> IsSubscriptionActiveAsync(string studentId);
    Task<StudentSubscriptionDto> AddSubscriptionAsync(StudentSubscriptionDto dto);
    Task<bool> RemoveSubscriptionAsync(int subscriptionId);
    Task<StudentSubscriptionDto?> UpdateSubscriptionAsync(StudentSubscriptionDto dto);
    Task<StudentSubscriptionDto?> GetActiveSubscriptionByStudentIdAsync(string studentId);
    
}