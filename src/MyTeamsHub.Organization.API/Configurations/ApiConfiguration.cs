using MyTeamsHub.APIs.Core.Configurations;
using MyTeamsHub.APIs.Core.Services;
using MyTeamsHub.Domain.Services;
using MyTeamsHub.Domain.Services.Auth;
using MyTeamsHub.Domain.Services.Organizations;
using MyTeamsHub.Persistence;
using MyTeamsHub.Persistence.Registers;
using MyTeamsHub.Persistence.Repositories;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class ApiConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // builder.Services.AddAppSettings();

        var configuration = builder.Configuration as IConfiguration;
        //  IAppSettings appSettings = configuration.Get<AppSettings>() ?? throw new AppSettingsException($"Can't load app settings configurations.");

        builder.Services.ConfigureDefaultOptions(builder.Configuration);

        builder.Services.ConfigurateAPIServices();

        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<ICryptoService, CryptoService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(IIdentityService).Assembly));


        builder.Services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();

        return builder;
    }

    public static IServiceCollection ConfigurateAPIServices(this IServiceCollection services)
    {
        services.ConfigureControllers()
                       //.ConfigureServices()
                       .AddEndpointsApiExplorer()
                       .AddVersioning()
                       .AddSwagger<IApiAssembly>("My Teams Hub API")
                       .ConfigureCors()
                       //.AddAppIdentity(appSettings)
                       .AddResponseCompression(opts => opts.EnableForHttps = true);

        services.ConfigureInfrastructurePersistence();

        return services;
    }

    public static void ConfigureDefaultOptions(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<AssemblyOptions>(opt => opt.MigratorAssembly = Assembly.Load("BookingHub.Infrastructure.Persistence"));
        // services.Configure<HangfireHandlersOptions>(configuration.GetSection(nameof(HangfireHandlersOptions)));
        // services.Configure<IdentityApiClientOptions>(configuration.GetSection(nameof(IdentityApiClientOptions)));
        // services.ConfigureServiceOptions(configuration);
        // services.ConfigureRestOptions(configuration);
        // services.ConfigureLoggingOptions(configuration);
        // services.ConfigureHangfireOptions(configuration);
        services.ConfigureDatabaseOptions(configuration);
        // services.ConfigureMessagingOptions(configuration);
        // services.ConfigureApplicationInsightsOptions(configuration);
    }
}