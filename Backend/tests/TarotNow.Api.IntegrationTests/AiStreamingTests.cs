/*
 * FILE: AiStreamingTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra luồng AI Streaming end-to-end.
 *   ĐÂY LÀ BỘ TEST PHỨC TẠP NHẤT — kiểm tra cả SSE streaming, quota guards, và escrow refund.
 *
 *   CÁC MOCK PROVIDERS:
 *   → MockAiProvider: trả về 7 chunks text bình thường (happy path)
 *   → ErrorMockAiProvider: throw HttpRequestException ngay lập tức (lỗi trước first token)
 *   → PartialMockAiProvider: trả 2 chunks rồi throw TaskCanceledException (lỗi sau first token)
 *   → MockCacheService: bypass rate limiting (luôn trả true)
 *
 *   CÁC TEST CASE (5 scenarios):
 *   1. StreamReading_ValidRequest_ShouldReturnSseAndCompleteState:
 *      → Happy path: stream SSE → verify "data: Đây" + "[DONE]" + AiRequest status = Completed
 *   2. StreamReading_ExceedsDailyQuota_ShouldReturnBadRequest:
 *      → Vượt quota 3/ngày → 400 Bad Request "Daily AI request quota exceeded"
 *   3. StreamReading_ExceedsInFlightCap_ShouldReturnBadRequest:
 *      → Quá nhiều request đang xử lý → 400 "Too many in-flight AI requests"
 *   4. StreamReading_FailedBeforeFirstToken_ShouldRefundDiamond:
 *      → AI fail TRƯỚC first token → auto-refund Diamond (100 → 95 → 100)
 *   5. StreamReading_FailedAfterFirstToken_FromUpstreamCancellation_ShouldRefundDiamond:
 *      → AI fail SAU first token (upstream cancel) → vẫn refund Diamond
 *
 *   PATTERN TEST:
 *   → Seed data (User + Session + AiRequests) → Act (gọi API) → Assert (DB state + response)
 *   → Reflection setId: vì entity Id là private set → dùng reflection để seed data
 *   → WithWebHostBuilder: override DI services per-test (inject mock providers)
 */

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

/// <summary>
/// Mock AI Provider: trả về 7 chunks text bình thường (giả lập streaming thành công).
/// Dùng cho happy path tests — không gọi OpenAI API thật (tiết kiệm chi phí).
/// </summary>
public class MockAiProvider : IAiProvider
{
    public string ProviderName => "Mock-OpenAI";
    public string ModelName => "gpt-mock-mini";

    public Task LogRequestAsync(
        Guid userId,
        string? sessionId,
        string? requestId,
        int inputTokens,
        int outputTokens,
        int latencyMs,
        string status,
        string? errorCode = null) => Task.CompletedTask;

    /// <summary>
    /// Stream 7 chunks text với delay 50ms/chunk (giả lập network latency).
    /// IAsyncEnumerable: yield return từng chunk → controller push qua SSE.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string[] chunks = { "Đây ", "là ", "kết ", "quả ", "giải ", "bài ", "mẫu " };
        
        foreach (var chunk in chunks)
        {
            if (cancellationToken.IsCancellationRequested)
                yield break;
                
            await Task.Delay(50, cancellationToken); // Giả lập độ trễ mạng
            yield return chunk;
        }
    }
}

/// <summary>
/// Mock AI Provider GÂY LỖI: throw HttpRequestException ngay (trước first token).
/// Dùng để test luồng auto-refund khi AI fail trước khi gửi bất kỳ dữ liệu nào.
/// </summary>
public class ErrorMockAiProvider : IAiProvider
{
    public string ProviderName => "ErrorMock-OpenAI";
    public string ModelName => "gpt-error";

    public Task LogRequestAsync(
        Guid userId,
        string? sessionId,
        string? requestId,
        int inputTokens,
        int outputTokens,
        int latencyMs,
        string status,
        string? errorCode = null) => Task.CompletedTask;

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            yield break;

