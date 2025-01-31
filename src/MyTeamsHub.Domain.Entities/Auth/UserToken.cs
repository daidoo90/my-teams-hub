namespace MyTeamsHub.Domain.Entities.Auth;

public record UserToken
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }
}
