namespace MyTeamsHub.Core.Domain.Auth;

public record UserToken
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }
}
