/*
 * FILE: DepositWebhookIntegrationTests.cs
 * MỤC ĐÍCH: Integration test kiểm tra webhook nạp tiền (deposit) end-to-end.
 *
 *   CÁC TEST CASE:
 *   1. Webhook_ShouldReject_WhenSignatureIsInvalid:
 *      → Gửi webhook với chữ ký SAI → 401 Unauthorized (chặn giả mạo)
 *   2. Webhook_ShouldApplyIdempotency_AndCreditOnce:
 *      → Gửi CÙNG webhook 2 lần (simulate double-fire từ cổng thanh toán)
 *      → Credit chỉ 1 lần (Diamond = 5, KHÔNG phải 10)
 *
 *   LUỒNG TEST IDEMPOTENCY:
 *   → Seed: User (balance=0) + DepositOrder (50000 VNĐ → 5 Diamond)
 *   → Gửi webhook lần 1: signature đúng → Credit 5 Diamond → balance = 5
 *   → Gửi webhook lần 2: CÙNG payload + signature → idempotency chặn → balance vẫn = 5
 *   → Assert: order.Status = "Success", user.DiamondBalance = 5 (KHÔNG phải 10)
 *
 *   BẢO MẬT:
 *   → HMAC-SHA256 signature verify trước khi xử lý webhook
 *   → Dùng test secret key: "TarotNow_Test_WebhookSecret_2026"
 */

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

/// <summary>
/// Test webhook nạp tiền: HMAC verify + idempotency (chống double-fire).
/// </summary>
[Collection("Testcontainers")]
public class DepositWebhookIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string WebhookSecret = "TarotNow_Test_WebhookSecret_2026";
    private readonly CustomWebApplicationFactory<Program> _factory;

    public DepositWebhookIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Chữ ký SAI → 401 Unauthorized.
    /// Đảm bảo webhook không thể bị giả mạo bằng payload tùy ý.
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
        
        // Chữ ký SAI → phải bị reject
        client.DefaultRequestHeaders.Add("X-Webhook-Signature", "wrong_signature");

        var response = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// IDEMPOTENCY: gửi cùng webhook 2 lần → Credit chỉ 1 lần.
    /// Simulate: cổng thanh toán fire webhook 2 lần (retry hoặc bug).
    /// Kết quả: Diamond = 5 (KHÔNG phải 10).
    /// </summary>
    [Fact]
    public async Task Webhook_ShouldApplyIdempotency_AndCreditOnce()
    {
        var client = _factory.CreateClient();

        // === SEED: User + DepositOrder ===
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var userId = Guid.NewGuid();
        var user = new TarotNow.Domain.Entities.User("testdep@test.com", "testdep", "hash", "DisplayName", DateTime.UtcNow, true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        db.Users.Add(user);
        
        var orderId = Guid.NewGuid();
        var depositOrder = new DepositOrder(userId, 50000, 5); // 50k VNĐ → 5 Diamond
        typeof(DepositOrder).GetProperty("Id")?.SetValue(depositOrder, orderId);
        db.DepositOrders.Add(depositOrder);
        await db.SaveChangesAsync();

        // Tạo payload + chữ ký HMAC hợp lệ
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

        // === ACT: gửi webhook 2 lần (simulate double-fire) ===
        var res1 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content1);
        if (!res1.IsSuccessStatusCode) 
        {
            var err = await res1.Content.ReadAsStringAsync();
            throw new Exception($"res1 failed: {res1.StatusCode} - {err}");
        }

        // Lần 2: cùng payload → idempotency phải chặn
        var res2 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content2);
        if (!res2.IsSuccessStatusCode)
        {
            var err = await res2.Content.ReadAsStringAsync();
            throw new Exception($"res2 failed: {res2.StatusCode} - {err}");
        }

        // === ASSERT: verify DB state ===
        using var scope2 = _factory.Services.CreateScope();
        var dbAfter = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var updatedOrder = await dbAfter.DepositOrders.FindAsync(orderId);
        var updatedUser = await dbAfter.Users.FindAsync(userId);

        Assert.NotNull(updatedOrder);
        Assert.Equal("Success", updatedOrder.Status); // Order marked success

        Assert.NotNull(updatedUser);
        // Diamond = 5 (KHÔNG phải 10) → idempotency hoạt động
        Assert.Equal(5, updatedUser.DiamondBalance);
    }

    /// <summary>Helper: tính HMAC-SHA256 signature từ payload + secret.</summary>
    private static string ComputeSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
