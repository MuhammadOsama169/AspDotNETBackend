// Repositories/GenericRepository.cs

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetBackend.Data;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T>             _dbSet;

        public GenericRepository(ApplicationDbContext db)
        {
            _db    = db;
            _dbSet = db.Set<T>();
        }

        public async Task<T?> GetByGuidAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, ct)
                .ConfigureAwait(false);
        }
        public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default) =>
            await _dbSet.FindAsync(new object[]{ id }, ct)
                .ConfigureAwait(false);

        
        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            var entry = await _db.Set<T>().AddAsync(entity, ct);
            return entry.Entity;
        }

        public async Task RemoveAsync(T entity, CancellationToken ct = default)
        {
            _db.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }
        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
        public Task<T?> FirstOrDefaultAsync(
            Expression<Func<T,bool>> predicate,
            CancellationToken ct = default
        )
        {
            return _dbSet.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T,bool>> predicate,
            CancellationToken ct = default)
        {
            var list = await _dbSet.Where(predicate)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            return list; 
        }
    }
}