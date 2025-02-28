using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MyTeamsHub.Core.Application.Interfaces.Shared;

using StackExchange.Redis;

namespace MyTeamsHub.Infrastructure.Cache;

internal static class DependencyInjection
{
    internal static IServiceCollection AddCache(this IServiceCollection services)
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
            .AddInMemoryCache()
            .AddRedisCache();

        return services;
    }

    private static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        => services
        .AddMemoryCache()
        .AddSingleton<IMemoryCacheService, MemoryCacheService>();

    private static IServiceCollection AddRedisCache(this IServiceCollection services)
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
