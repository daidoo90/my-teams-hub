using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Organizations;

namespace MyTeamsHub.Core.Application.Organization.Delete;

public sealed record DeleteOrganizationCommand(Guid OrganizationId) : ICommand;

public class DeleteOrganizationCommandHandler(IOrganizationsRepository organizationsRepository) : ICommandHandler<DeleteOrganizationCommand>
{
    private readonly IOrganizationsRepository _organizationsRepository = organizationsRepository;

    public async Task<ServiceResult> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.FirstOrDefaultAsync(o => o.OrganizationId == request.OrganizationId, cancellationToken);
        if (organization == null)
            return ServiceResult.WithError(ErrorCodes.InvalidOrganization);

        organization.IsDeleted = true;
        organization.DeletedOn = DateTime.UtcNow;

        await _organizationsRepository.UpdateAsync(organization, cancellationToken);

        return ServiceResult.Success;
    }
}
