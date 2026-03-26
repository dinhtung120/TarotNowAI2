using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly ISmtpClient _smtpClient;
    private readonly SmtpOptions _options;

    public SmtpEmailSender(
        IOptions<SmtpOptions> options,
        ILogger<SmtpEmailSender> logger,
        ISmtpClient smtpClient)
    {
        _options = options.Value;
        _logger = logger;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var smtpConfig = ValidateConfiguration();

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpConfig.SenderName, smtpConfig.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(smtpConfig.Host, smtpConfig.Port, SecureSocketOptions.StartTls, cancellationToken);
            }

            if (!_smtpClient.IsAuthenticated)
            {
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

    private SmtpConfig ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_options.Password) ||
            _options.Password.Contains("REPLACE_WITH", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Smtp:Password is missing or still a placeholder.");
        }

        var host = string.IsNullOrWhiteSpace(_options.Host) ? "smtp.gmail.com" : _options.Host.Trim();
        var port = _options.Port > 0 ? _options.Port : 587;
        var senderEmail = string.IsNullOrWhiteSpace(_options.SenderEmail) ? "no-reply@tarotnow.vn" : _options.SenderEmail.Trim();
        var senderName = string.IsNullOrWhiteSpace(_options.SenderName) ? "TarotNow Security" : _options.SenderName.Trim();
        var username = _options.Username?.Trim() ?? string.Empty;
        var password = _options.Password;
        return new SmtpConfig(host, port, username, password, senderEmail, senderName);
    }

    private sealed record SmtpConfig(
        string Host,
        int Port,
        string Username,
        string Password,
        string SenderEmail,
        string SenderName);
}
