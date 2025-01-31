using MyTeamsHub.APIs.Core.Configurations;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class WebApplicationConfiguration
{
    //public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    //{
    //    builder.Services.AddAppSettings();

    //    var configuration = builder.Configuration as IConfiguration;
    //    IAppSettings appSettings = configuration.Get<AppSettings>() ?? throw new AppSettingsException($"Can't load app settings configurations.");

    //    builder.Services.ConfigureDefaultOptions(builder.Configuration);

    //    builder.Services.ConfigurateAPIServices(appSettings);

    //    return builder;
    //}

    public static WebApplication UseWebApiPipeline(this WebApplication app)
    {
        app.ConfigureSwagger("My Teams Hub API")
        // .ConfigureHealthCheck()
        .UseStaticFiles()
        .UseRouting()
        .UseCors(CorsConfiguration.CorsPolicy)
        .UseAuthentication()
        .UseAuthorization();

        app.MapControllers();

        return app;
    }
}
