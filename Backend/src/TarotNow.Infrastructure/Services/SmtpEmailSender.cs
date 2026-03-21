/*
 * FILE: SmtpEmailSender.cs
 * MỤC ĐÍCH: Triển khai IEmailSender gửi thư thông qua chuẩn thư tín SMTP (SMTP Client).
 * 
 * LƯU Ý KHI DÙNG GMAIL:
 * - Để dùng Gmail, bạn KHÔNG ĐƯỢC phép dùng mật khẩu đăng nhập tài khoản.
 * - Bạn phải bật [Bảo mật 2 Lớp (2FA)] trên tải khoản Google. 
 * - Sau đó vào [App passwords / Mật khẩu ứng dụng] sinh ra dãy 16 kí tự. Dùng nó chép vào "appsettings.json".
 */

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Trình điều khiển gửi Email tiêu chuẩn, ưu tiên sử dụng cấu hình Gmail của Google cho server.
/// Đặc tả qua thư viện khuyết MailKit thay cho SmtpClient gốc của Microsoft do tính ổn định và không bị block thread.
/// </summary>
public class SmtpEmailSender : IEmailSender, IDisposable
{
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _senderEmail;
    private readonly string _senderName;
    private readonly SmtpClient _client; // Tái sử dụng TCP connection tiết kiệm chi phí Memory

    public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger)
    {
        _logger = logger;
        
        // Cú đẩy cài đặt từ file cấu hình, fallback thành Gmail SMTP.
        _host = configuration["Smtp:Host"] ?? "smtp.gmail.com";
        _port = int.TryParse(configuration["Smtp:Port"], out var p) ? p : 587;
        _username = configuration["Smtp:Username"] ?? "";
        _password = configuration["Smtp:Password"] ?? "";
        _senderEmail = configuration["Smtp:SenderEmail"] ?? "no-reply@tarotnow.vn";
        _senderName = configuration["Smtp:SenderName"] ?? "TarotNow Security";

        if (string.IsNullOrEmpty(_password) || _password.Contains("REPLACE_WITH"))
        {
            _logger.LogWarning("Smtp:Password chưa được thiết lập thực tế. Tính năng cấp OTP sẽ văng lỗi Authenticated failed!");
        }

        _client = new SmtpClient();
    }

    /// <summary>
    /// Hàm chính: Gói ghém, phân tích Header / Payload thành cục Tin nhắn MIME. Khởi trệ tiến trình bắn Server.
    /// </summary>
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            // Bọc thành gói tin chuẩn Mime
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_senderName, _senderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            
            // Format Plaintext, vì OTP không đòi hỏi HTML Render (Có thể đổi sang TextPart("html") dưới này nếu nâng cấp Template).
            message.Body = new TextPart("plain") 
            { 
                Text = body 
            };

            // Nếu cửa Socket SMTP đang đóng băng, ta dùng Secure Socket Opt cho STARTTLS (Bảo mật luồng vận chuyển Email)
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_host, _port, SecureSocketOptions.StartTls, cancellationToken);
            }

            // Chứng thực bản thân bằng tài khoản mật khẩu Mail (Vd: pass 16 số Gmail)
            if (!_client.IsAuthenticated)
            {
                await _client.AuthenticateAsync(_username, _password, cancellationToken);
            }

            // Giao kết tiến trình phát đi
            await _client.SendAsync(message, cancellationToken);
            
            _logger.LogInformation("SMTP MimeKit Info: Đã gửi email thành công tới hòm thư đích {ToEmail}", to);
            
            // Note: Để giữ tối ưu Socket Pooling, không _client.Disconnect() ngay lập tức,
            // để Disconnect ở tầng Dispose() vòng đời Scoped.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tiến trình gửi SMTP hỏng xé, nguyên nhân dứt gãy tại MailKit: {Message}", ex.Message);
            throw; // Propagate lỗi thẳng đứng lên Handle để Application xử rớt.
        }
    }

    /// <summary>
    /// Khi chu kỳ vòng đời HttpRequest tàn tro, framework tự động gọi chày Dispose() giúp xả Socket, đóng đường TCP nhẹ nhõm RAM server.
    /// </summary>
    public void Dispose()
    {
        if (_client.IsConnected)
        {
            _client.Disconnect(true);
        }
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}
