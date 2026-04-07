

using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Authentication;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TarotNow.Api.IntegrationTests;

public class MockAiProvider : IAiProvider
{
    public string ProviderName => "Mock-OpenAI";
    public string ModelName => "gpt-mock-mini";

    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

        public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string[] chunks = { "Đây ", "là ", "kết ", "quả ", "giải ", "bài ", "mẫu " };
        
        foreach (var chunk in chunks)
        {
            if (cancellationToken.IsCancellationRequested)
                yield break;
                
            await Task.Delay(50, cancellationToken); 
            yield return chunk;
        }
    }
}

public class ErrorMockAiProvider : IAiProvider
{
    public string ProviderName => "ErrorMock-OpenAI";
    public string ModelName => "gpt-error";

    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            yield break;

        await Task.Delay(10, cancellationToken);
        
        throw new HttpRequestException("OpenAI API is down or timeout.");
    }
}

public class PartialMockAiProvider : IAiProvider
{
    public string ProviderName => "PartialMock-OpenAI";
    public string ModelName => "gpt-partial";

    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return "Đây ";
        await Task.Delay(50); 
        yield return "là ";
        
        
        throw new TaskCanceledException("Client disconnected midway.");
    }
}

public class MockCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult<T?>(default);
    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default) => Task.FromResult(true);
    public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.FromResult(1L);
}