        await Task.Delay(10, cancellationToken);
        // Mô phỏng OpenAI API down → trigger auto-refund flow
        throw new HttpRequestException("OpenAI API is down or timeout.");
    }
}

/// <summary>
/// Mock AI Provider PARTIAL: gửi 2 chunks rồi throw (mô phỏng disconnect giữa stream).
/// Dùng để test: lỗi SAU first token → vẫn refund (vì upstream cancellation, không phải user cancel).
/// </summary>
public class PartialMockAiProvider : IAiProvider
{
    public string ProviderName => "PartialMock-OpenAI";
    public string ModelName => "gpt-partial";

    public Task LogRequestAsync(
        Guid userId,
        string? sessionId,
        string? requestId,
        int inputTokens,
        int outputTokens,
        int latencyMs,
        string status,
        string? errorCode = null) => Task.CompletedTask;

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return "Đây ";
        await Task.Delay(50); // Đảm bảo controller nhận được chunk đầu
        yield return "là ";
        
        // Mô phỏng disconnect giữa stream (upstream cancellation)
        throw new TaskCanceledException("Client disconnected midway.");
    }
}

/// <summary>
/// Mock Cache Service: bypass tất cả rate limiting + quota (luôn trả true/1).
/// Dùng cho integration tests — không cần Redis container cho cache logic.
/// </summary>
public class MockCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult<T?>(default);
    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default) => Task.FromResult(true);
    public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.FromResult(1L);
}

