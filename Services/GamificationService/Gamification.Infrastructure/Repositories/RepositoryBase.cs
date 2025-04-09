using System.Linq.Expressions;
using AutoMapper;
using Gamification.Application.Contracts.Persistence;
using Gamification.Infrastructure.Persistence;
using MentorAI.Shared;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Repositories;

public class RepositoryBase<K, T> : IRepositoryBase<K, T>
    where K : EntityBase
    where T : class
{
    private readonly GamificationContext _context;
    private readonly IMapper _mapper;

    public RepositoryBase(GamificationContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        var entities = await _context.Set<K>().ToListAsync();
        return _mapper.Map<IReadOnlyList<T>>(entities);
    }

    public async Task<T?> GetAsync(Expression<Func<K, bool>> predicate)
    {
        var entity = await _context.Set<K>().Where(predicate).FirstOrDefaultAsync();
        return _mapper.Map<T?>(entity);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<K, bool>>? predicate = null,
        Func<IQueryable<K>, IOrderedQueryable<K>>? orderBy = null, string? includeString = null,
        bool disableTracking = true)
    {
        IQueryable<K> query = _context.Set<K>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        var result = orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        return _mapper.Map<IReadOnlyList<T>>(result);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<K, bool>>? predicate = null,
        Func<IQueryable<K>, IOrderedQueryable<K>>? orderBy = null, List<Expression<Func<K, object>>>? includes = null,
        bool disableTracking = true)
    {
        IQueryable<K> query = _context.Set<K>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        var result = orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        return _mapper.Map<IReadOnlyList<T>>(result);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var entity = await _context.Set<K>().FindAsync(id);
        return _mapper.Map<T?>(entity);
    }

    public async Task<T> AddAsync(K dto)
    {
        var entity = _mapper.Map<K>(dto);
        _context.Set<K>().Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<T>(entity);
    }

    public async Task UpdateAsync(K dto)
    {
        var entity = _mapper.Map<K>(dto);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(K dto)
    {
        var entity = _mapper.Map<K>(dto);
        _context.Set<K>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> CommitAsync() => await _context.SaveChangesAsync() > 0;
    public async Task<bool> CommitAsync(int n) => await _context.SaveChangesAsync() == n;
}