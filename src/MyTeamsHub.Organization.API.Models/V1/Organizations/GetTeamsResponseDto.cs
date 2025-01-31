namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public record GetTeamsResponseDto
{
    public int Total { get; init; }
    public ICollection<Team> Teams { get; init; } = [];
}

public record Team
{
    public Guid TeamId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public IList<TeamMember> TeamMembers { get; init; }

}

public record TeamMember
{
    public Guid MemberId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Status { get; set; }
    public int Role { get; set; }
}


