namespace MyTeamsHub.Organization.API.Models.V1.Auth;

public record LoginRequestDto
{
    public string Email { get; init; }

    public string Password { get; init; }
}
