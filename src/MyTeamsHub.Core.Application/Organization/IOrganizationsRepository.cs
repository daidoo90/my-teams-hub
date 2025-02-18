using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Organizations;

namespace MyTeamsHub.Core.Application.Organizations;

public interface IOrganizationsRepository : IEfRepository<OrganizationEntity>
{
    Task<Guid> CreateAsync(string name, string description, Guid userId, CancellationToken cancellationToken = default);
}
