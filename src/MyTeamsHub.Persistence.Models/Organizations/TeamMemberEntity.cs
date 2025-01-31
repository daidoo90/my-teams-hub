using MyTeamsHub.Persistence.Models.Types;
using MyTeamsHub.Persistence.Models.Users;

namespace MyTeamsHub.Persistence.Models.Organizations;

public class TeamMemberEntity
{
    public Guid TeamId { get; set; }
    public TeamEntity Team { get; set; } = null!;
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public RoleType MemberType { get; set; }
}
