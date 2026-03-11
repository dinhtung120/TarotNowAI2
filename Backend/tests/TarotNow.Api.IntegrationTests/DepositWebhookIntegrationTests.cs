using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
public class DepositWebhookIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public DepositWebhookIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

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
        
        // Pass wrong signature (normally our mock accepts "test_bypass" or "TEST_BYPASS_SIG" for bypass)
        client.DefaultRequestHeaders.Add("X-Webhook-Signature", "wrong_signature");

        var response = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Webhook_ShouldApplyIdempotency_AndCreditOnce()
    {
        var client = _factory.CreateClient();

        // 1. Arrange DB
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var userId = Guid.NewGuid();
        var user = new TarotNow.Domain.Entities.User("testdep@test.com", "testdep", "hash", "DisplayName", DateTime.UtcNow, true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        db.Users.Add(user);
        
        var orderId = Guid.NewGuid();
        var depositOrder = new DepositOrder(userId, 50000, 5);
        typeof(DepositOrder).GetProperty("Id")?.SetValue(depositOrder, orderId);
        db.DepositOrders.Add(depositOrder); // Status Default is 'Pending'
        await db.SaveChangesAsync();

        var payload = new
        {
            OrderId = orderId.ToString(),
            TransactionId = "TX-IDEMP-001",
            Amount = 50000,
            Status = "SUCCESS"
        };
        var content1 = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var content2 = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        
        client.DefaultRequestHeaders.Add("X-Webhook-Signature", "test_bypass"); // Used by MockPaymentGatewayService

        // 2. Act
        // Send first time
        var res1 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content1);
        if (!res1.IsSuccessStatusCode) 
        {
            var err = await res1.Content.ReadAsStringAsync();
            throw new Exception($"res1 failed: {res1.StatusCode} - {err}");
        }

        // Send second time (simulating double webhook from gateway)
        var res2 = await client.PostAsync("/api/v1/deposits/webhook/vnpay", content2);
        if (!res2.IsSuccessStatusCode)
        {
            var err = await res2.Content.ReadAsStringAsync();
            throw new Exception($"res2 failed: {res2.StatusCode} - {err}");
        }

        // 3. Assert
        // Use a new scope to fetch fresh data from DB
        using var scope2 = _factory.Services.CreateScope();
        var dbAfter = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var updatedOrder = await dbAfter.DepositOrders.FindAsync(orderId);
        var updatedUser = await dbAfter.Users.FindAsync(userId);

        Assert.NotNull(updatedOrder);
        Assert.Equal("Success", updatedOrder.Status); // Should be marked Success
        
        Assert.NotNull(updatedUser);
        Assert.Equal(5, updatedUser.DiamondBalance); // Because starting was 0, credited 5. Should NOT be 10.
    }
}
