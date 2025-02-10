using Microsoft.Extensions.DependencyInjection;

using MyTeamsHub.Infrastructure.Services;

namespace MyTeamsHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string cacheConnectionString)
    {
        services.AddScoped<ICryptoService, CryptoService>();
        services.AddStackExchangeRedisCache(options => options.Configuration = cacheConnectionString);

        return services;
    }
}
