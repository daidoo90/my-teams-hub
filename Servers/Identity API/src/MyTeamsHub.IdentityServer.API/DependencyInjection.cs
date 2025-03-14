using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyTeamsHub.IdentityServer.API.DAL;

namespace MyTeamsHub.IdentityServer.API;

public static class DependencyInjection
{
    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        database.Database.Migrate();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var databaseOptions = services.BuildServiceProvider().GetRequiredService<IOptions<DatabaseOptions>>().Value.Validate();

        services
            .AddDbContext<IdentityDbContext>(options =>
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

public class DatabaseOptions
{
    public string ConnectionString { get; set; }

    public bool EnableSensitiveDataLogging { get; set; }

    public bool EnableDetailedErrors { get; set; }

    public bool EnableDatabaseMigrations { get; set; }

    public bool EnableSeedData { get; set; }

    public bool EnableSeedTestData { get; set; }

    public DatabaseOptions Validate()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new ArgumentNullException(nameof(ConnectionString));
        }

        return this;
    }
}
