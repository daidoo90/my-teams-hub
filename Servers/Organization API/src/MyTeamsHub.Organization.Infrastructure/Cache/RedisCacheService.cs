using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

using MyTeamsHub.Core.Application.Interfaces.Shared;

using StackExchange.Redis;

namespace MyTeamsHub.Infrastructure.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly IDatabase _database;
    private readonly CacheEntryOptions _defaultCacheEntryOptions;

    public RedisCacheService(
        IDistributedCache distributedCache,
        IDatabase database,
        CacheEntryOptions defaultCacheEntryOptions)
    {
        _cache = distributedCache;
        _database = database;
        _defaultCacheEntryOptions = defaultCacheEntryOptions;
    }

    /// <inheritdoc/>
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CancellationToken token = default)
        => await GetOrCreateAsync(key, factory, _defaultCacheEntryOptions, token);

    /// <inheritdoc/>
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CacheEntryOptions cacheEntryOptions, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var cacheValue = await _cache.GetStringAsync(key, token);
        if (cacheValue is not null)
        {
            try
            {
                var result = JsonSerializer.Deserialize<T>(cacheValue);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
                // Fallback to regeneration
                await _cache.RemoveAsync(key, token);
            }
        }

        var lockKey = $"lock:{key}";
        var lockToken = Guid.NewGuid().ToString();
        if (await _database.StringSetAsync(lockKey, lockToken, TimeSpan.FromSeconds(10), When.NotExists))
        {
            try
            {
                cacheValue = await _cache.GetStringAsync(key, token);
                if (cacheValue is not null)
                {
                    return JsonSerializer.Deserialize<T>(cacheValue);
                }

                var newValue = factory();
                if (newValue is not null)
                {
                    var newCacheValueSerialized = JsonSerializer.Serialize(newValue);

                    var distributedCacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cacheEntryOptions.AbsoluteExpirationRelativeToNow,
                        SlidingExpiration = cacheEntryOptions.SlidingExpiration
                    };
                    await _cache.SetStringAsync(key, newCacheValueSerialized, distributedCacheEntryOptions, token);
                }

                return newValue;
            }
            finally
            {
                await _database.ReleaseLockAsync(lockKey, lockToken);
            }
        }
        else
        {
            // Wait and retry
            await Task.Delay(100, token);
            return await GetOrCreateAsync(key, factory, cacheEntryOptions, token);
        }
    }

    /// <inheritdoc/>
    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var cacheValue = await _cache.GetStringAsync(key, token);
        if (cacheValue is not null)
        {
            try
            {
                var result = JsonSerializer.Deserialize<T>(cacheValue);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
                await _cache.RemoveAsync(key, token);
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public async Task SetAsync<T>(string key, T value, CancellationToken token = default)
        => await SetAsync(key, value, _defaultCacheEntryOptions, token);

    /// <inheritdoc/>
    public async Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Cache value cannot be null.");
        }

        var newCacheValueSezialized = JsonSerializer.Serialize(value);

        var distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheEntryOptions.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = cacheEntryOptions.SlidingExpiration
        };
        await _cache.SetStringAsync(key, newCacheValueSezialized, distributedCacheEntryOptions, token);
    }
}
