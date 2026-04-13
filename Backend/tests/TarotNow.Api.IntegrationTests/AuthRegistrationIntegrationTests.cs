using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarotNow.Api.Controllers;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
// Kiểm thử tích hợp luồng register và side-effect gửi OTP xác minh email.
public sealed class AuthRegistrationIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthRegistrationIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_ShouldCreatePendingUserAndPersistVerifyOtp_WhenEmailSendSucceeds()
    {
        var emailSender = new TrackingEmailSender();
        using var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IEmailSender>();
                services.AddSingleton<IEmailSender>(emailSender);
            });
        });

        var client = factory.CreateClient();
        var unique = Guid.NewGuid().ToString("N")[..8];
        var email = $"register-ok-{unique}@example.com";
        var username = $"register_ok_{unique}";

        var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email,
            username,
            password = "StrongPass1",
            displayName = "Register OK",
            dateOfBirth = "1990-01-01T00:00:00Z",
            hasConsented = true
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createdUser = await db.Users.SingleOrDefaultAsync(x => x.Email == email);
        Assert.NotNull(createdUser);
        Assert.Equal(UserStatus.Pending, createdUser.Status);

        var hasVerificationOtp = await db.EmailOtps.AnyAsync(x =>
            x.UserId == createdUser.Id &&
            x.Type == OtpType.VerifyEmail);
        Assert.True(hasVerificationOtp);
        Assert.Equal(1, emailSender.SendCount);
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenEmailSenderFails()
    {
        using var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IEmailSender>();
                services.AddSingleton<IEmailSender>(new FailingEmailSender());
            });
        });

        var client = factory.CreateClient();
        var unique = Guid.NewGuid().ToString("N")[..8];
        var email = $"register-fail-{unique}@example.com";
        var username = $"register_fail_{unique}";

        var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email,
            username,
            password = "StrongPass1",
            displayName = "Register Fail",
            dateOfBirth = "1990-01-01T00:00:00Z",
            hasConsented = true
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createdUser = await db.Users.SingleOrDefaultAsync(x => x.Email == email);
        Assert.NotNull(createdUser);
        Assert.Equal(UserStatus.Pending, createdUser.Status);

        var hasVerificationOtp = await db.EmailOtps.AnyAsync(x =>
            x.UserId == createdUser.Id &&
            x.Type == OtpType.VerifyEmail);
        Assert.True(hasVerificationOtp);
    }

    private sealed class TrackingEmailSender : IEmailSender
    {
        public int SendCount { get; private set; }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            SendCount++;
            return Task.CompletedTask;
        }
    }

    private sealed class FailingEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("Simulated SMTP failure.");
    }
}
