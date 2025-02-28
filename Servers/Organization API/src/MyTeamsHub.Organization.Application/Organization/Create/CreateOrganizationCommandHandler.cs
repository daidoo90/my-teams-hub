using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Organizations;

namespace MyTeamsHub.Core.Application.Organization.Create;

public sealed record CreateOrganizationCommand(string Name, string Description, Guid UserId) : ICommand<Guid>;

public class CreateOrganizationCommandHandler(IOrganizationsRepository organizationsRepository) : ICommandHandler<CreateOrganizationCommand, Guid>
{
    private readonly IOrganizationsRepository _organizationsRepository = organizationsRepository;

    public async Task<ServiceDataResult<Guid>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganizationName);

        if (string.IsNullOrEmpty(request.Description))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganizationDescription);

        var organization = await _organizationsRepository.FirstOrDefaultAsync(x => x.Name.Contains(request.Name));
        if (organization != null &&
           string.Equals(organization.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.OrganizationAlreadyExists);

        var newOrgId = await _organizationsRepository.CreateAsync(request.Name, request.Description, request.UserId, cancellationToken);

        return ServiceDataResult<Guid>.Created(newOrgId);
    }
}
