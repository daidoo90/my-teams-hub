using Microsoft.EntityFrameworkCore;
using MyTeamsHub.Domain.Entities.Organizations;
using MyTeamsHub.Domain.Services.Common;

namespace MyTeamsHub.Domain.Services.Organizations;

public interface IOrganizationService
{
    Task<ServiceDataResult<Guid>> CreateAsync(string name, string description, Guid userId, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<IEnumerable<OrganizationMember>>> GetAllAsync(Guid teamMemberId, CancellationToken cancellationToken = default);

    Task<ServiceResult> DeleteAsync(Guid organizationId, CancellationToken cancellationToken = default);
}

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public OrganizationService(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<ServiceDataResult<Guid>> CreateAsync(string name, string description, Guid userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganizationName);

        if (string.IsNullOrEmpty(description))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganizationDescription);

        var organization = await _organizationsRepository.FirstOrDefaultAsync(x => x.Name.Contains(name));
        if (organization != null &&
           string.Equals(organization.Name, name, StringComparison.OrdinalIgnoreCase))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.OrganizationAlreadyExists);

        var newOrgId = await _organizationsRepository.CreateAsync(name, description, userId, cancellationToken);

        return ServiceDataResult<Guid>.Created(newOrgId);
    }

    public async Task<ServiceResult> DeleteAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var organization = await _organizationsRepository.FirstOrDefaultAsync(o => o.OrganizationId == organizationId, cancellationToken);
        if (organization == null)
            return ServiceResult.WithError(ErrorCodes.InvalidOrganization);

        organization.IsDeleted = true;
        organization.DeletedOn = DateTime.UtcNow;

        await _organizationsRepository.UpdateAsync(organization, cancellationToken);

        return ServiceResult.Success;
    }

    public async Task<ServiceDataResult<IEnumerable<OrganizationMember>>> GetAllAsync(Guid teamMemberId, CancellationToken cancellationToken = default)
    {
        var organizations = await _organizationsRepository.Query()
            .Include(o => o.Teams)
            .ThenInclude(t => t.TeamMembers)
            .Where(o => o.Teams.Any(t => t.TeamMembers.Any(m => m.UserId == teamMemberId)) && !o.IsDeleted)
            .Select(o => new OrganizationMember
            {
                OrganizationId = o.OrganizationId,
                OrganizationName = o.Name,
                Role = (int)o.Teams
                        .SelectMany(t => t.TeamMembers)
                        .Where(m => m.UserId == teamMemberId)
                        .Select(m => m.MemberType)
                        .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<OrganizationMember>>.WithData(organizations);
    }
}