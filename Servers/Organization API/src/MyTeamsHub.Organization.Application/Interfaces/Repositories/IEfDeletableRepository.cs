using MyTeamsHub.Core.Domain.Shared;

namespace MyTeamsHub.Core.Application.Interfaces.Repositories;

public interface IEfDeletableRepository<TEntity> : IEfRepository<TEntity>
        where TEntity : class, IDeletableEntity
{
    Task SoftRemoveAsync(TEntity entity);

    Task SoftRemoveRangeAsync(params TEntity[] entities);
}
