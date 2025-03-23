using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Organization.Persistence.Context;

namespace MyTeamsHub.Organization.API.Services.Organization;

public class OrganizationQuery
{
    [UseFiltering]
    [UseSorting]
    [GraphQLName("organizations")]
    public IQueryable<OrganizationEntity> GetOrganizations([Service] IDbContext context)
        => context.Set<OrganizationEntity>()
        .AsQueryable()
        .Include(o => o.Teams);
}
