
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý notification để lưu, truy vấn và cập nhật trạng thái đã đọc.
public interface INotificationRepository
{
    /// <summary>
    /// Tạo notification mới trước khi phát sự kiện realtime đến client.
    /// Luồng xử lý: persist notification đầu vào vào kho dữ liệu.
    /// </summary>
    Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách notification của người dùng có phân trang và lọc trạng thái đọc.
    /// Luồng xử lý: lọc theo userId/isRead, áp page/pageSize và trả items cùng tổng số.
    /// </summary>
    Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu một notification đã đọc để đồng bộ badge và danh sách.
    /// Luồng xử lý: định vị theo notificationId/userId, cập nhật cờ đọc và trả kết quả.
    /// </summary>
    Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu toàn bộ notification của người dùng là đã đọc.
    /// Luồng xử lý: cập nhật hàng loạt theo userId và trả true nếu có thay đổi.
    /// </summary>
    Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số notification chưa đọc để hiển thị badge thông báo.
    /// Luồng xử lý: thống kê theo userId và trả tổng số bản ghi unread.
    /// </summary>
    Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default);
}

// DTO đầu vào khi tạo notification mới.
public class NotificationCreateDto
{
    // Định danh người nhận thông báo.
    public Guid UserId { get; set; }

    // Tiêu đề tiếng Việt.
    public string TitleVi { get; set; } = string.Empty;

    // Tiêu đề tiếng Anh.
    public string TitleEn { get; set; } = string.Empty;

    // Tiêu đề tiếng Trung.
    public string TitleZh { get; set; } = string.Empty;

    // Nội dung tiếng Việt.
    public string BodyVi { get; set; } = string.Empty;

    // Nội dung tiếng Anh.
    public string BodyEn { get; set; } = string.Empty;

    // Nội dung tiếng Trung.
    public string BodyZh { get; set; } = string.Empty;

    // Loại thông báo để client điều hướng hành vi hiển thị.
    public string Type { get; set; } = "system";

    // Metadata mở rộng theo loại thông báo.
    public Dictionary<string, string>? Metadata { get; set; }
}

// DTO đầu ra notification phục vụ hiển thị trên client.
public class NotificationDto
{
    // Định danh notification.
    public string Id { get; set; } = string.Empty;

    // Định danh người nhận.
    public Guid UserId { get; set; }

    // Tiêu đề tiếng Việt.
    public string TitleVi { get; set; } = string.Empty;

    // Tiêu đề tiếng Anh.
    public string TitleEn { get; set; } = string.Empty;

    // Nội dung tiếng Việt.
    public string BodyVi { get; set; } = string.Empty;

    // Nội dung tiếng Anh.
    public string BodyEn { get; set; } = string.Empty;

    // Loại thông báo.
    public string Type { get; set; } = string.Empty;

    // Cờ trạng thái đã đọc/chưa đọc.
    public bool IsRead { get; set; }

    // Thời điểm tạo notification.
    public DateTime CreatedAt { get; set; }
}
