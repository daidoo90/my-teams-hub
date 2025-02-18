using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Shared;
using MyTeamsHub.Persistence.Core.Context;

namespace MyTeamsHub.Persistence.Repositories;

public class EfRepository<TEntity> : IEfRepository<TEntity>
        where TEntity : class
{
    public EfRepository(IDbContext context)
    {
        Context = context;
        Context.ChangeTracker.LazyLoadingEnabled = false;
        Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    protected IDbContext Context { get; }

    public virtual IQueryable<TEntity> Query()
        => Set.AsQueryable();

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => Query().CountAsync(cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> wherePredicate, CancellationToken cancellationToken = default)
        => Query().CountAsync(wherePredicate, cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> wherePredicate, CancellationToken cancellationToken = default)
        => Query().AnyAsync(wherePredicate, cancellationToken);

    public Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selectPredicate, CancellationToken cancellationToken = default)
        => Query().Select(selectPredicate).ToListAsync(cancellationToken);

    public Task<List<TResult>> GetByAsync<TResult>(
        Expression<Func<TEntity, bool>> wherePredicate,
        Expression<Func<TEntity, TResult>> selectPredicate,
        CancellationToken cancellationToken = default)
            => Query().Where(wherePredicate).Select(selectPredicate).ToListAsync(cancellationToken);

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> wherePredicate, CancellationToken cancellationToken = default)
        => Query().Where(wherePredicate).FirstOrDefaultAsync<TEntity?>(cancellationToken);

    public Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>> wherePredicate,
        Expression<Func<TEntity, TResult>> selectPredicate,
        CancellationToken cancellationToken = default)
        where TResult : class
            => Query().Where(wherePredicate).Select(selectPredicate).FirstOrDefaultAsync<TResult?>(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Audit(entity, EntityState.Added);

        Set.Add(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(params TEntity[] entities)
    {
        foreach (var entity in entities)
        {
            Audit(entity, EntityState.Added);
        }

        Set.AddRange(entities);

        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Audit(entity, EntityState.Modified);

        Set.Update(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            Audit(entity, EntityState.Modified);
        }

        Set.UpdateRange(entities);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task HardRemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Remove(entity);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task HardRemoveRangeAsync(params TEntity[] entities)
    {
        Set.RemoveRange(entities);

        await Context.SaveChangesAsync();
    }

    private void Audit(TEntity entity, EntityState entityState)
    {
        if (entity is IAuditableEntity)
        {
            var auditableEntity = (IAuditableEntity)entity;

            if (entityState == EntityState.Added)
            {
                auditableEntity.CreatedOn = DateTime.UtcNow;
            }
            else if (entityState == EntityState.Modified)
            {
                auditableEntity.LastUpdatedOn = DateTime.UtcNow;
            }
        }
    }
}
