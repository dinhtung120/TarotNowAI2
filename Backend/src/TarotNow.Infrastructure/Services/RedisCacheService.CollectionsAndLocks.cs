using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace TarotNow.Infrastructure.Services;

public partial class RedisCacheService
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> LocalLockGuards = new(StringComparer.Ordinal);

    /// <inheritdoc />
    public async Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(member))
        {
            return;
        }

        var normalizedMember = member.Trim();
        if (_redisDatabase != null)
        {
            await _redisDatabase.SetAddAsync(key, normalizedMember);
            if (expiration.HasValue)
            {
                await _redisDatabase.KeyExpireAsync(key, expiration.Value);
            }

            return;
        }

        var list = await GetAsync<List<string>>(key, cancellationToken) ?? new List<string>();
        if (!list.Contains(normalizedMember, StringComparer.Ordinal))
        {
            list.Add(normalizedMember);
        }

        await SetAsync(key, list, expiration, cancellationToken);
    }

    /// <inheritdoc />
    public async Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(member))
        {
            return;
        }

        var normalizedMember = member.Trim();
        if (_redisDatabase != null)
        {
            await _redisDatabase.SetRemoveAsync(key, normalizedMember);
            return;
        }

        var list = await GetAsync<List<string>>(key, cancellationToken);
        if (list is null || list.Count == 0)
        {
            return;
        }

        list.RemoveAll(x => string.Equals(x, normalizedMember, StringComparison.Ordinal));
        if (list.Count == 0)
        {
            await RemoveAsync(key, cancellationToken);
            return;
        }

        await SetAsync(key, list, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            var members = await _redisDatabase.SetMembersAsync(key);
            if (members.Length == 0)
            {
                return Array.Empty<string>();
            }

            return members
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        var list = await GetAsync<List<string>>(key, cancellationToken);
        if (list is null || list.Count == 0)
        {
            return Array.Empty<string>();
        }

        return list
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

    /// <inheritdoc />
    public async Task<bool> AcquireLockAsync(
        string key,
        string owner,
        TimeSpan leaseTime,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(owner))
        {
            return false;
        }

        var normalizedOwner = owner.Trim();
        if (_redisDatabase != null)
        {
            return await _redisDatabase.StringSetAsync(key, normalizedOwner, leaseTime, When.NotExists);
        }

        var localGuard = GetLocalLockGuard(key);
        await localGuard.WaitAsync(cancellationToken);
        try
        {
            var existing = await _cache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrWhiteSpace(existing))
            {
                return false;
            }

            await _cache.SetStringAsync(
                key,
                normalizedOwner,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = leaseTime
                },
                cancellationToken);
            return true;
        }
        finally
        {
            localGuard.Release();
        }
    }

    /// <inheritdoc />
    public async Task<bool> ReleaseLockAsync(string key, string owner, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(owner))
        {
            return false;
        }

        var normalizedOwner = owner.Trim();
        if (_redisDatabase != null)
        {
            const string releaseScript = """
                if redis.call('GET', KEYS[1]) == ARGV[1] then
                    return redis.call('DEL', KEYS[1])
                end
                return 0
                """;
            var result = await _redisDatabase.ScriptEvaluateAsync(
                releaseScript,
                keys: [key],
                values: [normalizedOwner]);
            return (long)result == 1;
        }

        var localGuard = GetLocalLockGuard(key);
        await localGuard.WaitAsync(cancellationToken);
        try
        {
            var existing = await _cache.GetStringAsync(key, cancellationToken);
            if (!string.Equals(existing, normalizedOwner, StringComparison.Ordinal))
            {
                return false;
            }

            await _cache.RemoveAsync(key, cancellationToken);
            return true;
        }
        finally
        {
            localGuard.Release();
        }
    }

    private static SemaphoreSlim GetLocalLockGuard(string key)
    {
        return LocalLockGuards.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
    }
}
