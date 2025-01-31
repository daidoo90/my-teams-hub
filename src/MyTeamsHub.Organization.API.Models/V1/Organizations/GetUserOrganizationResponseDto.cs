namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public record GetUserOrganizationResponseDto
{
    public Guid OrganizationId { get; init; }

    public string OrganizationName { get; init; }

    public int Role { get; init; }
}
