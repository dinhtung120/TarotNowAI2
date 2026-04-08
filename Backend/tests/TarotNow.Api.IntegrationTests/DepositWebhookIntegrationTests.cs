

using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử luồng webhook nạp tiền: xác minh chữ ký và idempotency.
public class DepositWebhookIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    // Secret test dùng để sinh chữ ký HMAC giống gateway.
    private const string WebhookSecret = "TarotNow_Test_WebhookSecret_2026";
    // Factory tạo HttpClient + service scope cho integration test.
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class webhook với factory tích hợp.
    /// Luồng dùng chung factory giúp mọi test chạy trên cùng hạ tầng container.
    /// </summary>
    public DepositWebhookIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Xác nhận webhook bị từ chối khi chữ ký không hợp lệ.
    /// Luồng gửi payload đúng format nhưng signature giả để kỳ vọng 401.
    /// </summary>
    [Fact]
    public async Task Webhook_ShouldReject_WhenSignatureIsInvalid()
    {
        var client = _factory.CreateClient();

        var payload = new
        {
            OrderId = Guid.NewGuid().ToString(),
            TransactionId = "TX-INVALID",
            Amount = 10000,
            Status = "SUCCESS"
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        // Gắn chữ ký sai để kiểm tra nhánh bảo mật signature validation.
        client.DefaultRequestHeaders.Add("X-Webhook-Signature", "wrong_signature");

        var response = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Xác nhận webhook áp dụng idempotency và chỉ credit một lần.
    /// Luồng gọi cùng payload hai lần với cùng chữ ký rồi kiểm tra trạng thái order và số dư.
    /// </summary>
    [Fact]
    public async Task Webhook_ShouldApplyIdempotency_AndCreditOnce()
    {
        var client = _factory.CreateClient();

        // Seed user và deposit order nền để webhook có dữ liệu đích cần cập nhật.
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userId = Guid.NewGuid();
        var user = new TarotNow.Domain.Entities.User("testdep@test.com", "testdep", "hash", "DisplayName", DateTime.UtcNow, true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        db.Users.Add(user);

        var orderId = Guid.NewGuid();
        var depositOrder = new DepositOrder(userId, 50000, 5);
        typeof(DepositOrder).GetProperty("Id")?.SetValue(depositOrder, orderId);
        db.DepositOrders.Add(depositOrder);
        await db.SaveChangesAsync();

        // Tạo payload hợp lệ và chữ ký đúng theo secret test.
        var payload = new
        {
            OrderId = orderId.ToString(),
            TransactionId = "TX-IDEMP-001",
            Amount = 50000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payload);
        var signature = ComputeSignature(rawPayload, WebhookSecret);

        var content1 = new StringContent(rawPayload, Encoding.UTF8, "application/json");
        var content2 = new StringContent(rawPayload, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("X-Webhook-Signature", signature);

        // Lần 1: xử lý thành công và cập nhật đơn nạp.
        var res1 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content1);
        if (!res1.IsSuccessStatusCode)
        {
            var err = await res1.Content.ReadAsStringAsync();
            throw new Exception($"res1 failed: {res1.StatusCode} - {err}");
        }

        // Lần 2: cùng dữ liệu, hệ thống phải idempotent và không cộng thêm lần nữa.
        var res2 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content2);
        if (!res2.IsSuccessStatusCode)
        {
            var err = await res2.Content.ReadAsStringAsync();
            throw new Exception($"res2 failed: {res2.StatusCode} - {err}");
        }

        // Đọc lại dữ liệu sau webhook để xác nhận trạng thái cuối cùng.
        using var scope2 = _factory.Services.CreateScope();
        var dbAfter = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var updatedOrder = await dbAfter.DepositOrders.FindAsync(orderId);
        var updatedUser = await dbAfter.Users.FindAsync(userId);

        Assert.NotNull(updatedOrder);
        Assert.Equal("Success", updatedOrder.Status);

        Assert.NotNull(updatedUser);
        // Kỳ vọng chỉ nhận bonus đúng một lần theo dữ liệu seed của đơn.
        Assert.Equal(5, updatedUser.DiamondBalance);
    }

    /// <summary>
    /// Tính chữ ký HMAC SHA256 cho payload webhook.
    /// Luồng helper dùng để tạo dữ liệu test giống cách gateway ký request thật.
    /// </summary>
    private static string ComputeSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
