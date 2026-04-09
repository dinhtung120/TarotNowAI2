using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

// Email sender gửi mail thật qua SMTP.
public sealed class SmtpEmailSender : IEmailSender
{
    // Logger theo dõi trạng thái gửi mail.
    private readonly ILogger<SmtpEmailSender> _logger;
    // SMTP client abstraction để dễ mock trong test.
    private readonly ISmtpClient _smtpClient;
    // Cấu hình SMTP nạp từ appsettings.
    private readonly SmtpOptions _options;

    /// <summary>
    /// Khởi tạo SMTP email sender.
    /// Luồng inject client và options giúp gửi mail linh hoạt theo từng môi trường.
    /// </summary>
    public SmtpEmailSender(
        IOptions<SmtpOptions> options,
        ILogger<SmtpEmailSender> logger,
        ISmtpClient smtpClient)
    {
        _options = options.Value;
        _logger = logger;
        _smtpClient = smtpClient;
    }

    /// <summary>
    /// Gửi email plaintext tới người nhận chỉ định.
    /// Luồng validate cấu hình, kết nối SMTP, xác thực rồi gửi và ngắt kết nối an toàn.
    /// </summary>
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var smtpConfig = ValidateConfiguration();

        try
        {
            // Dựng message chuẩn MIME để đảm bảo tương thích máy chủ mail phổ biến.
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpConfig.SenderName, smtpConfig.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            if (!_smtpClient.IsConnected)
            {
                // Chỉ kết nối khi cần để tránh mở kết nối dư thừa.
                await _smtpClient.ConnectAsync(smtpConfig.Host, smtpConfig.Port, SecureSocketOptions.StartTls, cancellationToken);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                // Xác thực SMTP trước khi gửi để tránh bị relay deny.
                await _smtpClient.AuthenticateAsync(smtpConfig.Username, smtpConfig.Password, cancellationToken);
            }

            await _smtpClient.SendAsync(message, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _logger.LogInformation("Sent SMTP email to {ToEmail}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMTP email to {ToEmail}", to);
            throw;
        }
    }

    /// <summary>
    /// Kiểm tra và chuẩn hóa cấu hình SMTP từ options.
    /// Luồng fail-fast khi mật khẩu placeholder để tránh gửi lỗi âm thầm ở production.
    /// </summary>
    private SmtpConfig ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_options.Password) ||
            _options.Password.Contains("REPLACE_WITH", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Smtp:Password is missing or still a placeholder.");
        }

        var host = _options.Host?.Trim();
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException("Smtp:Host is missing.");
        }

        if (_options.Port <= 0)
        {
            throw new InvalidOperationException("Smtp:Port is missing or invalid.");
        }

        var senderEmail = _options.SenderEmail?.Trim();
        if (string.IsNullOrWhiteSpace(senderEmail))
        {
            throw new InvalidOperationException("Smtp:SenderEmail is missing.");
        }

        var senderName = _options.SenderName?.Trim();
        if (string.IsNullOrWhiteSpace(senderName))
        {
            throw new InvalidOperationException("Smtp:SenderName is missing.");
        }

        var username = _options.Username?.Trim();
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Smtp:Username is missing.");
        }

        var port = _options.Port;
        var password = _options.Password;
        // Trả config đã chuẩn hóa để luồng gửi mail dùng thống nhất.
        return new SmtpConfig(host, port, username, password, senderEmail, senderName);
    }

    // Bản ghi cấu hình SMTP đã chuẩn hóa cho một lần gửi email.
    private sealed record SmtpConfig(
        string Host,
        int Port,
        string Username,
        string Password,
        string SenderEmail,
        string SenderName);
}
