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

public class OrganizationType : ObjectType<OrganizationEntity>
{
    protected override void Configure(IObjectTypeDescriptor<OrganizationEntity> descriptor)
    {
        descriptor.Ignore(u => u.IsDeleted);
        descriptor.Ignore(u => u.DeletedOn);
        descriptor.Ignore(u => u.LastUpdatedOn);
        descriptor.Ignore(u => u.CreatedOn);
        descriptor.Ignore(u => u.Description);
        descriptor.Ignore(u => u.LogoBase64);

        descriptor.Field(u => u.Teams)
                .Type<ListType<TeamType>>();
    }
}

public class TeamType : ObjectType<TeamEntity>
{
    protected override void Configure(IObjectTypeDescriptor<TeamEntity> descriptor)
    {
        //descriptor.Field(u => u.IsSystem).Authorize();

        descriptor.Ignore(u => u.IsDeleted);
        descriptor.Ignore(u => u.DeletedOn);
        descriptor.Ignore(u => u.LastUpdatedOn);
        descriptor.Ignore(u => u.CreatedOn);

        descriptor.Ignore(u => u.Organization);
        descriptor.Ignore(u => u.TeamMembers);
        descriptor.Ignore(u => u.OrganizationId);
    }
}
