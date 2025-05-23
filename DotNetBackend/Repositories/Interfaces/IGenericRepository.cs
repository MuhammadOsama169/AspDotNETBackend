using System.Linq.Expressions;

namespace DotNetBackend.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByGuidAsync(Guid id, CancellationToken ct = default);
    Task<T?> GetByIdAsync(object id, CancellationToken ct = default);

    Task<T>AddAsync(T entity, CancellationToken ct = default);
    Task RemoveAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    IQueryable<T> Query();  
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default
    );

    Task<IReadOnlyList<T>> GetListAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken ct = default
    );
}