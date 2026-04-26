using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TarotNow.Application.Interfaces;
using StackExchange.Redis;

namespace TarotNow.Infrastructure.Services;

// Cache service hỗ trợ cả Redis thực và fallback DistributedCache.
public partial class RedisCacheService : ICacheService
{
    // Abstraction cache chung cho đọc/ghi dữ liệu tuần tự hóa.
    private readonly IDistributedCache _cache;
    // Redis database native để tận dụng lệnh atomic khi có kết nối.
    private readonly IDatabase? _redisDatabase;
    // Tùy chọn JSON dùng xuyên suốt để serialize/deserialize nhất quán.
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Khởi tạo cache service và tùy chọn kết nối Redis native.
    /// Luồng fallback tự động sang DistributedCache giúp hệ thống vẫn chạy khi thiếu Redis multiplexer.
    /// </summary>
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
    /// Lấy giá trị cache theo key và deserialize về kiểu đích.
    /// Luồng trả mặc định khi key không tồn tại để caller xử lý cache miss rõ ràng.
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null) return default;

        return JsonSerializer.Deserialize<T>(data, _jsonOptions);
    }

    /// <summary>
    /// Ghi giá trị cache theo key với thời gian hết hạn tùy chọn.
    /// Luồng serialize JSON trước khi ghi để thống nhất cách lưu giữa các kiểu dữ liệu.
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
    /// Xóa key khỏi cache.
    /// Luồng này dùng cho các tình huống invalidate sau khi state nghiệp vụ thay đổi.
    /// </summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra và đánh dấu rate-limit theo cửa sổ thời gian.
    /// Luồng dùng Redis SET NX để đảm bảo atomic; khi không có Redis sẽ fallback kiểm tra tuần tự.
    /// </summary>
    public async Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            // Redis path: atomic set-if-not-exists để tránh race condition giữa nhiều instance.
            return await _redisDatabase.StringSetAsync(key, "1", expiry: limitWindow, when: When.NotExists);
        }

        var localGuard = GetLocalLockGuard(key);
        await localGuard.WaitAsync(cancellationToken);
        try
        {
            // Fallback path: khóa theo key để giữ tính nhất quán trên node hiện tại.
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
        finally
        {
            ReleaseLocalLockGuard(key, localGuard);
        }
    }

    /// <summary>
    /// Tăng bộ đếm theo key và trả về giá trị mới.
    /// Luồng ưu tiên Redis INCR atomic; fallback local cache khi không có kết nối Redis.
    /// </summary>
    public async Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            // Redis path: INCR bảo toàn atomic khi có nhiều request đồng thời.
            var value = await _redisDatabase.StringIncrementAsync(key);

            if (value == 1 && expiration.HasValue)
            {
                // Chỉ set TTL ở lần tạo đầu để giữ semantics bộ đếm theo cửa sổ.
                await _redisDatabase.KeyExpireAsync(key, expiration);
            }

            return value;
        }

        var localGuard = GetLocalLockGuard(key);
        await localGuard.WaitAsync(cancellationToken);
        try
        {
            // Fallback path: khóa theo key để tránh lost update trên cùng instance.
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
        finally
        {
            ReleaseLocalLockGuard(key, localGuard);
        }
    }

}
