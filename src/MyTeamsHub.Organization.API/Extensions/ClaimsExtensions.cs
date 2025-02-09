using System.Security.Claims;

namespace MyTeamsHub.Organization.API.Extensions;

internal static class ClaimsExtensions
{
    internal static Guid GetUserId(this IEnumerable<Claim> claims)
    {
        var userIdClaim = claims.SingleOrDefault(x => x.Type == "userId");

        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userIdGuid))
        {
            return userIdGuid;
        }

        return default;
    }
}
