using HealthChecks.UI.Client;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class WebApplicationConfiguration
{
    //public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    //{
    //    builder.Services.AddAppSettings();

    //    var configuration = builder.Configuration as IConfiguration;
    //    //IAppSettings appSettings = configuration.Get<AppSettings>() ?? throw new AppSettingsException($"Can't load app settings configurations.");

    //    builder.Services.ConfigureDefaultOptions(builder.Configuration);

    //    builder.Services.ConfigurateAPIServices(appSettings);

    //    return builder;
    //}

    public static WebApplication UseWebApiPipeline(this WebApplication app)
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
