namespace MyTeamsHub.Organization.API.Models.V1.Teams;

public sealed record GetTeamsResponseDto
{
    public int Total { get; init; }
    public ICollection<Team> Teams { get; init; } = [];
}