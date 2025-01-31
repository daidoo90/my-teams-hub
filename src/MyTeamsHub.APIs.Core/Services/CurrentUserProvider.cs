using Microsoft.AspNetCore.Http;
using MyTeamsHub.APIs.Core.Extensions;
using System.Security.Claims;

namespace MyTeamsHub.APIs.Core.Services;

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