using MyTeamsHub.Organization.API.Models.V1.Organizations;

namespace MyTeamsHub.Organization.API.Models.V1.Teams;

public sealed record UpdateTeamRequestDto(string Name, string Description, IEnumerable<TeamMemberDto> TeamMembers);

