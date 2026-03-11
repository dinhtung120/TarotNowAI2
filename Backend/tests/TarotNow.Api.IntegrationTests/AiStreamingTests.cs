using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Interfaces;
using TarotNow.Infrastructure.Persistence;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Authentication;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

/// <summary>
/// Mock AI Provider để giả lập luồng Stream trả về, thay vì mất tiền gọi API ngoài thật.
/// </summary>
public class MockAiProvider : IAiProvider
{
    public string ProviderName => "Mock-OpenAI";
    public string ModelName => "gpt-mock-mini";

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
/// Mock AI Provider giả định lỗi mạng hoặc Timeout ngay lập tức (Trước khi gửi Token đầu tiên).
/// Dùng để test tính năng Auto-Refund.
/// </summary>
public class ErrorMockAiProvider : IAiProvider
{
    public string ProviderName => "ErrorMock-OpenAI";
    public string ModelName => "gpt-error";

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        throw new HttpRequestException("OpenAI API is down or timeout.");
        yield break;
    }
}

/// <summary>
/// Mock AI Provider cho phép nhả 1 vài Token trước, sau đó mô phỏng bị ngắt kết nối.
/// Dùng để test tính năng Không Refund khi lỗi xảy ra sau First Token.
/// </summary>
public class PartialMockAiProvider : IAiProvider
{
    public string ProviderName => "PartialMock-OpenAI";
    public string ModelName => "gpt-partial";

    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return "Đây ";
        await Task.Delay(10, cancellationToken);
        yield return "là ";
        
