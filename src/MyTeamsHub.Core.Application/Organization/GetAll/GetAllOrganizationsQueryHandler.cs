using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Organizations;
using MyTeamsHub.Core.Domain.Organizations;

namespace MyTeamsHub.Core.Application.Organization.GetAll;

public sealed record GetAllOrganizationsQuery(Guid TeamMemberId) : IQuery<IEnumerable<OrganizationMember>>;

public class GetAllOrganizationsQueryHandler(IOrganizationsRepository organizationsRepository) : IQueryHandler<GetAllOrganizationsQuery, IEnumerable<OrganizationMember>>
{
    private readonly IOrganizationsRepository _organizationsRepository = organizationsRepository;

    public async Task<ServiceDataResult<IEnumerable<OrganizationMember>>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.Query()
            .Include(o => o.Teams)
            .ThenInclude(t => t.TeamMembers)
            .Where(o => o.Teams.Any(t => t.TeamMembers.Any(m => m.UserId == request.TeamMemberId)) && !o.IsDeleted)
            .Select(o => new OrganizationMember
            {
                OrganizationId = o.OrganizationId,
                OrganizationName = o.Name,
                Role = (int)o.Teams
                        .SelectMany(t => t.TeamMembers)
                        .Where(m => m.UserId == request.TeamMemberId)
                        .Select(m => m.MemberType)
                        .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<OrganizationMember>>.WithData(organizations);
    }
}
