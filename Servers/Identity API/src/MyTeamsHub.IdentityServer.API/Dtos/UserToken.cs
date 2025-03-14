namespace MyTeamsHub.IdentityServer.API.Dtos;

public record UserToken
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }
}