        // Mô phỏng ngắt ngang luồng bằng cách văng exception sau khi đã gửi Token
        await Task.Delay(10, cancellationToken);
        throw new TaskCanceledException("Client disconnected midway.");
    }
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
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Set Default HttpClient Auth Header matching Mock TestAuthHandler
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        // Seed DB
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(User));
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Email")?.SetValue(user, "test@tarotnow.com");
            typeof(User).GetProperty("Username")?.SetValue(user, "TestUser");
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(User).GetProperty("DisplayName")?.SetValue(user, "Test");
            typeof(User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(User).GetProperty("Status")?.SetValue(user, UserStatus.Active);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.User);
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("DiamondBalance")?.SetValue(user, 100L);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId, SpreadType.Daily1Card);
        
        // Cấp Session ID force
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId);
        
        db.ReadingSessions.Add(session);
        session.CompleteSession("[12]");
        await db.SaveChangesAsync();

        // Cấp quỹ Diamond
        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance", null, "seed_idempotency_123");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        // 2. Act - Bắn cURL GET tới API SSE
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // 3. Assert
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned {response.StatusCode}. Body: {errorBody}");
        }
        
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);

        // Đọc nội dung Stream
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
        
        // Kiểm chứng format chuỗi SSE chuẩn
        Assert.Contains("data: Đây", fullStreamString);
        Assert.Contains("data: [DONE]", fullStreamString);

        // Kiểm chứng State Machine DB
        var aiReq = db.AiRequests.OrderByDescending(r => r.CreatedAt).FirstOrDefault(r => r.ReadingSessionRef == session.Id.ToString());
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.Completed, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt);
        Assert.NotNull(aiReq.CompletionMarkerAt);
    }

    [Fact]
    public async Task StreamReading_ExceedsDailyQuota_ShouldReturnBadRequest()
    {
        // 1. Arrange & Setup
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Seed new TestUser
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(User));
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Email")?.SetValue(user, "test2@tarotnow.com");
            typeof(User).GetProperty("Username")?.SetValue(user, "TestUser2");
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(User).GetProperty("DisplayName")?.SetValue(user, "Test2");
            typeof(User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(User).GetProperty("Status")?.SetValue(user, UserStatus.Active);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.User);
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("DiamondBalance")?.SetValue(user, 100L);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId, SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId);
        db.ReadingSessions.Add(session);
        session.CompleteSession("[1]");

        // Seed 3 AiRequests with Completed Status for today to hit Daily Quota Guard
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

        // 2. Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        // 3. Assert (Guard 1 throws 400 Bad Request directly without processing stream)
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Daily AI request quota exceeded", resBody);
    }

    [Fact]
    public async Task StreamReading_ExceedsInFlightCap_ShouldReturnBadRequest()
    {
        // 1. Arrange & Setup
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, MockAiProvider>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Seed new TestUser
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(User));
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Email")?.SetValue(user, "test3@tarotnow.com");
            typeof(User).GetProperty("Username")?.SetValue(user, "TestUser3");
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(User).GetProperty("DisplayName")?.SetValue(user, "Test3");
            typeof(User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(User).GetProperty("Status")?.SetValue(user, UserStatus.Active);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.User);
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("DiamondBalance")?.SetValue(user, 100L);
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId, SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId);
        db.ReadingSessions.Add(session);
        session.CompleteSession("[2]");

        // Seed 3 AiRequests with 'Requested' Status (In-flight) to hit Global Guard limits
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId.ToString(),
                Status = AiRequestStatus.Requested,
                IdempotencyKey = $"test_inflight_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10 * i) // Prevent Db Update Exceptions
            };
            db.AiRequests.Add(testReq);
        }
        await db.SaveChangesAsync();

        // 2. Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        // 3. Assert (Guard 1.5 throws 400 Bad Request directly)
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Too many in-flight AI requests", resBody);
    }

    [Fact]
    public async Task StreamReading_FailedBeforeFirstToken_ShouldRefundDiamond()
    {
        // 1. Arrange & Setup
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, ErrorMockAiProvider>(); // Use Error Mock
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Seed new TestUser with 100 Diamond
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(User));
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Email")?.SetValue(user, "test4@tarotnow.com");
            typeof(User).GetProperty("Username")?.SetValue(user, "TestUser4");
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(User).GetProperty("DisplayName")?.SetValue(user, "Test4");
            typeof(User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(User).GetProperty("Status")?.SetValue(user, UserStatus.Active);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.User);
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("DiamondBalance")?.SetValue(user, 100L); // Init 100
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId, SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId);
        db.ReadingSessions.Add(session);
        session.CompleteSession("[3]");
        await db.SaveChangesAsync();

        // Cấp quỹ Diamond (Ledger)
        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance Test4", null, "seed_idempotency_test4");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        // 2. Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // Đọc hết stream để trigger xử lý ngầm (đón lỗi từ ErrorMockProvider)
        var resBody = await response.Content.ReadAsStringAsync();

        // 3. Assert Backend Database State Check
        // Tạo Scope MỚI để load lại data mới nhất từ DB
        using var assertScope = _factory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        
        // Trừ 5 Diamond khi Requested -> Còn 95. Sau Exception Refund trả lại 5 -> Lại đầy 100.
        Assert.Equal(100, userAfter.DiamondBalance);

        // Kiểm tra State Machine
        var aiReq = assertDb.AiRequests.FirstOrDefault(r => r.ReadingSessionRef == session.Id.ToString());
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, aiReq.Status);
        Assert.Null(aiReq.FirstTokenAt);
        Assert.Contains("OpenAI API is down", aiReq.FinishReason ?? "");

        // Kiểm tra bản lưu lịch sử Refund vào Ledger Wallet
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.NotNull(refundTx);
        Assert.Equal(5, refundTx.Amount);
    }

    [Fact]
    public async Task StreamReading_FailedAfterFirstToken_ShouldNotRefundDiamond()
    {
        // 1. Arrange & Setup
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IAiProvider));
                services.AddScoped<IAiProvider, PartialMockAiProvider>(); // Use Partial Mock
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000005");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Seed new TestUser with 100 Diamond
        if (!db.Users.Any(u => u.Id == userId))
        {
            var user = (User)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(User));
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Email")?.SetValue(user, "test5@tarotnow.com");
            typeof(User).GetProperty("Username")?.SetValue(user, "TestUser5");
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hash");
            typeof(User).GetProperty("DisplayName")?.SetValue(user, "Test5");
            typeof(User).GetProperty("DateOfBirth")?.SetValue(user, new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            typeof(User).GetProperty("Status")?.SetValue(user, UserStatus.Active);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.User);
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("DiamondBalance")?.SetValue(user, 100L); // Init 100
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId, SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId);
        db.ReadingSessions.Add(session);
        session.CompleteSession("[4]");
        await db.SaveChangesAsync();

        // Cấp quỹ Diamond (Ledger)
        var walletTx = WalletTransaction.Create(
            userId, CurrencyType.Diamond, TransactionType.AdminTopup, 100, 0, 100, "AiRequest", "Seed", "Seed Balance Test5", null, "seed_idempotency_test5");
        db.WalletTransactions.Add(walletTx);
        await db.SaveChangesAsync();

        // 2. Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // Đọc stream để đón lấy 1-2 chunks trước khi lỗi văng
        var resBody = await response.Content.ReadAsStringAsync();

        // 3. Assert Backend Database State Check
        // Tạo Scope MỚI để load lại data mới nhất từ DB
        using var assertScope = _factory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        
        // Trừ 5 Diamond khi Requested -> Còn 95. DO KHÔNG REFUND NÊN GIỮ NGUYÊN 95.
        Assert.Equal(95, userAfter.DiamondBalance);

        // Kiểm tra State Machine
        var aiReq = assertDb.AiRequests.FirstOrDefault(r => r.ReadingSessionRef == session.Id.ToString());
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt); // Phải có First Token At
        Assert.Contains("Client disconnected midway", aiReq.FinishReason ?? "");

        // Kiểm tra KHÔNG CÓ bản lưu lịch sử Refund
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.Null(refundTx);
    }
}
