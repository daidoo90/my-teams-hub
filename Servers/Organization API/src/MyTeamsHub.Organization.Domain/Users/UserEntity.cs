using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Core.Domain.Shared;

namespace MyTeamsHub.Core.Domain.Users;

public class UserEntity : IAuditableEntity
{
    public Guid UserId { get; set; }

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PasswordHashBase64 { get; set; }

    public string? PasswordSaltBase64 { get; set; }

    public DateTime? PasswordExpiresOn { get; set; }

    public string? RoleTitle { get; set; } = default;

    public UserStatus UserStatus { get; set; }

    public UserType UserType { get; set; }

    public ICollection<TeamMemberEntity> TeamMembers { get; set; } = [];

    public DateTime CreatedOn { get; set; }

    public DateTime? LastUpdatedOn { get; set; }
}
