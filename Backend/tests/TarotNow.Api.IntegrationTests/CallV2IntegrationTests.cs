using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Services.CallV2;

namespace TarotNow.Api.IntegrationTests;

[Collection("IntegrationTests")]
public sealed class CallV2IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private static readonly Guid CallerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid CalleeId = Guid.Parse("00000000-0000-0000-0000-000000000002");

    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _callerClient;

    public CallV2IntegrationTests(CustomWebApplicationFactory<Program> baseFactory)
    {
        _factory = baseFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["LiveKit:Url"] = "wss://livekit.test",
                    ["LiveKit:ApiKey"] = "lk_test_key",
                    ["LiveKit:ApiSecret"] = "lk_test_secret_128_bits_minimum_value",
                    ["LiveKit:TokenTtlMinutes"] = "30",
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ILiveKitRoomGateway>();
                services.AddSingleton<FakeLiveKitRoomGateway>();
                services.AddSingleton<ILiveKitRoomGateway>(sp => sp.GetRequiredService<FakeLiveKitRoomGateway>());
            });
        });

        _callerClient = CreateAuthenticatedClient(CallerId);
    }

    [Fact]
    public async Task FullFlow_StartAcceptTokenEnd_ShouldSucceed_AndEndIsIdempotent()
    {
        var conversationId = await SeedConversationAsync(CallerId, CalleeId);
        var calleeClient = CreateAuthenticatedClient(CalleeId);

        var startResponse = await _callerClient.PostAsJsonAsync("/api/v1/calls/start", new
        {
            conversationId,
            type = "audio"
        });
        startResponse.EnsureSuccessStatusCode();

        var startTicket = await startResponse.Content.ReadFromJsonAsync<CallJoinTicketContract>();
        Assert.NotNull(startTicket);
        Assert.Equal(CallSessionV2Statuses.Requested, startTicket!.Session.Status);
        Assert.False(string.IsNullOrWhiteSpace(startTicket.AccessToken));
        Assert.Equal("wss://livekit.test", startTicket.LiveKitUrl);

        var acceptResponse = await calleeClient.PostAsync($"/api/v1/calls/{startTicket.Session.Id}/accept", content: null);
        acceptResponse.EnsureSuccessStatusCode();

        var acceptTicket = await acceptResponse.Content.ReadFromJsonAsync<CallJoinTicketContract>();
        Assert.NotNull(acceptTicket);
        Assert.Equal(CallSessionV2Statuses.Accepted, acceptTicket!.Session.Status);

        var tokenResponse = await _callerClient.PostAsync($"/api/v1/calls/{startTicket.Session.Id}/token", content: null);
        tokenResponse.EnsureSuccessStatusCode();

        var tokenTicket = await tokenResponse.Content.ReadFromJsonAsync<CallJoinTicketContract>();
        Assert.NotNull(tokenTicket);
        Assert.Equal(startTicket.Session.Id, tokenTicket!.Session.Id);

        var firstEndResponse = await _callerClient.PostAsJsonAsync($"/api/v1/calls/{startTicket.Session.Id}/end", new
        {
            reason = "normal"
        });
        firstEndResponse.EnsureSuccessStatusCode();

        var firstEnded = await firstEndResponse.Content.ReadFromJsonAsync<CallSessionContract>();
        Assert.NotNull(firstEnded);
        Assert.Equal(CallSessionV2Statuses.Ended, firstEnded!.Status);

        var secondEndResponse = await _callerClient.PostAsJsonAsync($"/api/v1/calls/{startTicket.Session.Id}/end", new
        {
            reason = "normal"
        });
        secondEndResponse.EnsureSuccessStatusCode();

        var secondEnded = await secondEndResponse.Content.ReadFromJsonAsync<CallSessionContract>();
        Assert.NotNull(secondEnded);
        Assert.Equal(CallSessionV2Statuses.Ended, secondEnded!.Status);
    }

    [Fact]
    public async Task Start_ShouldRejectWhenConversationAlreadyHasActiveSession()
    {
        var conversationId = await SeedConversationAsync(CallerId, CalleeId);

        var firstStart = await _callerClient.PostAsJsonAsync("/api/v1/calls/start", new
        {
            conversationId,
            type = "audio"
        });
        firstStart.EnsureSuccessStatusCode();

        var secondStart = await _callerClient.PostAsJsonAsync("/api/v1/calls/start", new
        {
            conversationId,
            type = "video"
        });

        Assert.Equal(HttpStatusCode.UnprocessableEntity, secondStart.StatusCode);
        var (errorCode, _) = await ReadProblemAsync(secondStart);
        Assert.Equal(CallV2ErrorCodes.CallAlreadyActive, errorCode);
    }

    private HttpClient CreateAuthenticatedClient(Guid userId)
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        client.DefaultRequestHeaders.Remove("X-Test-UserId");
        client.DefaultRequestHeaders.Add("X-Test-UserId", userId.ToString());
        return client;
    }

    private async Task<string> SeedConversationAsync(Guid callerId, Guid calleeId)
    {
        using var scope = _factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();

        var conversation = new ConversationDto
        {
            UserId = callerId.ToString(),
            ReaderId = calleeId.ToString(),
            Status = "ongoing",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            SlaHours = 12
        };

        await repository.AddAsync(conversation);
        return conversation.Id;
    }

    private static async Task<(string? ErrorCode, string? Detail)> ReadProblemAsync(HttpResponseMessage response)
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        string? errorCode = null;
        string? detail = null;

        if (root.TryGetProperty("errorCode", out var topLevelCode))
        {
            errorCode = topLevelCode.GetString();
        }

        if (root.TryGetProperty("extensions", out var ext)
            && ext.TryGetProperty("errorCode", out var code))
        {
            errorCode = code.GetString();
        }

        if (root.TryGetProperty("detail", out var detailProp))
        {
            detail = detailProp.GetString();
        }

        return (errorCode, detail);
    }

    private sealed class CallJoinTicketContract
    {
        public CallSessionContract Session { get; set; } = new();
        public string LiveKitUrl { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
    }

    private sealed class CallSessionContract
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
