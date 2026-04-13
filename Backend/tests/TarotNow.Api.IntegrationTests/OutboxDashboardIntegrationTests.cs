using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TarotNow.Api.Realtime;
using TarotNow.Infrastructure.BackgroundJobs.Outbox;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử endpoint dashboard vận hành outbox phía admin API.
public sealed class OutboxDashboardIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class dashboard outbox.
    /// </summary>
    public OutboxDashboardIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                RemoveHostedService<OutboxProcessorWorker>(services);
                RemoveHostedService<RedisRealtimeSignalRBridgeService>(services);
            });
        });
    }

    /// <summary>
    /// Xác nhận admin nhận đủ counts/ages và áp giới hạn top đúng.
    /// </summary>
    [Fact]
    public async Task Dashboard_ShouldReturnCountsAndRetryAge_WhenCalledByAdmin()
    {
        await ResetOutboxAsync();
        await SeedOutboxDashboardDataAsync();

        var client = CreateAdminClient();
        var response = await client.GetAsync("/api/v1/admin/outbox/dashboard?top=1");
        var responseText = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(responseText);
        var root = document.RootElement;

        Assert.Equal(1, root.GetProperty("pendingCount").GetInt32());
        Assert.Equal(1, root.GetProperty("processingCount").GetInt32());
        Assert.Equal(2, root.GetProperty("failedCount").GetInt32());
        Assert.Equal(1, root.GetProperty("deadLetterCount").GetInt32());
        Assert.Equal(1, root.GetProperty("retryOverdueCount").GetInt32());
        Assert.True(root.GetProperty("oldestPendingAgeSeconds").GetInt64() > 0);
        Assert.True(root.GetProperty("oldestFailedAgeSeconds").GetInt64() > 0);
        Assert.True(root.GetProperty("oldestDeadLetterAgeSeconds").GetInt64() > 0);
        Assert.True(root.GetProperty("maxRetryAgeSeconds").GetInt64() > 0);
        Assert.Equal(1, root.GetProperty("topFailed").GetArrayLength());
        Assert.Equal(1, root.GetProperty("topDeadLetters").GetArrayLength());
    }

    /// <summary>
    /// Xác nhận tham số top được giới hạn tối đa 100 theo contract dashboard.
    /// </summary>
    [Fact]
    public async Task Dashboard_ShouldCapTopToHundred_WhenTopExceedsLimit()
    {
        await ResetOutboxAsync();
        await SeedOutboxBulkDataAsync(failedCount: 120, deadLetterCount: 140);

        var client = CreateAdminClient();
        var response = await client.GetAsync("/api/v1/admin/outbox/dashboard?top=999");
        var responseText = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(responseText);
        var root = document.RootElement;
        Assert.Equal(100, root.GetProperty("topFailed").GetArrayLength());
        Assert.Equal(100, root.GetProperty("topDeadLetters").GetArrayLength());
    }

    /// <summary>
    /// Xác nhận role không phải admin bị chặn truy cập endpoint dashboard.
    /// </summary>
    [Fact]
    public async Task Dashboard_ShouldReturnForbidden_WhenCallerIsNotAdmin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "User");

        var response = await client.GetAsync("/api/v1/admin/outbox/dashboard");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private HttpClient CreateAdminClient()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-Role", "admin");
        return client;
    }

    private async Task ResetOutboxAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.OutboxHandlerStates.RemoveRange(dbContext.OutboxHandlerStates);
        dbContext.OutboxMessages.RemoveRange(dbContext.OutboxMessages);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedOutboxDashboardDataAsync()
    {
        var nowUtc = DateTime.UtcNow;

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.OutboxMessages.AddRange(
            CreateMessage(OutboxMessageStatus.Pending, nowUtc.AddMinutes(-8), nowUtc.AddMinutes(10), 0),
            CreateMessage(OutboxMessageStatus.Processing, nowUtc.AddMinutes(-7), nowUtc.AddMinutes(9), 1),
            CreateMessage(OutboxMessageStatus.Failed, nowUtc.AddMinutes(-6), nowUtc.AddMinutes(-2), 3, "failed-1"),
            CreateMessage(OutboxMessageStatus.Failed, nowUtc.AddMinutes(-5), nowUtc.AddMinutes(3), 2, "failed-2"),
            CreateMessage(OutboxMessageStatus.DeadLetter, nowUtc.AddMinutes(-30), nowUtc.AddMinutes(-15), 12, "dead-letter"));

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedOutboxBulkDataAsync(int failedCount, int deadLetterCount)
    {
        var nowUtc = DateTime.UtcNow;
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        for (var index = 0; index < failedCount; index++)
        {
            dbContext.OutboxMessages.Add(CreateMessage(
                OutboxMessageStatus.Failed,
                nowUtc.AddMinutes(-(index + 1)),
                nowUtc.AddMinutes(-(index + 1)),
                index + 1,
                $"failed-{index}"));
        }

        for (var index = 0; index < deadLetterCount; index++)
        {
            dbContext.OutboxMessages.Add(CreateMessage(
                OutboxMessageStatus.DeadLetter,
                nowUtc.AddMinutes(-(index + 1)),
                nowUtc.AddMinutes(-(index + 1)),
                index + 1,
                $"dead-{index}"));
        }

        await dbContext.SaveChangesAsync();
    }

    private static OutboxMessage CreateMessage(
        string status,
        DateTime createdAtUtc,
        DateTime nextAttemptAtUtc,
        int attemptCount,
        string? lastError = null)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = "Test.Event",
            PayloadJson = "{}",
            OccurredAtUtc = createdAtUtc,
            Status = status,
            AttemptCount = attemptCount,
            NextAttemptAtUtc = nextAttemptAtUtc,
            CreatedAtUtc = createdAtUtc,
            LastError = lastError
        };
    }

    private static void RemoveHostedService<THostedService>(IServiceCollection services)
        where THostedService : class, IHostedService
    {
        var descriptors = services
            .Where(descriptor =>
                descriptor.ServiceType == typeof(IHostedService)
                && descriptor.ImplementationType == typeof(THostedService))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}
