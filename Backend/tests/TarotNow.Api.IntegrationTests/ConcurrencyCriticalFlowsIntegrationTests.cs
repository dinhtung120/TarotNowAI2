using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;
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

        var statusCodes = responses.Select(response => response.StatusCode).ToArray();
        Assert.Contains(HttpStatusCode.OK, statusCodes);
        Assert.All(statusCodes, statusCode => Assert.True(
            statusCode is HttpStatusCode.OK or HttpStatusCode.TooManyRequests));

        var payloads = await Task.WhenAll(responses
            .Where(response => response.StatusCode == HttpStatusCode.OK)
            .Select(response => response.Content.ReadFromJsonAsync<DailyCheckInResult>()));
        Assert.All(payloads, payload => Assert.NotNull(payload));

        var hasFresh = payloads.Any(payload => payload!.IsAlreadyCheckedIn == false);
        Assert.True(hasFresh);

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

    [Fact]
    public async Task RateLimit_AuthSession_ShouldPartitionByAuthenticatedUser()
    {
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();

        var clientA = CreateAuthenticatedClient(_factory, userA);
        var clientB = CreateAuthenticatedClient(_factory, userB);

        var userAStatuses = new List<HttpStatusCode>();
        for (var i = 0; i < 110; i++)
        {
            var response = await clientA.GetAsync("/api/v1/me/runtime-policies");
            userAStatuses.Add(response.StatusCode);
        }

        var userBFirst = await clientB.GetAsync("/api/v1/me/runtime-policies");

        Assert.Contains(HttpStatusCode.TooManyRequests, userAStatuses);
        Assert.Equal(HttpStatusCode.OK, userBFirst.StatusCode);
    }

    [Fact]
    public async Task RateLimit_AuthSession_ShouldFallbackToIp_WhenNameIdentifierMissing()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", " ");

        var first = await client.GetAsync("/api/v1/me/runtime-policies");
        var statuses = new List<HttpStatusCode> { first.StatusCode };
        for (var i = 0; i < 110; i++)
        {
            var response = await client.GetAsync("/api/v1/me/runtime-policies");
            statuses.Add(response.StatusCode);
        }

        Assert.Equal(HttpStatusCode.Unauthorized, first.StatusCode);
        Assert.Contains(HttpStatusCode.TooManyRequests, statuses);
    }

    [Fact]
    public async Task ReadingSessionFollowup_ConcurrentUpdates_ShouldPersistFullDelta()
    {
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid().ToString("N");
        var now = DateTime.UtcNow;

        await CreateReadingSessionAsync(
            ReadingSession.Rehydrate(new ReadingSessionSnapshot
            {
                Id = sessionId,
                UserId = userId.ToString(),
                SpreadType = SpreadType.Spread3Cards,
                Question = "base",
                CardsDrawn = null,
                CurrencyUsed = CurrencyType.Gold,
                AmountCharged = 0,
                IsCompleted = false,
                CreatedAt = now,
                CompletedAt = null,
                AiSummary = null,
                Followups = []
            }));

        var followupA = new ReadingFollowup
        {
            Question = "Q1",
            Answer = "A1",
            AiRequestId = "ai-req-1"
        };
        var followupB = new ReadingFollowup
        {
            Question = "Q2",
            Answer = "A2",
            AiRequestId = "ai-req-2"
        };

        var writer1Session = ReadingSession.Rehydrate(new ReadingSessionSnapshot
        {
            Id = sessionId,
            UserId = userId.ToString(),
            SpreadType = SpreadType.Spread3Cards,
            Question = "base",
            CardsDrawn = null,
            CurrencyUsed = CurrencyType.Gold,
            AmountCharged = 0,
            IsCompleted = false,
            CreatedAt = now,
            CompletedAt = null,
            AiSummary = null,
            Followups = [followupA]
        });
        var writer2Session = ReadingSession.Rehydrate(new ReadingSessionSnapshot
        {
            Id = sessionId,
            UserId = userId.ToString(),
            SpreadType = SpreadType.Spread3Cards,
            Question = "base",
            CardsDrawn = null,
            CurrencyUsed = CurrencyType.Gold,
            AmountCharged = 0,
            IsCompleted = false,
            CreatedAt = now,
            CompletedAt = null,
            AiSummary = null,
            Followups = [followupA, followupB]
        });

        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var writer1 = Task.Run(async () =>
        {
            await gate.Task;
            await UpdateReadingSessionAsync(writer1Session);
        });
        var writer2 = Task.Run(async () =>
        {
            await gate.Task;
            await UpdateReadingSessionAsync(writer2Session);
        });

        gate.SetResult();
        await Task.WhenAll(writer1, writer2);

        using var verifyScope = _factory.Services.CreateScope();
        var repository = verifyScope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        var persisted = await repository.GetByIdAsync(sessionId);
        Assert.NotNull(persisted);
        Assert.Equal(2, persisted!.Followups.Count);

        var followupIds = persisted.Followups
            .Select(x => x.AiRequestId)
            .Where(x => string.IsNullOrWhiteSpace(x) == false)
            .ToHashSet(StringComparer.Ordinal);
        Assert.Contains("ai-req-1", followupIds);
        Assert.Contains("ai-req-2", followupIds);
    }

    [Fact]
    public async Task UserCollection_UpsertWithSameOperationKey_ShouldBeIdempotent()
    {
        var userId = Guid.NewGuid();
        const int cardId = 7;
        const string operationKey = "reading_reveal_collection_test_key";

        using (var scope = _factory.Services.CreateScope())
        {
            var collectionRepository = scope.ServiceProvider.GetRequiredService<IUserCollectionRepository>();
            await collectionRepository.UpsertCardAsync(
                userId,
                cardId,
                expToGain: 25m,
                orientation: CardOrientation.Upright,
                operationKey: operationKey);
            await collectionRepository.UpsertCardAsync(
                userId,
                cardId,
                expToGain: 25m,
                orientation: CardOrientation.Upright,
                operationKey: operationKey);
        }

        using var verifyScope = _factory.Services.CreateScope();
        var verifyRepository = verifyScope.ServiceProvider.GetRequiredService<IUserCollectionRepository>();
        var cards = (await verifyRepository.GetUserCollectionAsync(userId)).ToList();
        var collectionCard = Assert.Single(cards, x => x.CardId == cardId);

        Assert.Equal(1, collectionCard.Copies);
        Assert.Equal(25m, collectionCard.CurrentExp);
    }

    [Fact]
    public async Task MongoIndexBootstrap_DuplicateSystemEventKey_ShouldNormalizeAndRecreateUniqueIndex()
    {
        var conversationId = ObjectId.GenerateNewId().ToString();
        var duplicateSystemEventKey = $"sys_evt_{Guid.NewGuid():N}";

        using (var scope = _factory.Services.CreateScope())
        {
            var mongo = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
            try
            {
                mongo.ChatMessages.Indexes.DropOne("ux_conversationid_systemeventkey");
            }
            catch (MongoCommandException)
            {
                // Index chưa tồn tại ở DB mới thì bỏ qua.
            }

            await mongo.ChatMessages.InsertManyAsync([
                new ChatMessageDocument
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ConversationId = conversationId,
                    SenderId = Guid.NewGuid().ToString(),
                    Type = "system",
                    Content = "sys-msg-a",
                    SystemEventKey = duplicateSystemEventKey,
                    IsRead = false,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow.AddSeconds(-2)
                },
                new ChatMessageDocument
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ConversationId = conversationId,
                    SenderId = Guid.NewGuid().ToString(),
                    Type = "system",
                    Content = "sys-msg-b",
                    SystemEventKey = duplicateSystemEventKey,
                    IsRead = false,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            ]);
        }

        using (var scope = _factory.Services.CreateScope())
        {
            var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MongoDbContext>>();
            _ = new MongoDbContext(mongoDatabase, logger, new FixedHostEnvironment("Production"));
        }

        using (var scope = _factory.Services.CreateScope())
        {
            var mongo = scope.ServiceProvider.GetRequiredService<MongoDbContext>();

            var indexedDocuments = await mongo.ChatMessages.CountDocumentsAsync(
                Builders<ChatMessageDocument>.Filter.And(
                    Builders<ChatMessageDocument>.Filter.Eq(x => x.ConversationId, conversationId),
                    Builders<ChatMessageDocument>.Filter.Eq(x => x.SystemEventKey, duplicateSystemEventKey)));
            Assert.Equal(1, indexedDocuments);

            var indexes = await (await mongo.ChatMessages.Indexes.ListAsync()).ToListAsync();
            Assert.Contains(indexes, x => x["name"].AsString == "ux_conversationid_systemeventkey");
        }
    }

    private sealed class FixedHostEnvironment : IHostEnvironment
    {
        public FixedHostEnvironment(string environmentName)
        {
            EnvironmentName = environmentName;
            ApplicationName = "TarotNow.Api.IntegrationTests";
            ContentRootPath = AppContext.BaseDirectory;
            ContentRootFileProvider = new NullFileProvider();
        }

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
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

    private static HttpClient CreateAuthenticatedClient(WebApplicationFactory<Program> factory, Guid userId)
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
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

    private async Task CreateReadingSessionAsync(ReadingSession session)
    {
        using var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        await repository.CreateAsync(session);
    }

    private async Task UpdateReadingSessionAsync(ReadingSession session)
    {
        using var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        await repository.UpdateAsync(session);
    }
}
