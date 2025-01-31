namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public record GetTeamsRequestDto
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
