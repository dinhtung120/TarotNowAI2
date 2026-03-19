/*
 * FILE: MockEmailSender.cs
 * MỤC ĐÍCH: Email sender GIẢ LẬP dùng cho local development và testing.
 *   Thay vì gửi email thật (tốn tiền, cần SMTP/SendGrid) → chỉ log ra console.
 *
 *   KHI NÀO DÙNG?
 *   → Local dev: không cần cài SMTP server
 *   → Testing: kiểm tra logic gửi email mà không gửi thật
 *   → CI/CD: chạy test tự động không cần email provider
 *
 *   PHASE 2: sẽ thay bằng SendGrid/SMTP thực tế.
 *   → Chỉ cần tạo class SendGridEmailSender : IEmailSender + đổi DI registration.
 *   → Logic nghiệp vụ KHÔNG cần thay đổi (Interface Segregation Principle).
 */

using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implement IEmailSender — chỉ log ra console (không gửi email thật).
/// </summary>
public class MockEmailSender : IEmailSender
{
    private readonly ILogger<MockEmailSender> _logger;

    public MockEmailSender(ILogger<MockEmailSender> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// "Gửi" email bằng cách log thông tin ra console.
    /// Format rõ ràng để dev dễ đọc khi debug luồng OTP, thông báo, v.v.
    /// </summary>
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("\n========== MOCK EMAIL SENT ==========\nTo: {To}\nSubject: {Subject}\nBody: {Body}\n======================================\n", to, subject, body);
        
        return Task.CompletedTask; // Không làm gì thêm — mock
    }
}
