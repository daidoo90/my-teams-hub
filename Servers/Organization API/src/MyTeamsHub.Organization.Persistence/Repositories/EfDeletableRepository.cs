using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Shared;
using MyTeamsHub.Organization.Persistence.Context;

namespace MyTeamsHub.Persistence.Repositories;

public class EfDeletableRepository<TEntity> : EfRepository<TEntity>, IEfDeletableRepository<TEntity>
        where TEntity : class, IDeletableEntity
{
    public EfDeletableRepository(IDbContext context)
        : base(context)
    {
    }

    public override IQueryable<TEntity> Query()
        => Set.AsQueryable().Where(x => !x.IsDeleted);

    public async Task SoftRemoveAsync(TEntity entity)
    {
        Audit(entity);

        Set.Update(entity);

        await Context.SaveChangesAsync();
    }

    public async Task SoftRemoveRangeAsync(params TEntity[] entities)
    {
        foreach (var entity in entities)
        {
            Audit(entity);
        }

        Set.UpdateRange(entities);

        await Context.SaveChangesAsync();
    }

    private void Audit(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
    }
}
