
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract gửi email để chuẩn hóa kênh thông báo độc lập với nhà cung cấp SMTP.
public interface IEmailSender
{
    /// <summary>
    /// Gửi email theo nội dung đã dựng để phục vụ OTP, thông báo hoặc nghiệp vụ hệ thống.
    /// Luồng xử lý: nhận địa chỉ đích và nội dung, sau đó chuyển cho provider gửi email tương ứng.
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
