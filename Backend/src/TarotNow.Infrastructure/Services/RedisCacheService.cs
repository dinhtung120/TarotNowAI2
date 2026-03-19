/*
 * FILE: RedisCacheService.cs
 * MỤC ĐÍCH: Implementation cache service sử dụng Redis (hoặc fallback InMemory).
 *
 *   CÁC CHỨC NĂNG:
 *   → GetAsync<T>: lấy object từ cache (deserialize JSON → T)
 *   → SetAsync<T>: lưu object vào cache (serialize T → JSON, có TTL)
 *   → RemoveAsync: xóa key
 *   → CheckRateLimitAsync: rate limiting đơn giản (Set If Not Exists + TTL)
 *   → IncrementAsync: tăng giá trị số (dùng cho quota counting)
 *
 *   KIẾN TRÚC DUAL-MODE:
 *   → Redis available (production): dùng StackExchange.Redis trực tiếp
 *   → Redis unavailable (local dev): fallback IDistributedCache (InMemory)
 *   → Mỗi method kiểm tra _redisDatabase != null → chọn path phù hợp.
 *
 *   RATE LIMITING:
 *   → Dùng NX (Not Exists) set: key chưa có → set + return true (allowed)
 *   → Key đã có (trong TTL window) → return false (blocked)
 *   → Phase 1 chỉ block/allow, Phase 2 có thể upgrade lên sliding window counter.
 *
 *   INCREMENT:
 *   → Redis: dùng StringIncrementAsync (atomic INCR) + set TTL lần đầu.
 *   → InMemory fallback: Get → Parse → +1 → Set (KHÔNG atomic, chấp nhận cho quota).
 */

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TarotNow.Application.Interfaces;
using StackExchange.Redis;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implement ICacheService — dual-mode Redis/InMemory cache.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    // _redisDatabase: null nếu Redis không khả dụng → fallback IDistributedCache
    private readonly IDatabase? _redisDatabase;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer? redisConnection = null)
    {
        _cache = cache;
        _redisDatabase = redisConnection?.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // JSON key case-insensitive deserialization
        };
    }

    /// <summary>
    /// Lấy object từ cache, deserialize JSON bytes → T.
    /// Trả về default(T) nếu key không tồn tại (cache miss).
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null) return default; // Cache miss

        return JsonSerializer.Deserialize<T>(data, _jsonOptions);
    }

    /// <summary>
    /// Lưu object vào cache: serialize T → JSON string → UTF8 bytes → Redis/InMemory.
    /// expiration: thời gian sống (TTL). Null = không hết hạn (permanent).
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

    /// <summary>Xóa key khỏi cache.</summary>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }

    /// <summary>
    /// RATE LIMITING — kiểm tra User có bị giới hạn tần suất hay không.
    ///
    /// CÁCH HOẠT ĐỘNG:
    /// → Key dạng: "ratelimit:{userId}:{action}" (vd: "ratelimit:abc:login")
    /// → Key chưa có → SET key + TTL → trả true (ALLOWED)
    /// → Key đã có (trong window) → trả false (BLOCKED)
    ///
    /// REDIS PATH (atomic):
    /// → StringSetAsync(NX): atomic "Set If Not Exists" → thread-safe
    ///
    /// FALLBACK PATH (InMemory):
    /// → Get → kiểm tra null → Set → không atomic nhưng chấp nhận cho dev
    /// </summary>
    public async Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            // Atomic NX: chỉ set khi key CHƯA tồn tại + tự hết hạn sau limitWindow
            return await _redisDatabase.StringSetAsync(key, "1", expiry: limitWindow, when: When.NotExists);
        }

        // Fallback InMemory (không atomic, chấp nhận cho local dev)
        var existing = await _cache.GetStringAsync(key, cancellationToken);
        if (existing != null)
        {
            return false; // Đã bị limit trong window
        }

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = limitWindow
        };

        await _cache.SetStringAsync(key, "1", options, cancellationToken);
        return true; // Được phép qua
    }

    /// <summary>
    /// TĂNG giá trị số trong cache — dùng cho quota counting (vd: đếm số lần gọi AI/ngày).
    ///
    /// REDIS PATH (atomic):
    /// → StringIncrementAsync: Redis INCR command (atomic) → thread-safe
    /// → Set TTL lần đầu (value == 1) → auto-reset khi hết ngày/giờ
    ///
    /// FALLBACK PATH (InMemory):
    /// → Get → Parse → +1 → Set → KHÔNG atomic
    /// → Chấp nhận cho quota count (không yêu cầu chính xác tuyệt đối như tài chính)
    /// </summary>
    public async Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (_redisDatabase != null)
        {
            // Atomic increment
            var value = await _redisDatabase.StringIncrementAsync(key);
            // Set TTL lần đầu (khi counter mới tạo)
            if (value == 1 && expiration.HasValue)
            {
                await _redisDatabase.KeyExpireAsync(key, expiration);
            }

            return value;
        }

        // Fallback: Get → +1 → Set (không atomic)
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
