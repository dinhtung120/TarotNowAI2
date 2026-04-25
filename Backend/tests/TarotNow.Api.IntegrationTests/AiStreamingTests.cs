

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

// Mock provider trả stream thành công theo chunk để kiểm thử happy path.
public class MockAiProvider : IAiProvider
{
    // Tên provider giả lập cho metadata test.
    public string ProviderName => "Mock-OpenAI";
    // Tên model giả lập cho metadata test.
    public string ModelName => "gpt-mock-mini";

    /// <summary>
    /// Bỏ qua ghi log trong test để tập trung vào luồng streaming.
    /// Luồng trả CompletedTask vì test không cần kiểm tra persistence telemetry.
    /// </summary>
    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// Trả về chuỗi chunk mẫu để mô phỏng stream từ provider thật.
    /// Luồng đọc tuần tự từng chunk và tôn trọng cancellation token.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string[] chunks = { "Đây ", "là ", "kết ", "quả ", "giải ", "bài ", "mẫu " };

        foreach (var chunk in chunks)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // Dừng stream ngay khi request bị hủy từ client/test.
                yield break;
            }

            // Delay ngắn để mô phỏng provider trả token theo thời gian.
            await Task.Delay(50, cancellationToken);
            yield return chunk;
        }
    }
}

// Mock provider ném lỗi ngay đầu stream để kiểm thử nhánh failed-before-first-token.
public class ErrorMockAiProvider : IAiProvider
{
    // Tên provider giả lập cho metadata test.
    public string ProviderName => "ErrorMock-OpenAI";
    // Tên model giả lập cho metadata test.
    public string ModelName => "gpt-error";

    /// <summary>
    /// Bỏ qua ghi log trong test provider lỗi.
    /// Luồng này tránh side effect không liên quan đến kịch bản test.
    /// </summary>
    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// Mô phỏng provider bị sự cố trước khi trả token đầu tiên.
    /// Luồng ném HttpRequestException để backend đi vào nhánh xử lý lỗi upstream.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            // Nếu bị hủy sớm thì kết thúc để tránh ném lỗi giả.
            yield break;
        }

        await Task.Delay(10, cancellationToken);
        // Ném lỗi mạng mô phỏng OpenAI down/timeout.
        throw new HttpRequestException("OpenAI API is down or timeout.");
    }
}

// Mock provider trả một phần dữ liệu rồi hủy để kiểm thử failed-after-first-token.
public class PartialMockAiProvider : IAiProvider
{
    // Tên provider giả lập cho metadata test.
    public string ProviderName => "PartialMock-OpenAI";
    // Tên model giả lập cho metadata test.
    public string ModelName => "gpt-partial";

    /// <summary>
    /// Bỏ qua ghi log trong test provider hủy giữa chừng.
    /// Luồng giữ test tập trung vào hành vi stream và refund.
    /// </summary>
    public Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    /// <summary>
    /// Trả vài token đầu rồi ném TaskCanceledException.
    /// Luồng mô phỏng upstream timeout/cancellation sau khi đã phát token đầu tiên.
    /// </summary>
    public async IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        yield return "Đây ";
        await Task.Delay(50);
        yield return "là ";

        // Ném hủy giữa luồng để backend phân loại đúng trạng thái failed-after-first-token.
        throw new TaskCanceledException("Client disconnected midway.");
    }
}

