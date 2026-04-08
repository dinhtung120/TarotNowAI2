

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract thao tác cache để giảm tải truy vấn và hỗ trợ các cơ chế giới hạn tần suất.
public interface ICacheService
{
    /// <summary>
    /// Lấy dữ liệu từ cache theo khóa để tận dụng dữ liệu đã có sẵn.
    /// Luồng xử lý: tra cứu key trong cache và trả về null khi chưa có hoặc đã hết hạn.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi dữ liệu vào cache để tái sử dụng ở các request tiếp theo.
    /// Luồng xử lý: lưu value theo key và áp TTL nếu expiration được cung cấp.
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa dữ liệu cache theo khóa khi cần làm mới hoặc hủy dữ liệu cũ.
    /// Luồng xử lý: định vị key và loại bỏ record khỏi cache store.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra rate limit theo khóa để ngăn request vượt tần suất cho phép.
    /// Luồng xử lý: đánh giá số lần truy cập trong limitWindow và trả true khi còn trong ngưỡng.
    /// </summary>
    Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tăng bộ đếm cache nhằm theo dõi số lần phát sinh sự kiện theo khóa.
    /// Luồng xử lý: cộng dồn counter hiện tại, đồng thời thiết lập TTL nếu có expiration.
    /// </summary>
    Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
}
