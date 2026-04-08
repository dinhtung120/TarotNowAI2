
using System;

namespace TarotNow.Domain.Entities;

// Entity consent pháp lý của người dùng để truy vết phiên bản tài liệu đã chấp thuận.
public class UserConsent
{
    // Định danh consent.
    public Guid Id { get; private set; }

    // Người dùng thực hiện chấp thuận.
    public Guid UserId { get; private set; }

    // Loại tài liệu pháp lý.
    public string DocumentType { get; private set; } = string.Empty;

    // Phiên bản tài liệu đã chấp thuận.
    public string Version { get; private set; } = string.Empty;

    // Thời điểm chấp thuận.
    public DateTime ConsentedAt { get; private set; }

    // Địa chỉ IP tại thời điểm consent.
    public string IpAddress { get; private set; } = string.Empty;

    // User agent tại thời điểm consent.
    public string UserAgent { get; private set; } = string.Empty;

    // Navigation tới user.
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu đã lưu.
    /// </summary>
    protected UserConsent() { }

    /// <summary>
    /// Khởi tạo bản ghi consent mới cho một tài liệu pháp lý cụ thể.
    /// Luồng xử lý: sinh id, gán thông tin tài liệu/ngữ cảnh và chốt thời điểm ConsentedAt.
    /// </summary>
    public UserConsent(Guid userId, string documentType, string version, string ipAddress, string userAgent)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        DocumentType = documentType;
        Version = version;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        ConsentedAt = DateTime.UtcNow;
    }
}
