using StackExchange.Redis;

namespace MyTeamsHub.Infrastructure.Cache;

public static class IDatabaseExtensions
{
    internal static async Task ReleaseLockAsync(this IDatabase database, string lockKey, string lockToken)
    {
        var luaScript = @"
                if redis.call('get', KEYS[1]) == ARGV[1] then
                    return redis.call('del', KEYS[1])
                else
                    return 0
                end"
        ;

        await database.ScriptEvaluateAsync(luaScript, [lockKey], [lockToken]);
    }
}
