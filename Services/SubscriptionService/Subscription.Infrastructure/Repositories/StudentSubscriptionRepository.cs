using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Dto;
using Subscription.Domain.Entities;
using Subscription.Infrastructure.Persistence;

namespace Subscription.Infrastructure.Repositories;

public class StudentSubscriptionRepository : RepositoryBase<StudentSubscription, StudentSubscriptionDto>,
    IStudentSubscriptionRepository
{
    private readonly SubscriptionContext _context;
    private readonly IMapper _mapper;

    public StudentSubscriptionRepository(
        SubscriptionContext context,
        IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<bool> IsSubscriptionActiveAsync(string studentId)
    {
        return await _context.StudentSubscriptions
            .AnyAsync(s => s.StudentId == studentId && s.EndDate > DateTime.UtcNow);
    }
    
    public async Task<StudentSubscriptionDto> AddSubscriptionAsync(StudentSubscriptionDto dto)
    {
        var entity = _mapper.Map<StudentSubscription>(dto);
        await _context.StudentSubscriptions.AddAsync(entity);
        await CommitAsync();
        return _mapper.Map<StudentSubscriptionDto>(entity);
    }
    
    public async Task<bool> RemoveSubscriptionAsync(int subscriptionId)
    {
        var entity = await _context.StudentSubscriptions.FindAsync(subscriptionId);
        if (entity == null) return false;

        _context.StudentSubscriptions.Remove(entity);
        return await CommitAsync();
    }
    
    public async Task<StudentSubscriptionDto?> UpdateSubscriptionAsync(StudentSubscriptionDto dto)
    {
        var entity = await _context.StudentSubscriptions
            .FirstOrDefaultAsync(s => s.Id == dto.Id);

        if (entity == null) return null;

        entity.PlanId = dto.PlanId;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;

        _context.StudentSubscriptions.Update(entity);
        await CommitAsync();
        return _mapper.Map<StudentSubscriptionDto>(entity);
    }

    public async Task<StudentSubscriptionDto?> GetActiveSubscriptionByStudentIdAsync(string studentId)
    {
        var subscription = await _context.StudentSubscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.StudentId == studentId && s.EndDate > DateTime.UtcNow);

        return _mapper.Map<StudentSubscriptionDto?>(subscription);
    }
}