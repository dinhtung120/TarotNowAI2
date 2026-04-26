using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Api.Controllers;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public sealed class ConcurrencyCriticalFlowsIntegrationTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ConcurrencyCriticalFlowsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CheckIn_DoubleSubmit_ShouldPersistSingleBusinessDateRecord()
    {
        var userId = Guid.NewGuid();
        var client = CreateAuthenticatedClient(userId);
        await EnsureTestUserAsync(userId);

        var postTasks = new[]
        {
            client.PostAsync("/api/v1/checkin", content: null),
            client.PostAsync("/api/v1/checkin", content: null)
        };
        var responses = await Task.WhenAll(postTasks);

        Assert.All(responses, response => Assert.True(response.IsSuccessStatusCode));

        var payload1 = await responses[0].Content.ReadAsStringAsync();
        var payload2 = await responses[1].Content.ReadAsStringAsync();
        var hasFresh = payload1.Contains("\"isAlreadyCheckedIn\":false", StringComparison.OrdinalIgnoreCase)
                       || payload2.Contains("\"isAlreadyCheckedIn\":false", StringComparison.OrdinalIgnoreCase);
        var hasReplay = payload1.Contains("\"isAlreadyCheckedIn\":true", StringComparison.OrdinalIgnoreCase)
                        || payload2.Contains("\"isAlreadyCheckedIn\":true", StringComparison.OrdinalIgnoreCase);
        Assert.True(hasFresh);
        Assert.True(hasReplay);

        var businessDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
        using var scope = _factory.Services.CreateScope();
        var mongo = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
        var filter = Builders<DailyCheckinDocument>.Filter.Eq(x => x.UserId, userId.ToString())
                     & Builders<DailyCheckinDocument>.Filter.Eq(x => x.BusinessDate, businessDate);
        var count = await mongo.DailyCheckins.CountDocumentsAsync(filter);
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task LegalConsent_DoubleSubmit_ShouldPersistSingleConsentRow()
    {
        var userId = Guid.NewGuid();
        var client = CreateAuthenticatedClient(userId);
        await EnsureTestUserAsync(userId);

        var payload = new
        {
            DocumentType = "TOS",
            Version = "1.0"
        };

        var postTasks = new[]
        {
            client.PostAsJsonAsync("/api/v1/legal/consent", payload),
            client.PostAsJsonAsync("/api/v1/legal/consent", payload)
        };
        var responses = await Task.WhenAll(postTasks);

        Assert.All(responses, response => Assert.True(response.IsSuccessStatusCode));

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var count = await db.UserConsents.CountAsync(x =>
            x.UserId == userId
            && x.DocumentType == "TOS"
            && x.Version == "1.0");
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task ReaderApply_DoubleSubmit_ShouldKeepSinglePendingRequest()
    {
        var userId = Guid.NewGuid();
        var client = CreateAuthenticatedClient(userId);
        await EnsureTestUserAsync(userId);

        var body = new
        {
            bio = "I am a serious reader with stable profile.",
            specialties = new[] { "love", "career" },
            yearsOfExperience = 5,
            facebookUrl = "https://facebook.com/reader.demo",
            instagramUrl = "https://instagram.com/reader.demo",
            tikTokUrl = (string?)null,
            diamondPerQuestion = 200L,
            proofDocuments = new[] { "https://example.com/proof-1.png" }
        };

        var postTasks = new[]
        {
            client.PostAsJsonAsync("/api/v1/reader/apply", body),
            client.PostAsJsonAsync("/api/v1/reader/apply", body)
        };
        var responses = await Task.WhenAll(postTasks);

        var statusCodes = responses.Select(response => response.StatusCode).ToArray();
        Assert.Contains(HttpStatusCode.OK, statusCodes);
        Assert.All(statusCodes, code => Assert.True(code is HttpStatusCode.OK or HttpStatusCode.BadRequest));

        using var scope = _factory.Services.CreateScope();
        var mongo = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
        var filter = Builders<ReaderRequestDocument>.Filter.Eq(x => x.UserId, userId.ToString())
                     & Builders<ReaderRequestDocument>.Filter.Eq(x => x.Status, ReaderApprovalStatus.Pending)
                     & Builders<ReaderRequestDocument>.Filter.Eq(x => x.IsDeleted, false);
        var count = await mongo.ReaderRequests.CountDocumentsAsync(filter);
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task WalletRelease_OppositeDirectionConcurrent_ShouldCompleteWithoutDeadlock()
    {
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        await EnsureTestUserAsync(userA);
        await EnsureTestUserAsync(userB);

        await ExecuteWalletOperationAsync(repository => repository.CreditAsync(
            userA,
            CurrencyType.Diamond,
            TransactionType.Deposit,
            amount: 500,
            referenceSource: "concurrency_test",
            referenceId: "credit_a",
            idempotencyKey: $"wallet_credit_a_{Guid.NewGuid():N}"));
        await ExecuteWalletOperationAsync(repository => repository.CreditAsync(
            userB,
            CurrencyType.Diamond,
            TransactionType.Deposit,
            amount: 500,
            referenceSource: "concurrency_test",
            referenceId: "credit_b",
            idempotencyKey: $"wallet_credit_b_{Guid.NewGuid():N}"));
        await ExecuteWalletOperationAsync(repository => repository.FreezeAsync(
            userA,
            amount: 150,
            referenceSource: "concurrency_test",
            referenceId: "freeze_a",
            idempotencyKey: $"wallet_freeze_a_{Guid.NewGuid():N}"));
        await ExecuteWalletOperationAsync(repository => repository.FreezeAsync(
            userB,
            amount: 120,
            referenceSource: "concurrency_test",
            referenceId: "freeze_b",
            idempotencyKey: $"wallet_freeze_b_{Guid.NewGuid():N}"));

        var releaseAToB = ExecuteWalletOperationAsync(repository => repository.ReleaseAsync(
            userA,
            userB,
            amount: 150,
            referenceSource: "concurrency_test",
            referenceId: "release_ab",
            idempotencyKey: $"wallet_release_ab_{Guid.NewGuid():N}"));
        var releaseBToA = ExecuteWalletOperationAsync(repository => repository.ReleaseAsync(
            userB,
            userA,
            amount: 120,
            referenceSource: "concurrency_test",
            referenceId: "release_ba",
            idempotencyKey: $"wallet_release_ba_{Guid.NewGuid():N}"));

        var allReleases = Task.WhenAll(releaseAToB, releaseBToA);
        var completed = await Task.WhenAny(allReleases, Task.Delay(TimeSpan.FromSeconds(30)));
        Assert.Same(allReleases, completed);
        await allReleases;

        using var verifyScope = _factory.Services.CreateScope();
        var db = verifyScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var payerA = await db.Users.SingleAsync(x => x.Id == userA);
        var payerB = await db.Users.SingleAsync(x => x.Id == userB);

        Assert.Equal(470, payerA.DiamondBalance);
        Assert.Equal(530, payerB.DiamondBalance);
        Assert.Equal(0, payerA.FrozenDiamondBalance);
        Assert.Equal(0, payerB.FrozenDiamondBalance);
    }

    private HttpClient CreateAuthenticatedClient(Guid userId)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());
        return client;
    }

    private async Task EnsureTestUserAsync(Guid userId)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await db.Users.AnyAsync(u => u.Id == userId))
        {
            return;
        }

        var user = new User(
            email: $"concurrency-{userId:N}@test.local",
            username: $"user-{userId:N}".Substring(0, 20),
            passwordHash: "hash",
            displayName: "Concurrency Test User",
            dateOfBirth: new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        user.Activate();
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    private async Task ExecuteWalletOperationAsync(Func<IWalletRepository, Task> operation)
    {
        using var scope = _factory.Services.CreateScope();
        var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
        await operation(walletRepository);
    }
}
