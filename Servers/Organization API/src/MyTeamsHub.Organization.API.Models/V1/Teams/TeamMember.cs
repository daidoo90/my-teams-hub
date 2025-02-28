namespace MyTeamsHub.Organization.API.Models.V1.Teams;

public sealed record TeamMember
{
    public Guid MemberId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Status { get; init; }
    public int Role { get; init; }
}