public class AiStreamingTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AiStreamingTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

        [Fact]
    public async Task StreamReading_ValidRequest_ShouldReturnSseAndCompleteState()
    {
        
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
                services.RemoveAll(typeof(ICacheService));
                services.AddScoped<ICacheService, MockCacheService>();
            });
        });
        
        var client = refinedFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        
        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "test@tarotnow.com",
                username: "TestUser",
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

        
        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        
        var walletTx = WalletTransaction.Create(new WalletTransactionCreateRequest
        {
            UserId = userId,
            Currency = CurrencyType.Diamond,
            Type = TransactionType.AdminTopup,
            Amount = 100,
            BalanceBefore = 0,
            BalanceAfter = 100,
            ReferenceSource = "AiRequest",
            ReferenceId = "Seed",
            Description = "Seed Balance",
            MetadataJson = null,
            IdempotencyKey = "seed_idempotency_123"
        });
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned {response.StatusCode}. Body: {errorBody}");
        }
        
        
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);

        
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        var output = new System.Text.StringBuilder();
        while (true)
        {
            var line = await reader.ReadLineAsync();
            if (line == null)
            {
                break;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                output.AppendLine(line);
            }
        }

        var fullStreamString = output.ToString();
        
        
        Assert.Contains("data: Đây", fullStreamString);
        Assert.Contains("data: [DONE]", fullStreamString);

        
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var aiReqs = await assertDb.AiRequests.ToListAsync();
        var sessionRef = session.Id.ToString();
        var aiReq = aiReqs
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => string.Equals(r.ReadingSessionRef, sessionRef, StringComparison.OrdinalIgnoreCase));
        
        if (aiReq == null)
        {
            var msg = $"AiRequest not found for Session: {session.Id}. Available Refs: " + string.Join(", ", aiReqs.Select(r => r.ReadingSessionRef));
            throw new Exception(msg);
        }
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.Completed, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt);       
        Assert.NotNull(aiReq.CompletionMarkerAt);  
    }

        [Fact]
    public async Task StreamReading_ExceedsDailyQuota_ShouldReturnBadRequest()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
                services.RemoveAll(typeof(ICacheService));
                services.AddScoped<ICacheService, MockCacheService>();
            });
        });
        
        var client = refinedFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        
        
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "test2@tarotnow.com",
                username: "TestUser2",
                passwordHash: "hash",
                displayName: "Test2",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[1]");
        await readingRepo.CreateAsync(session);

        
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId.ToString(),
                Status = AiRequestStatus.Completed,
                IdempotencyKey = $"test_daily_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow
            };
            db.AiRequests.Add(testReq);
        }
        await db.SaveChangesAsync();

        
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Daily AI request quota exceeded", resBody);
    }

        [Fact]
    public async Task StreamReading_ExceedsInFlightCap_ShouldReturnBadRequest()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
                services.RemoveAll(typeof(ICacheService));
                services.AddScoped<ICacheService, MockCacheService>();
            });
        });
        
        var client = refinedFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "test3@tarotnow.com",
                username: "TestUser3",
                passwordHash: "hash",
                displayName: "Test3",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[2]");
        await readingRepo.CreateAsync(session);

        
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId.ToString(),
                Status = AiRequestStatus.Requested,
                IdempotencyKey = $"test_inflight_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10 * i) 
            };
            db.AiRequests.Add(testReq);
        }
        await db.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Too many in-flight AI requests", resBody);
    }

        [Fact]
    public async Task StreamReading_FailedBeforeFirstToken_ShouldRefundDiamond()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, ErrorMockAiProvider>(); 
                services.RemoveAll(typeof(ICacheService));
                services.AddScoped<ICacheService, MockCacheService>();
            });
        });
        
        var client = refinedFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "test4@tarotnow.com",
                username: "TestUser4",
                passwordHash: "hash",
                displayName: "Test4",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[1,2,3]");
        await readingRepo.CreateAsync(session);

        var walletTx = WalletTransaction.Create(new WalletTransactionCreateRequest
        {
            UserId = userId,
            Currency = CurrencyType.Diamond,
            Type = TransactionType.AdminTopup,
            Amount = 100,
            BalanceBefore = 0,
            BalanceAfter = 100,
            ReferenceSource = "AiRequest",
            ReferenceId = "Seed",
            Description = "Seed Balance Test4",
            MetadataJson = null,
            IdempotencyKey = "seed_idempotency_test4"
        });
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var resBody = await response.Content.ReadAsStringAsync();

        
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        Assert.Equal(100, userAfter.DiamondBalance); 

        
        var sessionRef = session.Id.ToString();
        var aiReq = assertDb.AiRequests
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => r.ReadingSessionRef != null && EF.Functions.ILike(r.ReadingSessionRef, sessionRef));
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, aiReq.Status);
        Assert.Null(aiReq.FirstTokenAt); 
        Assert.Contains("OpenAI API is down", aiReq.FinishReason ?? string.Empty);

        Assert.Equal(0, aiReq.ChargeDiamond);
        
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.Null(refundTx);
    }

        [Fact]
    public async Task StreamReading_FailedAfterFirstToken_FromUpstreamCancellation_ShouldRefundDiamond()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, PartialMockAiProvider>(); 
                services.RemoveAll(typeof(ICacheService));
                services.AddScoped<ICacheService, MockCacheService>();
            });
        });
        
        var client = refinedFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();
        
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = new User(
                email: "test5@tarotnow.com",
                username: "TestUser5",
                passwordHash: "hash",
                displayName: "Test5",
                dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[4]");
        await readingRepo.CreateAsync(session);

        var walletTx = WalletTransaction.Create(new WalletTransactionCreateRequest
        {
            UserId = userId,
            Currency = CurrencyType.Diamond,
            Type = TransactionType.AdminTopup,
            Amount = 100,
            BalanceBefore = 0,
            BalanceAfter = 100,
            ReferenceSource = "AiRequest",
            ReferenceId = "Seed",
            Description = "Seed Balance Test5",
            MetadataJson = null,
            IdempotencyKey = "seed_idempotency_test5"
        });
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var resBody = await response.Content.ReadAsStringAsync();

        
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        Assert.Equal(100, userAfter.DiamondBalance); 

        
        var sessionRef = session.Id.ToString();
        var aiReq = assertDb.AiRequests
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => r.ReadingSessionRef != null && EF.Functions.ILike(r.ReadingSessionRef, sessionRef));
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt); 
        Assert.Contains("Upstream timeout/cancellation", aiReq.FinishReason ?? string.Empty);

        Assert.Equal(0, aiReq.ChargeDiamond);
        
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.Null(refundTx);
    }
}
