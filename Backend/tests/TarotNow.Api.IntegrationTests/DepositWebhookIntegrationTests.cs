using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS.Utils;
using Newtonsoft.Json.Linq;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử webhook PayOS: verify signature và idempotency credit ví.
public class DepositWebhookIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string ChecksumKey = "payos-test-checksum-key";
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo fixture webhook tests.
    /// </summary>
    public DepositWebhookIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Webhook chữ ký sai phải bị chặn.
    /// </summary>
    [Fact]
    public async Task PayOsWebhook_ShouldReject_WhenSignatureIsInvalid()
    {
        var client = _factory.CreateClient();
        var rawPayload = BuildWebhookPayload(
            orderCode: 920_001,
            amount: 50_000,
            paymentLinkId: "plink_920001",
            reference: "REF-INVALID",
            isSuccess: true,
            signatureOverride: "invalid_signature");

        var response = await client.PostAsync(
            "/api/v1/deposits/webhook/payos",
            new StringContent(rawPayload, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Webhook retry chỉ credit ví một lần.
    /// </summary>
    [Fact]
    public async Task PayOsWebhook_ShouldBeIdempotent_AndCreditWalletOnce()
    {
        var client = _factory.CreateClient();
        var orderCode = 920_002L;

        var user = await SeedUserAsync("webhook-idempotency");
        var order = await SeedDepositOrderAsync(
            userId: user.Id,
            orderCode: orderCode,
            amountVnd: 50_000,
            baseDiamond: 500,
            bonusGold: 25);

        var rawPayload = BuildWebhookPayload(
            orderCode: orderCode,
            amount: order.AmountVnd,
            paymentLinkId: order.PayOsPaymentLinkId,
            reference: "REF-IDEMP-001",
            isSuccess: true);

        var request1 = new StringContent(rawPayload, Encoding.UTF8, "application/json");
        var request2 = new StringContent(rawPayload, Encoding.UTF8, "application/json");

        var response1 = await client.PostAsync("/api/v1/deposits/webhook/payos", request1);
        var response2 = await client.PostAsync("/api/v1/deposits/webhook/payos", request2);

        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var updatedOrder = await db.DepositOrders.FindAsync(order.Id);
        var updatedUser = await db.Users.FindAsync(user.Id);

        Assert.NotNull(updatedOrder);
        Assert.NotNull(updatedUser);
        Assert.Equal(DepositOrderStatus.Success, updatedOrder!.Status);
        Assert.NotNull(updatedOrder.WalletGrantedAtUtc);
        Assert.Equal(500, updatedUser!.DiamondBalance);
        Assert.Equal(25, updatedUser.GoldBalance);
    }

    private async Task<User> SeedUserAsync(string suffix)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User(
            email: $"{suffix}@test.local",
            username: suffix,
            passwordHash: "hash",
            displayName: "Webhook Test User",
            dateOfBirth: new DateTime(1996, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        user.Activate();
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    private async Task<DepositOrder> SeedDepositOrderAsync(
        Guid userId,
        long orderCode,
        long amountVnd,
        long baseDiamond,
        long bonusGold)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var order = new DepositOrder(
            userId: userId,
            packageCode: "topup_50k",
            amountVnd: amountVnd,
            baseDiamondAmount: baseDiamond,
            bonusGoldAmount: bonusGold,
            clientRequestKey: $"test-client-{orderCode}",
            payOsOrderCode: orderCode,
            payOsPaymentLinkId: $"plink_{orderCode}",
            checkoutUrl: $"https://payos.test/checkout/{orderCode}",
            qrCode: $"PAYOS_QR_{orderCode}",
            expiresAtUtc: DateTime.UtcNow.AddMinutes(15));

        db.DepositOrders.Add(order);
        await db.SaveChangesAsync();
        return order;
    }

    private static string BuildWebhookPayload(
        long orderCode,
        long amount,
        string paymentLinkId,
        string reference,
        bool isSuccess,
        string? signatureOverride = null)
    {
        var code = isSuccess ? "00" : "01";
        var desc = isSuccess ? "success" : "failed";
        var data = new
        {
            orderCode,
            amount,
            description = $"TOPUP {orderCode}",
            accountNumber = "12345678",
            reference,
            transactionDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            currency = "VND",
            paymentLinkId,
            code,
            desc,
            counterAccountBankId = "9704",
            counterAccountBankName = "TEST BANK",
            counterAccountName = "TEST USER",
            counterAccountNumber = "0123456789",
            virtualAccountName = "TAROTNOW",
            virtualAccountNumber = "987654321"
        };

        var dataJson = JObject.FromObject(data);
        var signature = signatureOverride
                        ?? SignatureControl.CreateSignatureFromObj(dataJson, ChecksumKey);

        var webhook = new
        {
            code,
            desc,
            success = isSuccess,
            data,
            signature
        };

        return JsonSerializer.Serialize(webhook);
    }
}
