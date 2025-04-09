using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Dto;
using Subscription.Domain.Entities;
using Subscription.Infrastructure.Persistence;

namespace Subscription.Infrastructure.Repositories;

public class SubscriptionPlanRepository : RepositoryBase<SubscriptionPlan, SubscriptionPlanDto>,
    ISubscriptionPlanRepository
{
    private readonly SubscriptionContext _context;
    private readonly IMapper _mapper;

    public SubscriptionPlanRepository(
        SubscriptionContext context,
        IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SubscriptionPlanDto> AddAsync(SubscriptionPlanDto dto)
    {
        var entity = _mapper.Map<SubscriptionPlan>(dto);
        _context.SubscriptionPlans.Add(entity);
        await CommitAsync();
        return _mapper.Map<SubscriptionPlanDto>(entity);
    }

    public async Task<List<SubscriptionPlanDto>> GetAllPlansAsync()
    {
        var plans = await _context.SubscriptionPlans.ToListAsync();
        return _mapper.Map<List<SubscriptionPlanDto>>(plans);
    }

    public async Task<SubscriptionPlanDto?> GetPlanByIdAsync(Guid id)
    {
        var plan = await _context.SubscriptionPlans
            .Include(sp => sp.StudentSubscriptions)
            .FirstOrDefaultAsync(p => p.Id == id);
        return _mapper.Map<SubscriptionPlanDto?>(plan);
    }

    public async Task<bool> PlanExistsAsync(Guid id)
    {
        return await _context.SubscriptionPlans.AnyAsync(p => p.Id == id);
    }

    public async Task<SubscriptionPlanDto?> UpdateAsync(SubscriptionPlanDto dto)
    {
        var existing = await _context.SubscriptionPlans.FindAsync(dto.Id);
        if (existing == null) return null;

        existing.PlanName = dto.PlanName;
        existing.Price = dto.Price;
        existing.DurationDays = dto.DurationDays;
        existing.AccessToCharts = dto.AccessToCharts;
        existing.AccessToAISupportChat = dto.AccessToAISupportChat;
        existing.BonusCoins = dto.BonusCoins;

        _context.SubscriptionPlans.Update(existing);
        await CommitAsync();

        return _mapper.Map<SubscriptionPlanDto>(existing);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var plan = await _context.SubscriptionPlans.FindAsync(id);
        if (plan == null) return false;

        _context.SubscriptionPlans.Remove(plan);
        return await CommitAsync();
    }
}