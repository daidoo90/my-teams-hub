using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyTeamsHub.Organization.Persistence.Options;

namespace MyTeamsHub.Persistence.Registers;

public static class OptionsRegister
{
    public static IServiceCollection ConfigureDatabaseOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));

    public static string GetDatabaseConnectionString(this IConfiguration configuration)
        => configuration.GetRequiredSection(nameof(DatabaseOptions)).GetValue<string>(nameof(DatabaseOptions.ConnectionString))!;
}
