using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Domain.Organizations;

public class TeamMemberEntity
{
    public Guid TeamId { get; set; }
    public TeamEntity Team { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public RoleType MemberType { get; set; }
}
