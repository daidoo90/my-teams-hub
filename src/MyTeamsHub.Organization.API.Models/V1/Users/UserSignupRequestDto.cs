namespace MyTeamsHub.Organization.API.Models.V1.Users;

public record UserSignupRequestDto
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string PhoneNumber { get; init; }

    public string Password { get; init; }
}
