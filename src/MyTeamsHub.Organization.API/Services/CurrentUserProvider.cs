using System.Security.Claims;

using MyTeamsHub.Organization.API.Extensions;

namespace MyTeamsHub.Organization.API.Services;

/// <inheritdoc/>
public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor
    /// </summary>
    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public Guid CurrentUserId => _httpContextAccessor.HttpContext.User.Claims.GetUserId();

    /// <inheritdoc/>
    public IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User.Claims ?? [];
}
