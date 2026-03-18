using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity lưu trữ lịch sử đồng ý các điều khoản pháp lý của User.
/// </summary>
public class UserConsent
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    
    /// <summary>
    /// Loại tài liệu: TOS, PrivacyPolicy, AiDisclaimer
    /// </summary>
    public string DocumentType { get; private set; } = string.Empty;
    
    /// <summary>
    /// Phiên bản của tài liệu khi User đồng ý (ví dụ: "1.0", "1.1")
    /// </summary>
    public string Version { get; private set; } = string.Empty;
    
    /// <summary>
    /// Thời điểm đồng ý
    /// </summary>
    public DateTime ConsentedAt { get; private set; }
    
    public string IpAddress { get; private set; } = string.Empty;
    public string UserAgent { get; private set; } = string.Empty;

    // Navigation Property
    public User User { get; private set; } = null!;

    protected UserConsent() { } // Dành cho EF Core

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
