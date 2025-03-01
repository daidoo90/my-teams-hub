using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Core.Domain.Users;
using MyTeamsHub.Organization.Persistence.Context;
using MyTeamsHub.Organization.Persistence.Extensions;
using MyTeamsHub.Persistence.Configurators;

namespace MyTeamsHub.Persistence.Context;

public class OrganizationDbContext : DbContext, IDbContext
{
    public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrganizationEntity> Organization { get; set; }
    public DbSet<TeamEntity> Team { get; set; }
    public DbSet<TeamMemberEntity> TeamMember { get; set; }
    public DbSet<UserEntity> User { get; set; }

    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw ex.GetDetailedDbUpdateException();
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw ex.GetDetailedDbUpdateException();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
    }
}
