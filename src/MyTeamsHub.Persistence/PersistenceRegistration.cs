using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyTeamsHub.Persistence.Core.Options;
using MyTeamsHub.Persistence.Context;
using MyTeamsHub.Persistence.Core.Registers;

namespace MyTeamsHub.Persistence;

public static class PersistenceRegistration
{
    public static IServiceCollection ConfigureInfrastructurePersistence(this IServiceCollection services)
    {
        services.AddDbContext();
        services.AddRepositories<OrganizationDbContext>();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var databaseOptions = services.BuildServiceProvider().GetRequiredService<IOptions<DatabaseOptions>>().Value.Validate();

        services
            .AddDbContext<OrganizationDbContext>(options =>
            {
                options.UseSqlServer(databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                // TODO: Disable on higher environments for less logs.
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

        return services;
    }
}