// Mock cache luôn cho qua để cô lập test khỏi behavior cache runtime.
public class MockCacheService : ICacheService
{
    /// <summary>
    /// Trả cache miss cho mọi key.
    /// Luồng này buộc backend đi vào nhánh xử lý thật trong các test.
    /// </summary>
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Task.FromResult<T?>(default);
    /// <summary>
    /// Bỏ qua ghi cache trong môi trường test.
    /// Luồng no-op giúp test deterministic và không phụ thuộc state cache.
    /// </summary>
    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.CompletedTask;
    /// <summary>
    /// Bỏ qua xóa cache trong môi trường test.
    /// Luồng no-op giữ test nhẹ và tập trung vào hành vi nghiệp vụ.
    /// </summary>
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) => Task.CompletedTask;
    /// <summary>
    /// Luôn cho phép rate-limit để không chặn các kịch bản test streaming.
    /// Luồng này cô lập test khỏi chính sách giới hạn tần suất cache-backed.
    /// </summary>
    public Task<bool> CheckRateLimitAsync(string key, TimeSpan limitWindow, CancellationToken cancellationToken = default) => Task.FromResult(true);
    /// <summary>
    /// Trả giá trị tăng cố định cho các luồng cần bộ đếm.
    /// Luồng deterministic hóa kết quả để assertion ổn định.
    /// </summary>
    public Task<long> IncrementAsync(string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.FromResult(1L);

    /// <summary>
    /// Bỏ qua thêm set member trong test.
    /// </summary>
    public Task AddToSetAsync(string key, string member, TimeSpan? expiration = null, CancellationToken cancellationToken = default) => Task.CompletedTask;

    /// <summary>
    /// Bỏ qua xóa set member trong test.
    /// </summary>
    public Task RemoveFromSetAsync(string key, string member, CancellationToken cancellationToken = default) => Task.CompletedTask;

    /// <summary>
    /// Trả set rỗng trong test.
    /// </summary>
    public Task<IReadOnlyCollection<string>> GetSetMembersAsync(string key, CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyCollection<string>>(Array.Empty<string>());

    /// <summary>
    /// Luôn acquire lock thành công trong test để không chặn luồng kiểm thử.
    /// </summary>
    public Task<bool> AcquireLockAsync(
        string key,
        string owner,
        TimeSpan leaseTime,
        CancellationToken cancellationToken = default) => Task.FromResult(true);

    /// <summary>
    /// Luôn release lock thành công trong test.
    /// </summary>
    public Task<bool> ReleaseLockAsync(
        string key,
        string owner,
        CancellationToken cancellationToken = default) => Task.FromResult(true);
}

// Bộ integration tests cho endpoint stream AI và các nhánh quota/error liên quan.
public class AiStreamingTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Factory tích hợp tạo host test với DI và database thật (container).
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class streaming với factory tích hợp.
    /// Luồng dùng chung factory để tái sử dụng cấu hình test host.
    /// </summary>
    public AiStreamingTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Kiểm thử happy path: stream trả SSE hợp lệ và AiRequest kết thúc trạng thái Completed.
    /// Luồng mock provider thành công, seed dữ liệu phiên đọc, gọi endpoint stream rồi assert DB state.
    /// </summary>
    [Fact]
    public async Task StreamReading_ValidRequest_ShouldReturnSseAndCompleteState()
    {
        // Ghi đè IAiProvider/ICacheService để kiểm soát hoàn toàn hành vi stream trong test.
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

        // Dùng auth test scheme để endpoint yêu cầu xác thực chấp nhận request.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        // Lấy scope dữ liệu để seed user/session/wallet phục vụ luồng stream.
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

            // Set Id qua reflection để cố định user test, tránh lệch dữ liệu assert.
            typeof(User).GetProperty("Id")?.SetValue(user, userId);

            // Kích hoạt user và nạp sẵn kim cương để tránh fail do thiếu số dư.
            user.Activate();
            user.Credit(CurrencyType.Diamond, 100L, TransactionType.AdminTopup);

            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Seed reading session đã complete để endpoint stream có dữ liệu đầu vào hợp lệ.
        var sessionId = Guid.NewGuid();
        var session = new ReadingSession(userId.ToString(), SpreadType.Daily1Card);
        typeof(ReadingSession).GetProperty("Id")?.SetValue(session, sessionId.ToString());
        session.CompleteSession("[12]");
        await readingRepo.CreateAsync(session);

        // Seed wallet transaction nền để hệ thống có lịch sử ví nhất quán khi tính charge/refund.
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

        // Gọi endpoint stream theo chuẩn SSE.
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // Nếu fail thì ném exception chi tiết để debug test dễ hơn.
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned {response.StatusCode}. Body: {errorBody}");
        }

        // Xác nhận response đúng MIME type của Server-Sent Events.
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);

        // Đọc toàn bộ stream line-by-line để kiểm tra token và marker kết thúc.
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

        // Assert có token đầu ra và marker [DONE] từ luồng stream.
        Assert.Contains("data: Đây", fullStreamString);
        Assert.Contains("data: [DONE]", fullStreamString);

        // Kiểm tra dữ liệu AI request đã lưu đúng trạng thái completed trong DB.
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var aiReqs = await assertDb.AiRequests.ToListAsync();
        var aiReq = aiReqs
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => r.ReadingSessionRef == sessionId);

        if (aiReq == null)
        {
            // Trả lỗi kèm danh sách ref hiện có để hỗ trợ điều tra mapping session ref.
            var msg = $"AiRequest not found for Session: {session.Id}. Available Refs: " + string.Join(", ", aiReqs.Select(r => r.ReadingSessionRef));
            throw new Exception(msg);
        }
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.Completed, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt);
        Assert.NotNull(aiReq.CompletionMarkerAt);
    }

    /// <summary>
    /// Xác nhận endpoint stream từ chối khi người dùng đã hết quota AI trong ngày.
    /// Luồng seed 3 request completed trong ngày rồi gọi stream mới để kỳ vọng 400.
    /// </summary>
    [Fact]
    public async Task StreamReading_ExceedsDailyQuota_ShouldReturnBadRequest()
    {
        // Ghi đè provider/cache để test tập trung vào policy quota thay vì tích hợp bên ngoài.
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
        // Override userId header để tách biệt dữ liệu khỏi test khác.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());

        using var scope = refinedFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readingRepo = scope.ServiceProvider.GetRequiredService<IReadingSessionRepository>();

        // Seed user nếu chưa tồn tại để luồng request hợp lệ.
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

        // Seed đúng ngưỡng quota hằng ngày để request kế tiếp bị từ chối.
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId,
                Status = AiRequestStatus.Completed,
                IdempotencyKey = $"test_daily_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow
            };
            db.AiRequests.Add(testReq);
        }
        await db.SaveChangesAsync();

        // Gọi stream mới và kiểm tra thông điệp vượt quota.
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Daily AI request quota exceeded", resBody);
    }

    /// <summary>
    /// Xác nhận endpoint stream từ chối khi số request in-flight đã đạt trần.
    /// Luồng seed 3 request trạng thái Requested rồi gọi stream mới để kỳ vọng 400.
    /// </summary>
    [Fact]
    public async Task StreamReading_ExceedsInFlightCap_ShouldReturnBadRequest()
    {
        // Ghi đè provider/cache để cố định hành vi phụ trợ trong test.
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
        // Override userId để kịch bản in-flight cap không đụng dữ liệu test khác.
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

        // Seed 3 request Requested để mô phỏng đang có nhiều request chạy đồng thời.
        for (int i = 0; i < 3; i++)
        {
            var testReq = new AiRequest
            {
                UserId = userId,
                ReadingSessionRef = sessionId,
                Status = AiRequestStatus.Requested,
                IdempotencyKey = $"test_inflight_{i}",
                ChargeDiamond = 5,
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10 * i)
            };
            db.AiRequests.Add(testReq);
        }
        await db.SaveChangesAsync();

        // Gọi stream mới và kỳ vọng bị chặn bởi in-flight cap.
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/sessions/{session.Id}/stream");
        var response = await client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var resBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Too many in-flight AI requests", resBody);
    }

    /// <summary>
    /// Xác nhận lỗi trước token đầu tiên không trừ kim cương và ghi trạng thái FailedBeforeFirstToken.
    /// Luồng dùng provider ném HttpRequestException ngay đầu stream rồi kiểm tra DB/user balance.
    /// </summary>
    [Fact]
    public async Task StreamReading_FailedBeforeFirstToken_ShouldRefundDiamond()
    {
        // Ép IAiProvider về nhánh lỗi sớm để kiểm chứng behavior xử lý thất bại.
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
        // Override userId để dữ liệu test lỗi sớm không chồng lấn kịch bản khác.
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
        _ = resBody;

        // Kiểm tra hậu điều kiện trong DB sau khi stream thất bại.
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        // Không trừ phí khi chưa nhận token đầu tiên từ provider.
        Assert.Equal(100, userAfter.DiamondBalance);

        // AiRequest phải được đánh dấu failed-before-first-token và không có first token timestamp.
        var aiReq = assertDb.AiRequests
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => r.ReadingSessionRef == sessionId);
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, aiReq.Status);
        Assert.Null(aiReq.FirstTokenAt);
        Assert.Contains("OpenAI API is down", aiReq.FinishReason ?? string.Empty);

        // Không thu phí nên charge phải bằng 0 và không có giao dịch refund phát sinh.
        Assert.Equal(0, aiReq.ChargeDiamond);
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.Null(refundTx);
    }

    /// <summary>
    /// Xác nhận lỗi sau token đầu tiên được phân loại FailedAfterFirstToken và không làm lệch số dư.
    /// Luồng dùng provider trả một phần dữ liệu rồi ném TaskCanceledException.
    /// </summary>
    [Fact]
    public async Task StreamReading_FailedAfterFirstToken_FromUpstreamCancellation_ShouldRefundDiamond()
    {
        // Ép IAiProvider vào nhánh hủy giữa chừng để kiểm tra phân loại lỗi sau first token.
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
        // Override userId để tách dữ liệu test cancellation khỏi các kịch bản khác.
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
        _ = resBody;

        // Kiểm tra hậu điều kiện DB để xác nhận phân loại lỗi và charge chính xác.
        using var assertScope = refinedFactory.Services.CreateScope();
        var assertDb = assertScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userAfter = assertDb.Users.First(u => u.Id == userId);
        // Số dư người dùng phải giữ nguyên vì kịch bản test không thu phí thành công.
        Assert.Equal(100, userAfter.DiamondBalance);

        // AiRequest phải có first token nhưng kết thúc ở trạng thái failed-after-first-token.
        var aiReq = assertDb.AiRequests
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault(r => r.ReadingSessionRef == sessionId);
        Assert.NotNull(aiReq);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, aiReq.Status);
        Assert.NotNull(aiReq.FirstTokenAt);
        Assert.Contains("Upstream timeout/cancellation", aiReq.FinishReason ?? string.Empty);

        // Không phát sinh charge/refund ở nhánh lỗi này theo policy hiện tại.
        Assert.Equal(0, aiReq.ChargeDiamond);
        var refundTx = assertDb.WalletTransactions.FirstOrDefault(t => t.UserId == userId && t.Type == TransactionType.EscrowRefund);
        Assert.Null(refundTx);
    }
}
