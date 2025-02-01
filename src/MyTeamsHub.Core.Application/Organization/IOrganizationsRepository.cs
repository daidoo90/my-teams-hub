using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Organizations;

namespace MyTeamsHub.Domain.Services.Organizations;
public interface IOrganizationsRepository : IEfRepository<OrganizationEntity>
{
    Task<Guid> CreateAsync(string name, string description, Guid userId, CancellationToken cancellationToken = default);
}