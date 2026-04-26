using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Threading;

namespace TarotNow.Infrastructure.Services;

public partial class RedisCacheService
{
    private static readonly ConcurrentDictionary<string, LocalLockGuardEntry> LocalLockGuards = new(StringComparer.Ordinal);
    private static readonly TimeSpan LocalLockGuardIdleTtl = TimeSpan.FromMinutes(10);
    private const int LocalLockGuardCleanupEvery = 256;
    private static long _localLockGuardTouchCounter;

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
            ReleaseLocalLockGuard(key, localGuard);
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
            ReleaseLocalLockGuard(key, localGuard);
        }
    }

    private static SemaphoreSlim GetLocalLockGuard(string key)
    {
        var entry = LocalLockGuards.GetOrAdd(key, _ => new LocalLockGuardEntry());
        entry.Touch();
        TryCleanupLocalLockGuards();
        return entry.Semaphore;
    }

    private static void ReleaseLocalLockGuard(string key, SemaphoreSlim localGuard)
    {
        localGuard.Release();
        if (LocalLockGuards.TryGetValue(key, out var entry))
        {
            entry.Touch();
        }
    }

    private static void TryCleanupLocalLockGuards()
    {
        if (Interlocked.Increment(ref _localLockGuardTouchCounter) % LocalLockGuardCleanupEvery != 0)
        {
            return;
        }

        var nowUtc = DateTime.UtcNow;
        foreach (var pair in LocalLockGuards)
        {
            var entry = pair.Value;
            if (entry.Semaphore.CurrentCount != 1 || !entry.IsExpired(nowUtc, LocalLockGuardIdleTtl))
            {
                continue;
            }

            _ = LocalLockGuards.TryRemove(pair.Key, out _);
        }
    }

    private sealed class LocalLockGuardEntry
    {
        public SemaphoreSlim Semaphore { get; } = new(1, 1);

        private long _lastTouchedUtcTicks = DateTime.UtcNow.Ticks;

        public void Touch()
        {
            Interlocked.Exchange(ref _lastTouchedUtcTicks, DateTime.UtcNow.Ticks);
        }

        public bool IsExpired(DateTime nowUtc, TimeSpan ttl)
        {
            var touchedTicks = Interlocked.Read(ref _lastTouchedUtcTicks);
            var touchedUtc = new DateTime(touchedTicks, DateTimeKind.Utc);
            return nowUtc - touchedUtc > ttl;
        }
    }
}
