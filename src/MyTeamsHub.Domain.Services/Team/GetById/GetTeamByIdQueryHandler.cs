using Microsoft.EntityFrameworkCore;
using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Organizations;

namespace MyTeamsHub.Domain.Services.Team.GetById;

public sealed record GetTeamByIdQuery(Guid OrganizationId, Guid TeamId) : IQuery<TeamEntity>;

public class GetTeamByIdQueryHandler(IEfRepository<TeamEntity> teams) : IQueryHandler<GetTeamByIdQuery, TeamEntity>
{
    private readonly IEfRepository<TeamEntity> _teams = teams;

    public async Task<ServiceDataResult<TeamEntity>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        var team = await _teams.Query()
             .Include(t => t.TeamMembers)
             .ThenInclude(tm => tm.User)
             .FirstOrDefaultAsync(t => t.OrganizationId == request.OrganizationId && t.TeamId == request.TeamId, cancellationToken);

        return ServiceDataResult<TeamEntity>.WithData(team);
    }
}
