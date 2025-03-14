namespace MyTeamsHub.IdentityServer.API.DAL;

public class UserEntity
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

    public DateTime CreatedOn { get; set; }

    public DateTime? LastUpdatedOn { get; set; }
}


public enum UserStatus
{
    Invited = 1,
    Active = 2,
    Inactive = 3
}

public enum UserType
{
    SuperAdmin = 1,
    ClientAdmin = 2,
    ClientUser = 3
}
