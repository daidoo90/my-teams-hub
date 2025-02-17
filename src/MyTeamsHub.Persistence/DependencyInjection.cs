using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Application.Organizations;
using MyTeamsHub.Persistence.Context;
using MyTeamsHub.Persistence.Core.Context;
using MyTeamsHub.Persistence.Core.Options;
using MyTeamsHub.Persistence.Repositories;

namespace MyTeamsHub.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistanceInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();
        services.AddDbContext();

        services.AddScoped<IDbContext, OrganizationDbContext>();
        services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IEfDeletableRepository<>), typeof(EfDeletableRepository<>));

        return services;
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<OrganizationDbContext>();

        database.Database.Migrate();
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
