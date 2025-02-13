using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyTeamsHub.Infrastructure.Services;

namespace MyTeamsHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICryptoService, CryptoService>();

        //var cacheConnectionString = configuration.GetRequiredSection(nameof(ConnectionStrings)).GetValue<string>(nameof(ConnectionStrings.Cache))!;

        services.AddStackExchangeRedisCache(options => options.Configuration = "myteamshub.redis.cache:6379");

        return services;
    }
}

internal sealed class ConnectionStrings
{
    internal string Cache { get; set; }
}
