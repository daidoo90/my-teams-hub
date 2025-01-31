namespace MyTeamsHub.Organization.API.Models.V1.Organizations;

public record UpdateTeamRequestDto
{
    public string Name { get; init; }

    public string Description { get; init; }

    public IEnumerable<TeamMemberDto> TeamMembers { get; init; }
}

public record TeamMemberDto
{
    public Guid? TeamMemberId { get; init; }
    public string Email { get; init; }
    public bool IsLead { get; set; }
}