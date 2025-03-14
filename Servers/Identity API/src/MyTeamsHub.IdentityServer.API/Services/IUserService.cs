using MyTeamsHub.IdentityServer.API.Dtos;

namespace MyTeamsHub.IdentityServer.API.Services;

public interface IUserService
{
    Task<Guid> CreateAsync(UserSignupRequestDto newUser, CancellationToken cancellationToken);

    Task<User> TryGetAsync(string email, string password, CancellationToken cancellationToken = default);
}