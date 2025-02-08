using HealthChecks.UI.Client;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class WebApplicationConfiguration
{
    internal static WebApplication UseWebApiPipeline(this WebApplication app)
    {
        app
            .ConfigureSwagger()
            .UseRouting()
            .UseCors("CostPolicy")
            .UseAuthentication()
            .UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
