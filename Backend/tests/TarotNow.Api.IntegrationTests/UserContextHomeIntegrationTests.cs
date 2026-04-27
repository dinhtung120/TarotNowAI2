using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using Xunit;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public class UserContextHomeIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private static readonly Guid DefaultUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UserContextHomeIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    [Fact]
    public async Task UserContextMetadata_ShouldReturnExpectedSnapshotShape()
    {
        await EnsureTestUserAsync(DefaultUserId);

        var response = await _client.GetAsync("/api/v1/user-context/metadata");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("wallet", out var wallet));
        Assert.Equal(JsonValueKind.Object, wallet.ValueKind);
        Assert.True(root.TryGetProperty("streak", out var streak));
        Assert.Equal(JsonValueKind.Object, streak.ValueKind);
        Assert.True(root.TryGetProperty("unreadNotificationCount", out var unreadNotificationCount));
        Assert.Equal(JsonValueKind.Number, unreadNotificationCount.ValueKind);
        Assert.True(root.TryGetProperty("unreadChatCount", out var unreadChatCount));
        Assert.Equal(JsonValueKind.Number, unreadChatCount.ValueKind);
        Assert.True(root.TryGetProperty("recentNotifications", out var recentNotifications));
        Assert.Equal(JsonValueKind.Object, recentNotifications.ValueKind);
        Assert.True(root.TryGetProperty("activeConversations", out var activeConversations));
        Assert.Equal(JsonValueKind.Object, activeConversations.ValueKind);
    }

    [Fact]
    public async Task HomeSnapshot_ShouldReturnFeaturedReadersPayload()
    {
        var response = await _client.GetAsync("/api/v1/home/snapshot");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("featuredReaders", out var featuredReaders));
        Assert.Equal(JsonValueKind.Array, featuredReaders.ValueKind);
        Assert.True(root.TryGetProperty("totalCount", out var totalCount));
        Assert.Equal(JsonValueKind.Number, totalCount.ValueKind);
    }

    private async Task EnsureTestUserAsync(Guid userId)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await db.Users.AnyAsync(x => x.Id == userId))
        {
            return;
        }

        var user = new User(
            email: "usercontext@test.local",
            username: "usercontext",
            passwordHash: "hash",
            displayName: "User Context",
            dateOfBirth: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            hasConsented: true);
        typeof(User).GetProperty("Id")?.SetValue(user, userId);
        user.Activate();
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }
}
