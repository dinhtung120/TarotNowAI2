using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TarotNow.Application.Interfaces;
using StackExchange.Redis;

namespace TarotNow.Infrastructure.Services;

// Cache service hỗ trợ cả Redis thực và fallback DistributedCache.
public class RedisCacheService : ICacheService
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

        // Fallback path: kiểm tra tồn tại trong DistributedCache (không đảm bảo atomic tuyệt đối).
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

        // Fallback path: parse giá trị hiện tại rồi tăng thủ công.
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
}
