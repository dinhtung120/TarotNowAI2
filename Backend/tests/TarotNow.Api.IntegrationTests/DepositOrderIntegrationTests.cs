using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử luồng tạo order nạp theo package và idempotency key.
public class DepositOrderIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo fixture test tạo order.
    /// </summary>
    public DepositOrderIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tạo order cùng idempotency key phải trả cùng order.
    /// </summary>
    [Fact]
    public async Task CreateOrder_ShouldBeIdempotent_ForSameUserAndKey()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        await SeedUserAsync(userId);

        var requestPayload = new
        {
            packageCode = "topup_100k",
            idempotencyKey = "create-order-idem-001"
        };
        var clientRequestKey = BuildClientRequestKey(userId, requestPayload.idempotencyKey);

        var response1 = await client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);
        var response2 = await client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);

        await AssertStatusCodeAsync(response1, HttpStatusCode.OK);
        await AssertStatusCodeAsync(response2, HttpStatusCode.OK);

        var order1 = await response1.Content.ReadFromJsonAsync<CreateOrderResponse>();
        var order2 = await response2.Content.ReadFromJsonAsync<CreateOrderResponse>();

        Assert.NotNull(order1);
        Assert.NotNull(order2);
        Assert.Equal(order1!.OrderId, order2!.OrderId);
        Assert.Equal(order1.PayOsOrderCode, order2.PayOsOrderCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var orders = await db.DepositOrders
            .Where(order => order.ClientRequestKey == clientRequestKey)
            .ToListAsync();

        Assert.Single(orders);
        Assert.Equal("topup_100k", orders[0].PackageCode);
    }

    /// <summary>
    /// Tạo order đồng thời cùng idempotency key phải luôn hội tụ về một order duy nhất.
    /// </summary>
    [Fact]
    public async Task CreateOrder_ShouldBeIdempotent_UnderConcurrentRequests()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        await SeedUserAsync(userId);

        var requestPayload = new
        {
            packageCode = "topup_50k",
            idempotencyKey = "create-order-concurrent-idem-001"
        };
        var clientRequestKey = BuildClientRequestKey(userId, requestPayload.idempotencyKey);

        var request1 = client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);
        var request2 = client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);
        var responses = await Task.WhenAll(request1, request2);
        var response1 = responses[0];
        var response2 = responses[1];

        await AssertStatusCodeAsync(response1, HttpStatusCode.OK);
        await AssertStatusCodeAsync(response2, HttpStatusCode.OK);

        var order1 = await response1.Content.ReadFromJsonAsync<CreateOrderResponse>();
        var order2 = await response2.Content.ReadFromJsonAsync<CreateOrderResponse>();
        Assert.NotNull(order1);
        Assert.NotNull(order2);
        Assert.Equal(order1!.OrderId, order2!.OrderId);
        Assert.Equal(order1.PayOsOrderCode, order2.PayOsOrderCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var orderCount = await db.DepositOrders
            .CountAsync(order => order.ClientRequestKey == clientRequestKey);

        Assert.Equal(1, orderCount);
    }

    private static async Task AssertStatusCodeAsync(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
    {
        if (response.StatusCode == expectedStatusCode)
        {
            return;
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        throw new Xunit.Sdk.XunitException(
            $"Expected status {(int)expectedStatusCode} but was {(int)response.StatusCode}. Body: {responseBody}");
    }

    private async Task SeedUserAsync(Guid userId)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await db.Users.AnyAsync(user => user.Id == userId))
        {
            return;
        }

        var user = new User(
            email: "deposit-order@test.local",
            username: "deposit-order-user",
            passwordHash: "hash",
            displayName: "Deposit User",
            dateOfBirth: new DateTime(1997, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        user.Activate();

        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    private static string BuildClientRequestKey(Guid userId, string idempotencyKey)
    {
        var source = $"{userId:N}:{idempotencyKey.Trim()}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        var hashPrefix = Convert.ToHexString(hash.AsSpan(0, 16)).ToLowerInvariant();
        return $"deposit_{userId:N}_{hashPrefix}";
    }

    private sealed class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public long PayOsOrderCode { get; set; }
    }
}
