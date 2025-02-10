using MyTeamsHub.Persistence;

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

        app.MapHealthChecks();

        app.Services.TryMigrate();

        return app;
    }
}
