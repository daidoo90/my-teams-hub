namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public sealed record GetUserOrganizationResponseDto(Guid OrganizationId, string OrganizationName, int Role);