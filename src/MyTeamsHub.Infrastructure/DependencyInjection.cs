using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MyTeamsHub.Core.Application.Interfaces;
using MyTeamsHub.Infrastructure.Cache;
using MyTeamsHub.Infrastructure.Crypto;

using StackExchange.Redis;

namespace MyTeamsHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICryptoService, CryptoService>();

        //var cacheConnectionString = configuration.GetRequiredSection(nameof(ConnectionStrings)).GetValue<string>(nameof(ConnectionStrings.Cache))!;

        services.AddCache(configuration);

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultCacheEntryOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<CacheEntryOptions>>()
            .Value ?? new CacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

        services
            .AddSingleton(defaultCacheEntryOptions)
            .AddMemoryCache()
            .AddRedisCache(configuration);

        return services;
    }

    private static IServiceCollection AddMemoryCache(this IServiceCollection services)
        => services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddStackExchangeRedisCache(options => options.Configuration = "myteamshub.redis.cache:6379")
        .AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse("myteamshub.redis.cache:6379");
            return ConnectionMultiplexer.Connect(configuration);
        })
        .AddSingleton(sp =>
        {
            var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        })
        .AddSingleton<IRedisCacheService, RedisCacheService>();
}

internal sealed class ConnectionStrings
{
    internal string Cache { get; set; }
}
