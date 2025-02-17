using Microsoft.Extensions.Caching.Memory;

using MyTeamsHub.Core.Application.Interfaces.Shared;

namespace MyTeamsHub.Infrastructure.Cache;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;
    private readonly CacheEntryOptions _defaultCacheEntryOptions;

    public MemoryCacheService(
        IMemoryCache inMemoryCache,
        CacheEntryOptions defaultCacheEntryOptions)
    {
        _cache = inMemoryCache;
        _defaultCacheEntryOptions = defaultCacheEntryOptions;
    }

    /// <inheritdoc/>
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CancellationToken token = default)
        => await GetOrCreateAsync(key, factory, _defaultCacheEntryOptions, token);

    /// <inheritdoc/>
    public async Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CacheEntryOptions cacheEntryOptions, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        return await _cache.GetOrCreateAsync<T>(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = cacheEntryOptions.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = cacheEntryOptions.SlidingExpiration;
            entry.Priority = CacheItemPriority.Normal;
            return Task.FromResult(factory());
        });
    }

    /// <inheritdoc/>
    public Task<T?> GetAsync<T>(string key, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var isItemFound = _cache.TryGetValue<T>(key, out T? value);
        if (isItemFound)
        {
            return Task.FromResult(default(T?));
        }

        return Task.FromResult(value);
    }

    /// <inheritdoc/>
    public async Task SetAsync<T>(string key, T value, CancellationToken token = default)
        => await SetAsync(key, value, _defaultCacheEntryOptions, token);

    /// <inheritdoc/>
    public Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Cache value cannot be null.");
        }

        var memoryCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheEntryOptions.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = cacheEntryOptions.SlidingExpiration
        };
        _cache.Set<T>(key, value, memoryCacheEntryOptions);

        return Task.CompletedTask;
    }
}
