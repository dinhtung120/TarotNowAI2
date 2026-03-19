/*
 * ===================================================================
 * FILE: IEmailSender.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Truyền Thư Lệnh Gọi Hệ Thống Hộp Gửi Thư Mực Đỏ Ra Bên Ngoài Thế Giới (SMTP, SendGrid, Amazon SES,...).
 * ===================================================================
 */

using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Dịch vụ Bồ Câu Đưa Thư (sẽ được Mock Thành Log Chạy Cửa Sổ Console Trong Quá Trình Phát Triển Phase 1.x Để Không Tốn Phí Đăng Ký API).
/// Dùng để gửi thư kích hoạt tài khoản, đặt lại mật khẩu, thư cảnh cáo khóa Tài Khoản,...
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Bay Đi Chim Ơi: Hãy Cầm Giấy Này Và Truyền Đạt Đúng Chữ Đã Viết Gửi Tới Trái Tim Đích.
    /// </summary>
    /// <param name="to">Email Khách. Gửi vào mục Spam Hay Inbox Chờ Số Phận.</param>
    /// <param name="subject">Tiêu Đoạn Bức Huyết Thư Này Nói Chuyện Gì?</param>
    /// <param name="body">Lõi Văn Cố Tích Lạc Lõng Bên Trong Có Html Hay Sao...</param>
    /// <param name="cancellationToken">Lệnh Tắt Nguồn Câm Chuyện Gửi Email Khi Ngắn Lộ Trình Bất Chợt.</param>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
