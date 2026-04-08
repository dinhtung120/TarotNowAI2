using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Email sender giả lập cho môi trường dev/test không cần SMTP thật.
public class MockEmailSender : IEmailSender
{
    // Logger dùng để in nội dung email thay cho thao tác gửi thật.
    private readonly ILogger<MockEmailSender> _logger;

    /// <summary>
    /// Khởi tạo mock email sender.
    /// Luồng inject logger để đảm bảo email giả lập vẫn có thể theo dõi trong log.
    /// </summary>
    public MockEmailSender(ILogger<MockEmailSender> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log nội dung email thay vì gửi thực tế.
    /// Luồng này giúp test nhanh nghiệp vụ gửi mail mà không phụ thuộc hạ tầng SMTP.
    /// </summary>
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // Log đầy đủ To/Subject/Body để developer dễ đối chiếu trong local.
        _logger.LogInformation("\n========== MOCK EMAIL SENT ==========\nTo: {To}\nSubject: {Subject}\nBody: {Body}\n======================================\n", to, subject, body);

        return Task.CompletedTask;
    }
}
