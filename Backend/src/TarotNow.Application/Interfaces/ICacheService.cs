/*
 * ===================================================================
 * FILE: ICacheService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Khuôn Đúc Cho Công Cụ Tạm Nhớ (Cache).
 *   Cung cấp Cơ Chế Thắt Cổ Chai (Rate Limiting) để chống phá hoại, ấn nút điên cuồng.
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface Cung Cấp Hòm Chứa Tạm (Redis/ Memory Cache).
/// Tại sao lại vẽ bạt mạng ra Interface này thay vì Xài Đồ Có Sẵn (IDistributedCache)?
/// -> Phía Application Đừng Quan Tâm Code Bên Trong Viết Bằng Redis Khỉ Mốc Gì. (Clean Arch).
/// -> Để Tự Độ Chế Được Hàm Chặn Spam Đặc Thù (CheckRateLimit) (Đồ Gốc Của Microsoft Không Có).
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Thò tay vào Kho xem có Lưu cái Chìa Khóa Này Không. Không Có Thì Báo Null.
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gửi Đồ Cất Kho Kèm Nhãn Dán Hết Hạn (Mấy Tiếng Sau Vứt Đi).
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rảnh Tay Xóa Mẹ Đồ Bỏ Đi (Bất Cứ Lúc Nào Muốn).
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tường Lửa Ngăn Lũ (Spam Rate Limiting). Bắt IP Thằng Gõ Phím Siêu Tốc Đi Xa.
    /// Nếu ID Khách Lạ Hoắc -> Cho Qua Trạm Đóng Dấu Rớt Thời Gian Chờ (Return True).
    /// Nếu Nó Vừa Vào Vài Giây Xong Lại Nhấn Tiếp (Chưa Hết Hạn Dấu Redis) -> Đóng Cửa Thả Chó (Return False).
    /// </summary>
    Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Nhồi Điểm Danh Mỗi Bước Chân Đi Vô. Tiện Dụng Quản Lý Số Lượt Hỏi Bằng Đếm Bộ Đếm Sống.
    /// </summary>
    Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
}
