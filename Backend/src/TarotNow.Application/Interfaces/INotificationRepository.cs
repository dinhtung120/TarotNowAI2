namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository interface cho notifications — thông báo in-app.
///
/// Notifications tự hết hạn sau 30 ngày (TTL index MongoDB).
/// Hỗ trợ đa ngôn ngữ (vi/en/zh) cho title và body.
/// </summary>
public interface INotificationRepository
{
    /// <summary>Tạo 1 thông báo mới cho user.</summary>
    Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông báo theo user — hỗ trợ filter isRead + phân trang.
    /// Nếu isRead = null → lấy tất cả. true → chỉ đã đọc. false → chỉ chưa đọc.
    /// </summary>
    Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>Đánh dấu 1 thông báo đã đọc — kiểm tra ownership (userId).</summary>
    Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Đếm số thông báo chưa đọc — dùng cho badge count trên UI.</summary>
    Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>DTO tạo notification mới.</summary>
public class NotificationCreateDto
{
    public Guid UserId { get; set; }
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string BodyVi { get; set; } = string.Empty;
    public string BodyEn { get; set; } = string.Empty;
    public string BodyZh { get; set; } = string.Empty;
    public string Type { get; set; } = "system";
    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>DTO đọc notification.</summary>
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
