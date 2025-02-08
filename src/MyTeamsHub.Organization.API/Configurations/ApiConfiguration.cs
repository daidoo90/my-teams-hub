using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

using MyTeamsHub.Core.Application;
using MyTeamsHub.Organization.API.Services;
using MyTeamsHub.Persistence;
using MyTeamsHub.Persistence.Core.Options;
using MyTeamsHub.Persistence.Registers;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class ApiConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        // builder.Services.AddAppSettings();
        //var configuration = builder.Configuration as IConfiguration;
        //  IAppSettings appSettings = configuration.Get<AppSettings>() ?? throw new AppSettingsException($"Can't load app settings configurations.");

        var databaseOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<DatabaseOptions>>().Value.Validate();

        builder.Services.ConfigureDefaultOptions(builder.Configuration);

        builder.Services
            .AddAPIServices()
            .AddApplication()
            .AddInfrastructure()
            .AddHealthChecks()
            .AddSqlServer(databaseOptions.ConnectionString);

        return builder;
    }

    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddControllers();

        services
            .AddEndpointsApiExplorer()
            .AddHttpContextAccessor()
            .AddVersioning()
            .AddSwagger()
            .ConfigureCors()
            .AddResponseCompression(opts => opts.EnableForHttps = true);

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        return services
            .AddApiVersioning(opts =>
            {
                opts.DefaultApiVersion = new ApiVersion(1, 0);
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.ReportApiVersions = true;
                opts.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Version"),
                        new MediaTypeApiVersionReader("ver"));
            })
            .AddVersionedApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            });
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("CostPolicy",
                builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Patch, HttpMethods.Options));
        });
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
