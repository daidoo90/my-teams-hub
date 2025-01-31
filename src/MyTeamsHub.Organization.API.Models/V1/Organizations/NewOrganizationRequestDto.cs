namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public record NewOrganizationRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
