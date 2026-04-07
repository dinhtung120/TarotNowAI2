

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TarotNow.Application.Interfaces;
using StackExchange.Redis;

namespace TarotNow.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    
    private readonly IDatabase? _redisDatabase;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer? redisConnection = null)
    {
        _cache = cache;
        _redisDatabase = redisConnection?.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true 
        };
    }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null) return default; 

        return JsonSerializer.Deserialize<T>(data, _jsonOptions);
    }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        var json = JsonSerializer.Serialize(value, _jsonOptions);
        await _cache.SetStringAsync(key, json, options, cancellationToken);
    }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

        public async Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            
            return await _redisDatabase.StringSetAsync(key, "1", expiry: limitWindow, when: When.NotExists);
        }

        
        var existing = await _cache.GetStringAsync(key, cancellationToken);
        if (existing != null)
        {
            return false; 
        }

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = limitWindow
        };

        await _cache.SetStringAsync(key, "1", options, cancellationToken);
        return true; 
    }

        public async Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            
            var value = await _redisDatabase.StringIncrementAsync(key);
            
            if (value == 1 && expiration.HasValue)
            {
                await _redisDatabase.KeyExpireAsync(key, expiration);
            }

            return value;
        }

        
        var valString = await _cache.GetStringAsync(key, cancellationToken);
        long currentVal = 0;

        if (valString != null && long.TryParse(valString, out var parsed))
        {
            currentVal = parsed;
        }

        currentVal++;

        var options = new DistributedCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        await _cache.SetStringAsync(key, currentVal.ToString(), options, cancellationToken);
        return currentVal;
    }
}
