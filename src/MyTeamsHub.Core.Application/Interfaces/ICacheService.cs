namespace MyTeamsHub.Core.Application.Interfaces;

public interface ICacheService
{
    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="T">The type of the object to get.</typeparam>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>New entry</returns>
    Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CancellationToken token = default);

    /// <summary>
    /// Gets the value associated with this key if it exists, or generates a new entry using the provided key and a value from the given factory if the key is not found.
    /// </summary>
    /// <typeparam name="T">The type of the object to get.</typeparam>
    /// <param name="key">The key of the entry to look for or create.</param>
    /// <param name="factory">The factory that creates the value associated with this key if the key does not exist in the cache.</param>
    /// <param name="cacheEntryOptions">The options to be applied to the ICacheEntry if the key does not exist in the cache.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>New entry</returns>
    Task<T?> GetOrCreateAsync<T>(string key, Func<T> factory, CacheEntryOptions cacheEntryOptions, CancellationToken token = default);

    Task<T?> GetAsync<T>(string key, CancellationToken token = default);

    Task SetAsync<T>(string key, T value, CancellationToken token = default);

    Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions, CancellationToken token = default);
}

public sealed class CacheEntryOptions
{
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    public TimeSpan? SlidingExpiration { get; set; }
}
