/*
 * ===================================================================
 * FILE: INotificationRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Máy Tạo Dấu Chấm Đỏ Hình Chuông: Đẩy Xuống CSDL Lịch Sử Thông Báo Push (Notifications).
 *   Phân Tầng Ngôn Ngữ Notification Thành Vi, En, Zh Theo Định Dạng Json Để Chơi Cả Thế Giới Đọc Hiểu.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Giao Diện Ổ Chứa Tự Động Xử Lý Đám Tin Rác Khách Không Đọc. 
/// MongoDb đã Setup Time-To-Live TTL Xóa Auto 30 ngày Tránh Tràn Rác Lệnh Này Gửi Vô Tội Vạ.
/// </summary>
public interface INotificationRepository
{
    /// <summary>Rung Chuông: Thảy 1 Tin Rác Quà Tặng Đòi Tiền Mới Vào Nhà Ai Đó.</summary>
    Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Phân Lọc Thư Mục Báo: Box Tin Mới Trồi Lên Trình Duyệt Bằng Nhau Tránh Đứng Web Đọc Đứt Gánh Nữa Chừng Chống Quá Load (Limit Paginate).
    /// </summary>
    Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>Lệnh Triệt Tiêu Ấn Nút Dấu Đỏ: "Click Mở Ra Chữ Thường Chìm Nghỉm (Has Read)".</summary>
    Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Đánh dấu toàn bộ thông báo của User thành đã đọc (Mark All As Read).</summary>
    Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Bộ Đếm Chứa Chỉ Số To Chìm Nền Đỏ (Bạn Đang Có 99+ Thông Báo Rợn Người Kèm Chưa Mở Cửa Tủ Đọc Đo).</summary>
    Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>Tấm Viết Bảng Bơm Dữ Liệu Tự Do Định Dạng Tiếng (Multilingual Bell Data).</summary>
public class NotificationCreateDto
{
    public Guid UserId { get; set; }
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string BodyVi { get; set; } = string.Empty;
    public string BodyEn { get; set; } = string.Empty;
    public string BodyZh { get; set; } = string.Empty;
    public string Type { get; set; } = "system"; // VD event, payment...
    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>Đóng Gói Dành Cho Phía Ống Nối Dò Chuyển Về Font End.</summary>
public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string BodyVi { get; set; } = string.Empty;
    public string BodyEn { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
