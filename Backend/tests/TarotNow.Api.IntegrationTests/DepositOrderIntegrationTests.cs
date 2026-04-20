using System.Net;
using System.Net.Http.Json;
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

        var response1 = await client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);
        var response2 = await client.PostAsJsonAsync("/api/v1/deposits/orders", requestPayload);

        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        var order1 = await response1.Content.ReadFromJsonAsync<CreateOrderResponse>();
        var order2 = await response2.Content.ReadFromJsonAsync<CreateOrderResponse>();

        Assert.NotNull(order1);
        Assert.NotNull(order2);
        Assert.Equal(order1!.OrderId, order2!.OrderId);
        Assert.Equal(order1.PayOsOrderCode, order2.PayOsOrderCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var orders = await db.DepositOrders
            .Where(order => order.UserId == userId)
            .ToListAsync();

        Assert.Single(orders);
        Assert.Equal("topup_100k", orders[0].PackageCode);
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

    private sealed class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public long PayOsOrderCode { get; set; }
    }
}
