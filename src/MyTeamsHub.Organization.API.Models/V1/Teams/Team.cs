namespace MyTeamsHub.Organization.API.Models.V1.Teams;

public sealed record Team
{
    public Guid TeamId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public IList<TeamMember> TeamMembers { get; init; }
}