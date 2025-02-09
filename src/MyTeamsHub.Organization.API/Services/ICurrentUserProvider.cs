using System.Security.Claims;

namespace MyTeamsHub.Organization.API.Services;

/// <summary>
/// Service that exposes current user's metadata
/// </summary>
public interface ICurrentUserProvider
{
    /// <summary>
    /// Current user identifier
    /// </summary>
    Guid CurrentUserId { get; }

    /// <summary>
    /// Collection of claims
    /// </summary>
    IEnumerable<Claim> Claims { get; }
}
