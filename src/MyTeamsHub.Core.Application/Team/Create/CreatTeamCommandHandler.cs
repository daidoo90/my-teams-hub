using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Organizations;

namespace MyTeamsHub.Domain.Services.Team.Create;

public sealed record CreateTeamCommand(Guid Organizationid, string TeamName, string Description) : ICommand<Guid>;

public class CreateTeamCommandHandler(
    IEfRepository<TeamEntity> teams,
    IEfRepository<OrganizationEntity> organizations) : ICommandHandler<CreateTeamCommand, Guid>
{
    private readonly IEfRepository<TeamEntity> _teams = teams;
    private readonly IEfRepository<OrganizationEntity> _organizations = organizations;

    public async Task<ServiceDataResult<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        if (request.Organizationid.Equals(Guid.Empty))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organization = await _organizations
            .Query()
            .Include(o => o.Teams)
            .FirstOrDefaultAsync(o => o.OrganizationId == request.Organizationid, cancellationToken);

        if (organization == null)
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganization);
        }

        if (string.IsNullOrEmpty(request.TeamName))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidTeamName);
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidTeamDescription);
        }

        var team = organization.Teams.FirstOrDefault(t => t.Name.Contains(request.TeamName));
        if (team != null &&
            string.Equals(team.Name, request.TeamName, StringComparison.OrdinalIgnoreCase))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.TeamAlreadyExists);
        }

        var newTeam = new TeamEntity
        {
            OrganizationId = request.Organizationid,
            Name = request.TeamName,
            Description = request.Description
        };

        await _teams.AddAsync(newTeam);
        return ServiceDataResult<Guid>.Created(newTeam.TeamId);
    }
}
