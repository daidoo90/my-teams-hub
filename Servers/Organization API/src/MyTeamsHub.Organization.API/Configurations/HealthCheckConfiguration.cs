using HealthChecks.UI.Client;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class HealthCheckConfiguration
{
    internal static WebApplication MapHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
