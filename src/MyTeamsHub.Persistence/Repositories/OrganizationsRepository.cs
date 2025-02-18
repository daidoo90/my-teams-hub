using MyTeamsHub.Core.Application.Organizations;
using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Persistence.Core.Context;

namespace MyTeamsHub.Persistence.Repositories;

public class OrganizationsRepository : EfRepository<OrganizationEntity>, IOrganizationsRepository
{
    public OrganizationsRepository(IDbContext context)
        : base(context)
    {
    }

    public async Task<Guid> CreateAsync(string name, string description, Guid userId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var organization = new OrganizationEntity
        {
            Name = name,
            Description = description,
            CreatedOn = now,
        };

        var team = new TeamEntity
        {
            Name = "Management Team",
            Description = "Management Team",
            CreatedOn = now,
            IsSystem = true,
            Organization = organization
        };
        organization.Teams.Add(team);

        var teamMember = new TeamMemberEntity
        {
            TeamId = team.TeamId,
            UserId = userId,
        };
        team.TeamMembers.Add(teamMember);

        Context.Set<TeamEntity>().Add(team);
        Context.Set<OrganizationEntity>().Add(organization);
        await Context.SaveChangesAsync(cancellationToken);

        return organization.OrganizationId;
    }
}
