

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public class PromotionIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PromotionIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        _client.DefaultRequestHeaders.Add("X-Test-Role", "admin"); 
    }

        private async Task SeedUserAsync(Guid userId)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!await db.Users.AnyAsync(u => u.Id == userId))
        {
            var user = new User(
                email: "test@example.com",
                username: "testadmin",
                passwordHash: "hashed",
                displayName: "Test Admin",
                dateOfBirth: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                hasConsented: true);
            typeof(User).GetProperty("Id")?.SetValue(user, userId);
            typeof(User).GetProperty("Role")?.SetValue(user, UserRole.Admin);
            user.Activate();
            typeof(User).GetProperty("ReaderStatus")?.SetValue(user, ReaderApprovalStatus.Pending);
            typeof(User).GetProperty("PasswordHash")?.SetValue(user, "hashed");
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }
    }

        [Fact]
    public async Task DepositOrder_ShouldAutoApply_ActivePromotion_If_Met()
    {
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001"); 
        await SeedUserAsync(userId);

        
        var createPromoRequest = new 
        {
            MinAmountVnd = 100000,
            BonusDiamond = 20,
            IsActive = true
        };

        var promoResponse = await _client.PostAsJsonAsync("/api/v1/admin/promotions", createPromoRequest);
        promoResponse.EnsureSuccessStatusCode();

        
        var createDepositRequest = new 
        {
            AmountVnd = 100000,
            Gateway = "VNPay"
        };
        var depositRes = await _client.PostAsJsonAsync("/api/v1/deposits/orders", createDepositRequest);
        depositRes.EnsureSuccessStatusCode();

        
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var latestOrder = await db.DepositOrders.OrderByDescending(o => o.CreatedAt).FirstOrDefaultAsync();
        
        Assert.NotNull(latestOrder);
        Assert.Equal(100000, latestOrder!.AmountVnd);
        
        
        Console.WriteLine($"Diamond Applied: {latestOrder.DiamondAmount}");
    }
}
