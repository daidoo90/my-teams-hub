using System.Linq.Expressions;

namespace MyTeamsHub.Core.Application.Interfaces.Repositories;

public interface IEfRepository<TEntity>
        where TEntity : class
{
    IQueryable<TEntity> Query();

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> countPredicate,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> anyPredicate,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selectPredicate,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetByAsync<TResult>(
        Expression<Func<TEntity, bool>> wherePredicate,
        Expression<Func<TEntity, TResult>> selectPredicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> wherePredicate,
        CancellationToken cancellationToken = default);

    Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>> wherePredicate,
        Expression<Func<TEntity, TResult>> selectPredicate,
        CancellationToken cancellationToken = default)
            where TResult : class;

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(params TEntity[] entities);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task HardRemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task HardRemoveRangeAsync(params TEntity[] entities);
}
