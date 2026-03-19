/*
 * ===================================================================
 * FILE: UserConsent.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lưu Trữ Chữ Kí Bù Nhìn (Consent) Chấp Nhận Hợp Đồng TOS/Privacy Lúc Khách Bấm Đăng Kí Ban Đầu.
 *   Xoáy Đền Tòa Lúc Bị Kiện Vì Hợp Đồng Điều Khoản Ứng Dụng Đã Submit.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity lưu trữ lịch sử đồng ý các điều khoản pháp lý của User (Bảng SQL user_consents).
/// Lôi Ra Cãi Trước Tòa Nếu Bị Xộ Khám Ai Rắc Rối.
/// </summary>
public class UserConsent
{
    // Cột Điểm Đếm Khung SQL Tự Bóp.
    public Guid Id { get; private set; }
    // Khách Nào Đã Ký Đơn Nhượng Bộ TOS?
    public Guid UserId { get; private set; }
    
    /// <summary>
    /// Loại tài liệu: TOS (Điều Khoản App), PrivacyPolicy (Chính Sách Chối Data), AiDisclaimer (Miễn Trách Nhiệm Phù Bói).
    /// </summary>
    public string DocumentType { get; private set; } = string.Empty;
    
    /// <summary>
    /// Phiên bản của tài liệu khi User đồng ý (ví dụ Đổi Luật Đòi Thuê Thêm Tiền Nâng Lên Version: "1.0", "1.1").
    /// </summary>
    public string Version { get; private set; } = string.Empty;
    
    /// <summary>
    /// Bút Tích Xuống Tay Nhấn Nút Ký Lúc Mấy Giờ Sáng.
    /// </summary>
    public DateTime ConsentedAt { get; private set; }
    
    // Ghi Nhận Địa Chỉ Máy Tính IP Kí Của Khách Tránh Đổ Lỗi Bị Trả Lời (Tao Không Ký IP Khác Hack Tao).
    public string IpAddress { get; private set; } = string.Empty;
    // Kiểu Trình Duyệt Ngửi Bọn Chơi Trình Đạo Cụ Chống Gì Không Trình Duyệt Bàn Cào (UserAgent).
    public string UserAgent { get; private set; } = string.Empty;

    // Đường Lương Duyệt Bản Sắc Entity Bảng Ánh Xạ Rút Báo Nối Từ Con User (Phần 1->N của EF Core).
    public User User { get; private set; } = null!;

    protected UserConsent() { } // Dành cho Trình Cảo EF Core Tự Chui Data Hất Lên Lén

    /// <summary>Mút Nháy Đồng Ý Bịch.</summary>
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
