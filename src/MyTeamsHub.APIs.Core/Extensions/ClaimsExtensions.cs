using System.Security.Claims;

namespace MyTeamsHub.APIs.Core.Extensions;

public static class ClaimsExtensions
{
    public static Guid GetUserId(this IEnumerable<Claim> claims)
    {
        var userIdClaim = claims.SingleOrDefault(x => x.Type == "userId");

        if(userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userIdGuid))
        {
            return userIdGuid;
        }

        return default;
    }
}
