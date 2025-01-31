using System.Net;

namespace MyTeamsHub.IntegrationTests.Core.Extensions;

public static class HttpStatusCodeExtensioncs
{
    public static void ShouldBeOk(this HttpStatusCode statusCode)
    {
        Assert.Equal(statusCode, HttpStatusCode.OK);
    }
}
