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
public class FollowupCapIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public FollowupCapIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task StreamEndpoint_ShouldReject_WhenFollowUpCapIsReached()
    {
        // 1. Arrange: Setup DB with 5 completed follow-ups
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();

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
            
            // Set Id via reflection since it's private set and we need a consistent Id for the test
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            
            user.Activate(); // Set Status = Active
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup); // Set balance
            
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card.ToString());
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        // Seed Initial Reading + 5 Follow-ups (total 6 requests logic)
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

        // 2. Act: Request the 6th Follow-up natively
        var requestUrl = $"/api/v1/sessions/{sessionId}/stream?followupQuestion=Should+Fail";
        var requestMsg = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        // Accept header to trigger SSE
        requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        var response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead);

        // 3. Assert
        // The API should NOT allow this to stream -> Typically 400 Bad Request or 500
        // We ensure it is not a success status code (200 OK with stream).
        Assert.False(response.IsSuccessStatusCode, "Because the hard cap of 5 follow ups has been reached");
    }
}
