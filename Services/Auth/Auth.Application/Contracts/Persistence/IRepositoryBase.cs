using System.Linq.Expressions;

namespace Auth.Application.Contracts.Persistence;

public interface IRepositoryBase<K, T>
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<K, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<K, bool>>? predicate = null,
        Func<IQueryable<K>, IOrderedQueryable<K>>? orderBy = null,
        string? includeString = null,
        bool disableTracking = true);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<K, bool>>? predicate = null,
        Func<IQueryable<K>, IOrderedQueryable<K>>? orderBy = null,
        List<Expression<Func<K, object>>>? includes = null,
        bool disableTracking = true);
    Task<T?> GetByIdAsync(string id);
    Task<T> AddAsync(K dto);
    Task UpdateAsync(K dto);
    Task DeleteAsync(K dto);
    Task<bool> CommitAsync();
    Task<bool> CommitAsync(int n);
}