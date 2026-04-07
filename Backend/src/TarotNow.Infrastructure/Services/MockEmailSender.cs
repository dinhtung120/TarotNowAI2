

using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class MockEmailSender : IEmailSender
{
    private readonly ILogger<MockEmailSender> _logger;

    public MockEmailSender(ILogger<MockEmailSender> logger)
    {
        _logger = logger;
    }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("\n========== MOCK EMAIL SENT ==========\nTo: {To}\nSubject: {Subject}\nBody: {Body}\n======================================\n", to, subject, body);
        
        return Task.CompletedTask; 
    }
}
