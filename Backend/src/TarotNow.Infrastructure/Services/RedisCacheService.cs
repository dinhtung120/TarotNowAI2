using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TarotNow.Application.Interfaces;
using StackExchange.Redis;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implementation của ICacheService sử dụng Redis (thông qua IDistributedCache).
/// </summary>
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

    /// <summary>
    /// Lấy object từ Redis, deserialize từ byte[] sang T.
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null) return default;

        return JsonSerializer.Deserialize<T>(data, _jsonOptions);
    }

    /// <summary>
    /// Serialize object sang JSON string -> UTF8 bytes và lưu vào Redis.
    /// </summary>
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

    /// <summary>
    /// Xóa key khỏi Redis.
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra Rate Limit sử dụng cơ chế "Set if not exists" với TTL.
    /// 
    /// Logic:
    /// - Key thường có dạng: "ratelimit:{userId}:{action}"
    /// - Nếu key chưa có -> Set key với giá trị bất kỳ và TTL = limitWindow -> return true (Allowed).
    /// - Nếu key đã có -> return false (Blocked).
    /// 
    /// Tại sao dùng SetStringAsync thay vì Increment?
    /// -> Vì yêu cầu Phase 1 chỉ cần block request tiếp theo trong 1 khoảng thời gian (spam protection),
    ///    chưa cần đếm số lượng request phức tạp (sliding window).
    /// </summary>
    public async Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            // Atomic NX set: chỉ set khi key chưa tồn tại.
            return await _redisDatabase.StringSetAsync(key, "1", expiry: limitWindow, when: When.NotExists);
        }

        // Kiểm tra xem key đã tồn tại chưa
        var existing = await _cache.GetStringAsync(key, cancellationToken);
        if (existing != null)
        {
            return false; // Đã bị limit
        }

        // Nếu chưa, set key với TTL
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = limitWindow
        };

        // Lưu giá trị "1" làm flag
        await _cache.SetStringAsync(key, "1", options, cancellationToken);
        return true; // Được phép qua
    }

    /// <summary>
    /// Tăng giá trị số trong cache. 
    /// Lưu ý: IDistributedCache không hỗ trợ Increment atomic trực tiếp (cần dùng Lua script hoặc StackExchange.Redis trực tiếp).
    /// Trong Phase 1, ta tạm dùng Get -> Parse -> Set vì quota count không yêu cầu độ chính xác tuyệt đối 100% 
    /// như tài chính (wallet).
    /// </summary>
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
