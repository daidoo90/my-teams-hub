using System.Security.Claims;

using MyTeamsHub.Organization.API.Extensions;

namespace MyTeamsHub.Organization.API.Services;

public interface ICurrentUserProvider
{
    Guid CurrentUserId { get; }

    IEnumerable<Claim> Claims { get; }
}

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CurrentUserId => _httpContextAccessor.HttpContext.User.Claims.GetUserId();

    public IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User.Claims ?? Enumerable.Empty<Claim>();
}
