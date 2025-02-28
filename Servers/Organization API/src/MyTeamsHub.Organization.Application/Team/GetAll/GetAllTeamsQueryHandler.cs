using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Application.Organizations;
using MyTeamsHub.Core.Domain.Organizations;

namespace MyTeamsHub.Domain.Services.Team.GetAll;

public sealed record GetAllTeamsQuery(Guid OrganizationId, int PageNumber, int PageSize) : IQuery<(IEnumerable<TeamEntity>, int)>;

public class GetAllTeamsQueryHandler(
    IEfRepository<TeamEntity> teams,
    IOrganizationsRepository organizationsRepository) : IQueryHandler<GetAllTeamsQuery, (IEnumerable<TeamEntity>, int)>
{
    private readonly IEfRepository<TeamEntity> teams = teams;
    private readonly IOrganizationsRepository _organizations = organizationsRepository;

    public async Task<ServiceDataResult<(IEnumerable<TeamEntity>, int)>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        if (request.OrganizationId == Guid.Empty)
        {
            return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organizationExists = await _organizations.AnyAsync(o => o.OrganizationId == request.OrganizationId);
        if (!organizationExists)
        {
            return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organization = await _organizations.Query()
            .Include(o => o.Teams)
            .ThenInclude(x => x.TeamMembers)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(o => o.OrganizationId == request.OrganizationId, cancellationToken);

        var teams = organization.Teams
            .Where(t => !t.IsDeleted);

        var totalTeams = teams.Count();

        return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithData((teams.
            Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize), totalTeams));
    }
}