/// <summary>
/// Integration tests cho AI Streaming — kiểm tra SSE, quota, escrow refund.
/// </summary>
public class AiStreamingTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AiStreamingTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// HAPPY PATH: Stream SSE bình thường.
    /// Verify: response có "data: Đây" + "[DONE]", AiRequest status = Completed.
    /// </summary>
    [Fact]
    public async Task StreamReading_ValidRequest_ShouldReturnSseAndCompleteState()
    {
        // === ARRANGE: override DI → mock providers ===
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

        // Mock auth header
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        // Seed: tạo User + ReadingSession + WalletTransaction trong DB
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
            
            // Reflection: set private Id (không có public setter)
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Tạo ReadingSession đã reveal (có drawn_cards)
        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        // Seed WalletTransaction (ledger entry cho Diamond)
        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance", null, "seed_idempotency_123");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        // === ACT: gọi SSE endpoint ===
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // === ASSERT: kiểm tra response + DB state ===
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned {response.StatusCode}. Body: {errorBody}");
        }
        
        // Content-Type phải là SSE
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);

        // Đọc toàn bộ stream
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        var output = new System.Text.StringBuilder();
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
                output.AppendLine(line);
        }

        var fullStreamString = output.ToString();
        
        // Verify SSE format: phải chứa "data: Đây" và "[DONE]"
        Assert.Contains("data: Đây", fullStreamString);
        Assert.Contains("data: [DONE]", fullStreamString);

        // Verify DB: AiRequest phải ở status Completed + có timestamp
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var aiReqs = await assertDb.AiRequests.ToListAsync();
        var aiReq = aiReqs.OrderByDescending(r => r.CreatedAt).FirstOrDefault(r => r.ReadingSessionRef?.ToLower() == session.Id.ToString().ToLower());
        
        if (aiReq == null)
        {
            var msg = $"AiRequest not found for Session: {session.Id}. Available Refs: " + string.Join(", ", aiReqs.Select(r => r.ReadingSessionRef));
            throw new Exception(msg);
        }
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.Completed, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt);       // Phải ghi nhận thời điểm nhận token đầu
        Assert.NotNull(aiReq.CompletionMarkerAt);  // Phải ghi nhận thời điểm stream hoàn tất
    }

    /// <summary>
    /// GUARD 1: Vượt quota hàng ngày (3 requests/ngày) → 400 Bad Request.
    /// Seed 3 AiRequests Completed cho hôm nay → request thứ 4 phải bị chặn.
    /// </summary>
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
        
        // Seed User
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

        // Seed 3 AiRequests Completed hôm nay → vượt daily quota
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

        // ACT: request thứ 4 phải bị chặn
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        // ASSERT: 400 + thông báo quota exceeded
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Daily AI request quota exceeded", resBody);
    }

    /// <summary>
    /// GUARD 1.5: Quá nhiều request đang xử lý (in-flight) → 400 Bad Request.
    /// Seed 3 AiRequests ở status Requested (đang streaming) → request mới phải bị chặn.
    /// </summary>
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

        // Seed 3 AiRequests ở status Requested (in-flight) → vượt cap
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId.ToString(),
                Status = AiRequestStatus.Requested,
                IdempotencyKey = $"test_inflight_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10 * i) // Thời gian khác nhau tránh conflict
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

    /// <summary>
    /// AUTO-REFUND: AI fail TRƯỚC first token → Diamond phải được hoàn trả.
    /// Luồng: 100 Diamond → freeze 5 → AI fail → refund 5 → balance = 100 (không mất tiền).
    /// Verify: AiRequest.Status = FailedBeforeFirstToken, FirstTokenAt = null, ledger có refund entry.
    /// </summary>
    [Fact]
    public async Task StreamReading_FailedBeforeFirstToken_ShouldRefundDiamond()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, ErrorMockAiProvider>(); // Inject Error Mock
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

        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance Test4", null, "seed_idempotency_test4");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var resBody = await response.Content.ReadAsStringAsync();

        // ASSERT: Diamond phải quay về 100 (freeze 5 → refund 5 → net = 0)
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        Assert.Equal(100, userAfter.DiamondBalance); // Không mất tiền

        // AiRequest: status = FailedBeforeFirstToken, FirstTokenAt = null
        var aiReq = assertDb.AiRequests.OrderByDescending(r => r.CreatedAt).FirstOrDefault(r => r.ReadingSessionRef.ToLower() == session.Id.ToString().ToLower());
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, aiReq.Status);
        Assert.Null(aiReq.FirstTokenAt); // Chưa nhận được token nào
        Assert.Contains("OpenAI API is down", aiReq.FinishReason ?? "");

        // Ledger: phải có bản ghi refund
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.NotNull(refundTx);
        Assert.Equal(5, refundTx.Amount); // Hoàn 5 Diamond
    }

    /// <summary>
    /// AUTO-REFUND SAU FIRST TOKEN: AI gửi được 2 chunks rồi disconnect (upstream cancel).
    /// Luồng: 100 → freeze 5 → nhận 2 chunks → disconnect → refund 5 → balance = 100.
    /// Verify: Status = FailedAfterFirstToken, FirstTokenAt IS NOT NULL, ledger có refund.
    /// </summary>
    [Fact]
    public async Task StreamReading_FailedAfterFirstToken_FromUpstreamCancellation_ShouldRefundDiamond()
    {
        var refinedFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, PartialMockAiProvider>(); // Inject Partial Mock
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

        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance Test5", null, "seed_idempotency_test5");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        var resBody = await response.Content.ReadAsStringAsync();

        // ASSERT: Diamond phải quay về 100 (dù đã nhận 2 chunks)
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        Assert.Equal(100, userAfter.DiamondBalance); // Không mất tiền

        // AiRequest: status = FailedAfterFirstToken, FirstTokenAt IS NOT NULL
        var aiReq = assertDb.AiRequests.OrderByDescending(r => r.CreatedAt).FirstOrDefault(r => r.ReadingSessionRef.ToLower() == session.Id.ToString().ToLower());
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt); // Đã nhận được token trước khi lỗi
        Assert.Contains("Upstream timeout/cancellation", aiReq.FinishReason ?? "");

        // Ledger: có bản ghi refund
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.NotNull(refundTx);
        Assert.Equal(5, refundTx.Amount);
    }
}
