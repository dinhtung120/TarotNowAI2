

using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authentication;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử giới hạn số câu follow-up của luồng streaming AI.
public class FollowupCapIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Factory tích hợp dùng để tạo client và service scope.
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class follow-up cap.
    /// Luồng dùng chung factory để đồng nhất dữ liệu và cấu hình test.
    /// </summary>
    public FollowupCapIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Xác nhận endpoint stream từ chối khi đã chạm trần follow-up.
    /// Luồng seed sẵn 6 request completed rồi gọi follow-up mới và kỳ vọng thất bại.
    /// </summary>
    [Fact]
    public async Task StreamEndpoint_ShouldReject_WhenFollowUpCapIsReached()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();

        // Seed user nếu chưa có để test luôn có dữ liệu hợp lệ.
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "limit_test@tarotnow.com",
                username: "LimitUser",
                passwordHash: "hash",
                displayName: "Test",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Tạo phiên reading nền để gắn follow-up requests.
        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card.ToString());
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        // Seed vượt ngưỡng cap (6 bản ghi) để ép endpoint vào nhánh từ chối.
        for (int i = 0; i <= 5; i++)
        {
            var request = new AiRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ReadingSessionRef = sessionId.ToString(),
                Status = AiRequestStatus.Completed,
                ChargeDiamond = 0,
                CompletionMarkerAt = DateTimeOffset.UtcNow,
                IdempotencyKey = $"seed_{sessionId}_{i}"
            };
            db.AiRequests.Add(request);
        }
        await db.SaveChangesAsync();

        // Gọi follow-up mới với query param để kiểm tra enforcement của cap.
        var requestUrl = $"/api/v1/sessions/{sessionId}/stream?followupQuestion=Should+Fail";
        var requestMsg = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        var response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead);

        // Kỳ vọng request bị từ chối do đã đạt hard cap.
        Assert.False(response.IsSuccessStatusCode, "Because the hard cap of 5 follow ups has been reached");
    }
}
