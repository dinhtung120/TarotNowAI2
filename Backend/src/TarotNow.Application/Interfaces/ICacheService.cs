namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface cung cấp dịch vụ caching tập trung và quản lý rate limiting.
/// 
/// Tại sao dùng interface này thay vì IDistributedCache trực tiếp?
/// - Tách biệt logic caching khỏi Application layer (Clean Architecture).
/// - Cung cấp các method đặc thù cho business như CheckRateLimit (không có sẵn trong IDistributedCache).
/// - Dễ dàng chuyển đổi giữa Redis, In-Memory hoặc các provider khác mà không sửa code Application.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Lấy dữ liệu từ cache.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu dữ liệu vào cache với thời gian hết hạn (TTL).
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa dữ liệu khỏi cache.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra Rate Limit cho một action dựa trên Key.
    /// Trả về true nếu request được phép, false nếu bị chặn do quá giới hạn tốc độ.
    /// 
    /// Logic: Nếu key chưa tồn tại => lưu key với TTL và return true. Nếu tồn tại => return false.
    /// </summary>
    Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tăng một giá trị số trong cache (dùng cho counter, quota).
    /// </summary>
    Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
}
