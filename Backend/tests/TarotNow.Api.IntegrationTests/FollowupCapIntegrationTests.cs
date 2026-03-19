/*
 * FILE: FollowupCapIntegrationTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra HARD CAP follow-up questions (giới hạn 5 câu/session).
 *
 *   QUY TẮC KINH DOANH:
 *   → Mỗi ReadingSession cho phép tối đa 5 follow-up questions (sau câu hỏi đầu).
 *   → Tổng: 1 câu gốc + 5 follow-up = 6 AiRequests / session.
 *   → Nếu vượt cap → API trả lỗi (400 hoặc tương tự) → KHÔNG cho stream.
 *
 *   TEST CASE:
 *   StreamEndpoint_ShouldReject_WhenFollowUpCapIsReached:
 *   → Seed: User + Session + 6 AiRequests (1 gốc + 5 follow-up, tất cả Completed)
 *   → Act: gọi follow-up thứ 6 (câu thứ 7 tổng cộng)
 *   → Assert: response KHÔNG phải success (400/500)
 *
 *   PATTERN: Seed data → request → assert non-success.
 */

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

/// <summary>
/// Test hard cap follow-up: vượt 5 follow-up/session → bị reject.
/// </summary>
[Collection("Testcontainers")]
public class FollowupCapIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public FollowupCapIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Vượt hard cap 5 follow-up → stream bị reject.
    /// Seed 6 AiRequests (1 gốc + 5 follow-up) rồi gửi thêm 1 → phải fail.
    /// </summary>
    [Fact]
    public async Task StreamEndpoint_ShouldReject_WhenFollowUpCapIsReached()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();

        // Seed User
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

        // Tạo ReadingSession đã reveal
        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card.ToString());
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        // Seed 6 AiRequests Completed (1 gốc + 5 follow-up) → đã đạt hard cap
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

        // ACT: gửi follow-up thứ 6 (câu thứ 7 tổng cộng) → phải bị reject
        var requestUrl = $"/api/v1/sessions/{sessionId}/stream?followupQuestion=Should+Fail";
        var requestMsg = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        var response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead);

        // ASSERT: phải KHÔNG thành công (hard cap đã đạt)
        Assert.False(response.IsSuccessStatusCode, "Because the hard cap of 5 follow ups has been reached");
    }
}
