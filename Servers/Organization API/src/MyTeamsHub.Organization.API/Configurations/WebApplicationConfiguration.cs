using MyTeamsHub.Organization.Persistence;
using MyTeamsHub.SignalR.Users.Hub;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class WebApplicationConfiguration
{
    internal static WebApplication UseWebApiPipeline(this WebApplication app)
    {
        app
            .UseCors("AllowSpecificOrigin")
            .UseRouting();
        //.UseAuthentication()
        //.UseAuthorization();

        app.ConfigureSwagger();

        app.MapControllers();

        app.MapHealthChecks("/health");

        app.Services.ApplyMigrations();

        app.MapHub<UserNotificationHub>("/users-notifications");

        return app;
    }
}
