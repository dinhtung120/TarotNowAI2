using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
// Kiểm thử áp dụng promotion Gold khi tạo lệnh nạp.
public class PromotionIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    /// <summary>
    /// Khởi tạo test class với client mang role admin.
    /// </summary>
    public PromotionIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        _client.DefaultRequestHeaders.Add("X-Test-Role", "admin");
    }

    /// <summary>
    /// Tạo order nạp phải chụp đúng bonus gold từ promotion đang active.
    /// </summary>
    [Fact]
    public async Task DepositOrder_ShouldApply_ActivePromotionBonusGold()
    {
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        await SeedUserAsync(userId);

        var createPromotionRequest = new
        {
            MinAmountVnd = 100_000,
            BonusGold = 50,
            IsActive = true
        };
        var promotionResponse = await _client.PostAsJsonAsync("/api/v1/admin/promotions", createPromotionRequest);
        promotionResponse.EnsureSuccessStatusCode();

        var createOrderRequest = new
        {
            PackageCode = "topup_100k",
            IdempotencyKey = $"promo-test-{Guid.NewGuid():N}"
        };
        var orderResponse = await _client.PostAsJsonAsync("/api/v1/deposits/orders", createOrderRequest);
        orderResponse.EnsureSuccessStatusCode();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var latestOrder = await db.DepositOrders.OrderByDescending(order => order.CreatedAt).FirstOrDefaultAsync();

        Assert.NotNull(latestOrder);
        Assert.Equal(100_000, latestOrder!.AmountVnd);
        Assert.Equal(1_000, latestOrder.BaseDiamondAmount);
        Assert.Equal(50, latestOrder.BonusGoldAmount);
        Assert.Equal(1_000, latestOrder.DiamondAmount);
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
            email: "test@example.com",
            username: "testadmin",
            passwordHash: "hashed",
            displayName: "Test Admin",
            dateOfBirth: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        typeof(User).GetProperty("Role")?.SetValue(user, UserRole.Admin);
        typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
        user.Activate();

        db.Users.Add(user);
        await db.SaveChangesAsync();
    }
}
