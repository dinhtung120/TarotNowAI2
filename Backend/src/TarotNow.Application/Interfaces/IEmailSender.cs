namespace TarotNow.Application.Interfaces;

/// <summary>
/// Dịch vụ gửi Email (sẽ được Mock trong quá trình phát triển Phase 1.x)
/// Dùng để gửi thư kích hoạt tài khoản, đặt lại mật khẩu, v.v.
/// </summary>
public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
