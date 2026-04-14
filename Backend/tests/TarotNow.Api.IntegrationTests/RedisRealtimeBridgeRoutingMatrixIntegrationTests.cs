using System.Collections.Concurrent;
using System.Text.Json;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TarotNow.Api.Realtime;
using TarotNow.Api.IntegrationTests.Realtime;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.BackgroundJobs.Outbox;
using TarotNow.Infrastructure.Messaging.Redis;

namespace TarotNow.Api.IntegrationTests;

[Collection("Testcontainers")]
// Kiểm thử routing matrix Redis bridge -> SignalR theo contract FE hiện tại.
public sealed class RedisRealtimeBridgeRoutingMatrixIntegrationTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private static readonly TimeSpan PositiveTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan NegativeTimeout = TimeSpan.FromMilliseconds(500);

    private readonly InMemoryRealtimeBridgeSource _bridgeSource = new();
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Khởi tạo test class bridge routing matrix.
    /// </summary>
    public RedisRealtimeBridgeRoutingMatrixIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IRealtimeBridgeSource>();
                services.AddSingleton<IRealtimeBridgeSource>(_bridgeSource);
                services.RemoveAll<IRedisPublisher>();
                services.AddSingleton<IRedisPublisher, NoOpRedisPublisher>();
                RemoveHostedService<OutboxProcessorWorker>(services);
            });
        });
    }

    /// <summary>
    /// Xác nhận routing matrix đầy đủ: notifications/wallet/chat/gacha/gamification + invalid payload.
    /// </summary>
    [Fact]
    public async Task Bridge_ShouldRouteRealtimeEvents_ToExpectedSignalRGroups()
    {
        _ = _factory.CreateClient();
        var subscribed = await _bridgeSource.WaitForSubscriptionCountAsync(5, TimeSpan.FromSeconds(5));
        Assert.True(subscribed);

        const string userId = "11111111-1111-1111-1111-111111111111";
        const string readerId = "22222222-2222-2222-2222-222222222222";
        const string otherUserId = "33333333-3333-3333-3333-333333333333";
        var conversationId = ObjectId.GenerateNewId().ToString();

        await EnsureConversationAsync(conversationId, userId, readerId);

        await using var presenceUser = CreateHubConnection("/api/v1/presence", userId);
        await using var presenceReader = CreateHubConnection("/api/v1/presence", readerId);
        await using var presenceOther = CreateHubConnection("/api/v1/presence", otherUserId);
        await using var chatUser = CreateHubConnection("/api/v1/chat", userId);
        await using var chatReader = CreateHubConnection("/api/v1/chat", readerId);
        await using var chatOther = CreateHubConnection("/api/v1/chat", otherUserId);

        using var presenceUserProbe = new HubEventProbe(
            presenceUser,
            "notification.new",
            "wallet.balance_changed",
            "conversation.updated",
            "gacha.result",
            "gamification.quest_completed",
            "gamification.achievement_unlocked",
            "gamification.card_level_up");
        using var presenceReaderProbe = new HubEventProbe(
            presenceReader,
            "notification.new",
            "wallet.balance_changed",
            "conversation.updated",
            "gacha.result",
            "gamification.quest_completed",
            "gamification.achievement_unlocked",
            "gamification.card_level_up");
        using var presenceOtherProbe = new HubEventProbe(
            presenceOther,
            "notification.new",
            "wallet.balance_changed",
            "conversation.updated",
            "gacha.result",
            "gamification.quest_completed",
            "gamification.achievement_unlocked",
            "gamification.card_level_up");
        using var chatUserProbe = new HubEventProbe(chatUser, "message.created", "conversation.updated");
        using var chatReaderProbe = new HubEventProbe(chatReader, "message.created", "conversation.updated");
        using var chatOtherProbe = new HubEventProbe(chatOther, "message.created", "conversation.updated");

        await StartConnectionsAsync(presenceUser, presenceReader, presenceOther, chatUser, chatReader, chatOther);
        await JoinConversationWithRetryAsync(chatUser, conversationId);
        await JoinConversationWithRetryAsync(chatReader, conversationId);
        await Task.Delay(150);
        ClearAllProbes(presenceUserProbe, presenceReaderProbe, presenceOtherProbe, chatUserProbe, chatReaderProbe, chatOtherProbe);

        await AssertNotificationRoutingAsync(userId, presenceUserProbe, presenceReaderProbe, presenceOtherProbe);
        await AssertWalletRoutingAsync(userId, presenceUserProbe, presenceReaderProbe, presenceOtherProbe);
        await AssertChatMessageCreatedRoutingAsync(conversationId, chatUserProbe, chatReaderProbe, chatOtherProbe);
        await AssertConversationUpdatedRoutingAsync(userId, readerId, conversationId, chatUserProbe, chatReaderProbe, chatOtherProbe, presenceUserProbe, presenceReaderProbe, presenceOtherProbe);
        await AssertUnreadChangedRoutingAsync(userId, readerId, conversationId, chatUserProbe, chatReaderProbe, chatOtherProbe, presenceUserProbe, presenceReaderProbe, presenceOtherProbe);
        await AssertGamificationRoutingAsync(userId, presenceUserProbe, presenceReaderProbe, presenceOtherProbe);
        await AssertInvalidPayloadDroppedAsync(userId, readerId, presenceUserProbe, presenceReaderProbe, presenceOtherProbe, chatUserProbe, chatReaderProbe, chatOtherProbe);
    }

    private async Task AssertNotificationRoutingAsync(
        string userId,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Notifications, "notification.new", new
        {
            userId,
            notificationId = "notif-001"
        });

        var payload = await presenceUserProbe.WaitForAsync("notification.new", PositiveTimeout);
        Assert.Equal(userId, payload.GetProperty("userId").GetString());

        await presenceReaderProbe.AssertNoEventAsync("notification.new", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("notification.new", NegativeTimeout);
    }

    private async Task AssertWalletRoutingAsync(
        string userId,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Wallet, "wallet.balance_changed", new
        {
            userId,
            goldBalance = 100
        });

        var payload = await presenceUserProbe.WaitForAsync("wallet.balance_changed", PositiveTimeout);
        Assert.Equal(userId, payload.GetProperty("userId").GetString());

        await presenceReaderProbe.AssertNoEventAsync("wallet.balance_changed", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("wallet.balance_changed", NegativeTimeout);
    }

    private async Task AssertChatMessageCreatedRoutingAsync(
        string conversationId,
        HubEventProbe chatUserProbe,
        HubEventProbe chatReaderProbe,
        HubEventProbe chatOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Chat, "message.created", new
        {
            conversationId,
            message = new
            {
                id = "msg-001",
                content = "hello"
            }
        });

        var userPayload = await chatUserProbe.WaitForAsync("message.created", PositiveTimeout);
        var readerPayload = await chatReaderProbe.WaitForAsync("message.created", PositiveTimeout);
        Assert.Equal("msg-001", userPayload.GetProperty("id").GetString());
        Assert.Equal("msg-001", readerPayload.GetProperty("id").GetString());
        await chatOtherProbe.AssertNoEventAsync("message.created", NegativeTimeout);
    }

    private async Task AssertConversationUpdatedRoutingAsync(
        string userId,
        string readerId,
        string conversationId,
        HubEventProbe chatUserProbe,
        HubEventProbe chatReaderProbe,
        HubEventProbe chatOtherProbe,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Chat, "conversation.updated", new
        {
            userId,
            readerId,
            conversationId,
            type = "status_changed"
        });

        var chatUserPayload = await chatUserProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var chatReaderPayload = await chatReaderProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var presenceUserPayload = await presenceUserProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var presenceReaderPayload = await presenceReaderProbe.WaitForAsync("conversation.updated", PositiveTimeout);

        Assert.Equal(conversationId, chatUserPayload.GetProperty("conversationId").GetString());
        Assert.Equal(conversationId, chatReaderPayload.GetProperty("conversationId").GetString());
        Assert.Equal(conversationId, presenceUserPayload.GetProperty("conversationId").GetString());
        Assert.Equal(conversationId, presenceReaderPayload.GetProperty("conversationId").GetString());

        await chatOtherProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
    }

    private async Task AssertUnreadChangedRoutingAsync(
        string userId,
        string readerId,
        string conversationId,
        HubEventProbe chatUserProbe,
        HubEventProbe chatReaderProbe,
        HubEventProbe chatOtherProbe,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Chat, "chat.unread_changed", new
        {
            userId,
            readerId,
            conversationId,
            unreadCount = 4
        });

        var chatUserPayload = await chatUserProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var chatReaderPayload = await chatReaderProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var presenceUserPayload = await presenceUserProbe.WaitForAsync("conversation.updated", PositiveTimeout);
        var presenceReaderPayload = await presenceReaderProbe.WaitForAsync("conversation.updated", PositiveTimeout);

        Assert.Equal("unread_changed", chatUserPayload.GetProperty("type").GetString());
        Assert.Equal("unread_changed", chatReaderPayload.GetProperty("type").GetString());
        Assert.Equal("unread_changed", presenceUserPayload.GetProperty("type").GetString());
        Assert.Equal("unread_changed", presenceReaderPayload.GetProperty("type").GetString());

        await chatOtherProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
    }

    private async Task AssertGamificationRoutingAsync(
        string userId,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe)
    {
        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Gacha, "gacha.result", new
        {
            userId,
            cardId = "card-001"
        });

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Gamification, "gamification.quest_completed", new
        {
            userId,
            questCode = "daily_checkin"
        });

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Gamification, "gamification.achievement_unlocked", new
        {
            userId,
            achievementCode = "first_reading"
        });

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Gamification, "gamification.card_level_up", new
        {
            userId,
            cardId = "card-001"
        });

        var gachaPayload = await presenceUserProbe.WaitForAsync("gacha.result", PositiveTimeout);
        var questPayload = await presenceUserProbe.WaitForAsync("gamification.quest_completed", PositiveTimeout);
        var achievementPayload = await presenceUserProbe.WaitForAsync("gamification.achievement_unlocked", PositiveTimeout);
        var cardLevelPayload = await presenceUserProbe.WaitForAsync("gamification.card_level_up", PositiveTimeout);
        Assert.Equal(userId, gachaPayload.GetProperty("userId").GetString());
        Assert.Equal(userId, questPayload.GetProperty("userId").GetString());
        Assert.Equal(userId, achievementPayload.GetProperty("userId").GetString());
        Assert.Equal(userId, cardLevelPayload.GetProperty("userId").GetString());

        await presenceReaderProbe.AssertNoEventAsync("gacha.result", NegativeTimeout);
        await presenceReaderProbe.AssertNoEventAsync("gamification.quest_completed", NegativeTimeout);
        await presenceReaderProbe.AssertNoEventAsync("gamification.achievement_unlocked", NegativeTimeout);
        await presenceReaderProbe.AssertNoEventAsync("gamification.card_level_up", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("gacha.result", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("gamification.quest_completed", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("gamification.achievement_unlocked", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("gamification.card_level_up", NegativeTimeout);
    }

    private async Task AssertInvalidPayloadDroppedAsync(
        string userId,
        string readerId,
        HubEventProbe presenceUserProbe,
        HubEventProbe presenceReaderProbe,
        HubEventProbe presenceOtherProbe,
        HubEventProbe chatUserProbe,
        HubEventProbe chatReaderProbe,
        HubEventProbe chatOtherProbe)
    {
        ClearAllProbes(presenceUserProbe, presenceReaderProbe, presenceOtherProbe, chatUserProbe, chatReaderProbe, chatOtherProbe);

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Notifications, "notification.new", new
        {
            notificationId = "missing-user"
        });

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Chat, "message.created", new
        {
            message = new
            {
                id = "missing-conversation"
            }
        });

        await _bridgeSource.PublishEnvelopeAsync(RealtimeChannelNames.Chat, "conversation.updated", new
        {
            userId,
            readerId
        });

        await presenceUserProbe.AssertNoEventAsync("notification.new", NegativeTimeout);
        await presenceReaderProbe.AssertNoEventAsync("notification.new", NegativeTimeout);
        await presenceOtherProbe.AssertNoEventAsync("notification.new", NegativeTimeout);
        await chatUserProbe.AssertNoEventAsync("message.created", NegativeTimeout);
        await chatReaderProbe.AssertNoEventAsync("message.created", NegativeTimeout);
        await chatOtherProbe.AssertNoEventAsync("message.created", NegativeTimeout);
        await chatUserProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
        await chatReaderProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
        await chatOtherProbe.AssertNoEventAsync("conversation.updated", NegativeTimeout);
    }

    private HubConnection CreateHubConnection(string hubPath, string userId)
    {
        return new HubConnectionBuilder()
            .WithUrl(
                new Uri($"http://localhost{hubPath}"),
                options =>
                {
                    options.Transports = HttpTransportType.LongPolling;
                    options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
                    options.Headers["Authorization"] = TestAuthHandler.AuthenticationScheme;
                    options.Headers["X-Test-Role"] = "User";
                    options.Headers["X-Test-UserId"] = userId;
                })
            .WithAutomaticReconnect()
            .Build();
    }

    private async Task EnsureConversationAsync(string conversationId, string userId, string readerId)
    {
        using var scope = _factory.Services.CreateScope();
        var conversationRepository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();
        var existing = await conversationRepository.GetByIdAsync(conversationId, CancellationToken.None);
        if (existing is not null)
        {
            return;
        }

        await conversationRepository.AddAsync(new ConversationDto
        {
            Id = conversationId,
            UserId = userId,
            ReaderId = readerId,
            Status = ConversationStatus.Ongoing,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }, CancellationToken.None);
    }

    private static async Task StartConnectionsAsync(params HubConnection[] connections)
    {
        foreach (var connection in connections)
        {
            await EnsureConnectedAsync(connection);
        }
    }

    private static async Task JoinConversationWithRetryAsync(HubConnection connection, string conversationId)
    {
        const int maxAttempts = 4;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            await EnsureConnectedAsync(connection);

            try
            {
                await connection.InvokeAsync("JoinConversation", conversationId);
                return;
            }
            catch (InvalidOperationException ex) when (
                attempt < maxAttempts
                && ex.Message.Contains("not active", StringComparison.OrdinalIgnoreCase))
            {
                await RestartConnectionAsync(connection);
                await Task.Delay(100 * attempt);
            }
        }

        throw new InvalidOperationException($"Unable to join conversation '{conversationId}' after retries.");
    }

    private static async Task EnsureConnectedAsync(HubConnection connection)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            return;
        }

        if (connection.State == HubConnectionState.Disconnected)
        {
            await connection.StartAsync();
            return;
        }

        var deadline = DateTime.UtcNow.AddSeconds(3);
        while (DateTime.UtcNow <= deadline)
        {
            if (connection.State == HubConnectionState.Connected)
            {
                return;
            }

            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
                return;
            }

            await Task.Delay(50);
        }

        if (connection.State != HubConnectionState.Connected)
        {
            await RestartConnectionAsync(connection);
        }
    }

    private static async Task RestartConnectionAsync(HubConnection connection)
    {
        if (connection.State != HubConnectionState.Disconnected)
        {
            await connection.StopAsync();
        }

        await connection.StartAsync();
    }

    private static void ClearAllProbes(params HubEventProbe[] probes)
    {
        foreach (var probe in probes)
        {
            probe.ClearAll();
        }
    }

    private static void RemoveHostedService<THostedService>(IServiceCollection services)
        where THostedService : class, IHostedService
    {
        var descriptors = services
            .Where(descriptor =>
                descriptor.ServiceType == typeof(IHostedService)
                && descriptor.ImplementationType == typeof(THostedService))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }

    private sealed class HubEventProbe : IDisposable
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<JsonElement>> _events =
            new(StringComparer.Ordinal);
        private readonly List<IDisposable> _subscriptions = [];

        public HubEventProbe(HubConnection connection, params string[] eventNames)
        {
            foreach (var eventName in eventNames)
            {
                var queue = _events.GetOrAdd(eventName, _ => new ConcurrentQueue<JsonElement>());
                var subscription = connection.On<JsonElement>(eventName, payload =>
                {
                    queue.Enqueue(payload);
                    return Task.CompletedTask;
                });

                _subscriptions.Add(subscription);
            }
        }

        public async Task<JsonElement> WaitForAsync(string eventName, TimeSpan timeout)
        {
            var start = DateTime.UtcNow;
            while (DateTime.UtcNow - start < timeout)
            {
                if (TryDequeue(eventName, out var payload))
                {
                    return payload;
                }

                await Task.Delay(25);
            }

            throw new TimeoutException($"Timed out waiting for SignalR event '{eventName}'.");
        }

        public async Task AssertNoEventAsync(string eventName, TimeSpan timeout)
        {
            var start = DateTime.UtcNow;
            while (DateTime.UtcNow - start < timeout)
            {
                if (TryDequeue(eventName, out _))
                {
                    throw new Xunit.Sdk.XunitException($"Unexpected SignalR event '{eventName}' was received.");
                }

                await Task.Delay(25);
            }
        }

        public void ClearAll()
        {
            foreach (var queue in _events.Values)
            {
                while (queue.TryDequeue(out _))
                {
                }
            }
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
        }

        private bool TryDequeue(string eventName, out JsonElement payload)
        {
            payload = default;
            if (_events.TryGetValue(eventName, out var queue) == false)
            {
                return false;
            }

            return queue.TryDequeue(out payload);
        }
    }
}
