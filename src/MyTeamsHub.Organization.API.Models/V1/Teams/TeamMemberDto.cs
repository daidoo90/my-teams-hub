namespace MyTeamsHub.Organization.API.Models.V1.Teams;

public sealed record TeamMemberDto(Guid? TeamMemberId, string Email, bool IsLead);