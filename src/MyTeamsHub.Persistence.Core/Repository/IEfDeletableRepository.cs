using MyTeamsHub.Persistence.Core.Entities;

namespace MyTeamsHub.Persistence.Core.Repository;

public interface IEfDeletableRepository<TEntity> : IEfRepository<TEntity>
        where TEntity : class, IDeletableEntity
{
    Task SoftRemoveAsync(TEntity entity);

    Task SoftRemoveRangeAsync(params TEntity[] entities);
}