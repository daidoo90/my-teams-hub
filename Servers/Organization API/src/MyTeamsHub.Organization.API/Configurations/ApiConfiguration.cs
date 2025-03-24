using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

using MyTeamsHub.Core.Application;
using MyTeamsHub.Infrastructure;
using MyTeamsHub.Organization.API.Services;
using MyTeamsHub.Organization.API.Services.Organization;
using MyTeamsHub.Organization.Notifications;
using MyTeamsHub.Organization.Persistence;
using MyTeamsHub.Persistence.Registers;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class ApiConfiguration
{
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureDefaultOptions(builder.Configuration);

        builder.Services
            .AddAPIServices()
            .AddApplication()
            .AddInfrastructure(builder.Configuration)
            .AddPersistanceInfrastructure()
            .AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetDatabaseConnectionString());

        builder.Services
            .AddGraphQLServer()
            .AddQueryType<OrganizationQuery>()
            .AddType<OrganizationType>()
            .AddType<TeamType>()
            .AddFiltering()
            .AddSorting();

        return builder;
    }

    private static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddControllers();

        services
            .AddEndpointsApiExplorer()
            .AddHttpContextAccessor()
            .AddVersioning()
            .AddSwagger()
            .ConfigureCors()
            .AddResponseCompression(opts => opts.EnableForHttps = true)
            .AddSignalRServices();

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

    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            builder.SetIsOriginAllowed(origin => string.IsNullOrEmpty(origin) || origin.StartsWith("http://localhost:4001") || origin.StartsWith("http://localhost:4000"))
                       .AllowCredentials()
                       .AllowAnyHeader()
                       .AllowAnyMethod());
        });
    }

    private static void ConfigureDefaultOptions(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<AssemblyOptions>(opt => opt.MigratorAssembly = Assembly.Load("BookingHub.Infrastructure.Persistence"));
        // services.Configure<HangfireHandlersOptions>(configuration.GetSection(nameof(HangfireHandlersOptions)));
        // services.Configure<IdentityApiClientOptions>(configuration.GetSection(nameof(IdentityApiClientOptions)));
        // services.ConfigureServiceOptions(configuration);
        // services.ConfigureRestOptions(configuration);
        // services.ConfigureLoggingOptions(configuration);
        // services.ConfigureHangfireOptions(configuration);
        services.ConfigureDatabaseOptions(configuration);

        // services.ConfigureApplicationInsightsOptions(configuration);
    }
}
