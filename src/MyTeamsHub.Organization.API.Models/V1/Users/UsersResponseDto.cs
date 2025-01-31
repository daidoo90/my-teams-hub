namespace MyTeamsHub.Organization.API.Models.V1.Users;

public record UsersResponseDto
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; }
}
