using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Bridge realtime event từ Redis Pub/Sub sang SignalR hubs để giữ nguyên contract FE hiện tại.
/// </summary>
public sealed partial class RedisRealtimeSignalRBridgeService : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private static readonly string[] Channels =
    {
        RealtimeChannelNames.Notifications,
        RealtimeChannelNames.Wallet,
        RealtimeChannelNames.Chat,
        RealtimeChannelNames.ChatFast,
        RealtimeChannelNames.Gacha,
        RealtimeChannelNames.Gamification,
        RealtimeChannelNames.UserState
    };

    private readonly IRealtimeBridgeSource _realtimeBridgeSource;
    private readonly IHubContext<PresenceHub> _presenceHubContext;
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly ILogger<RedisRealtimeSignalRBridgeService> _logger;
    private readonly ConcurrentDictionary<string, DateTime> _bridgeDedupByEventId = new(StringComparer.Ordinal);

    /// <summary>
    /// Khởi tạo bridge service Redis sang SignalR.
    /// </summary>
    public RedisRealtimeSignalRBridgeService(
        IHubContext<PresenceHub> presenceHubContext,
        IHubContext<ChatHub> chatHubContext,
        ILogger<RedisRealtimeSignalRBridgeService> logger,
        IRealtimeBridgeSource realtimeBridgeSource)
    {
        _presenceHubContext = presenceHubContext;
        _chatHubContext = chatHubContext;
        _logger = logger;
        _realtimeBridgeSource = realtimeBridgeSource;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_realtimeBridgeSource.IsEnabled == false)
        {
            _logger.LogWarning("Redis realtime bridge disabled because Redis connection is unavailable.");
            return;
        }

        foreach (var channel in Channels)
        {
            await _realtimeBridgeSource.SubscribeAsync(channel, ProcessRealtimeMessageAsync, stoppingToken);
        }

        _logger.LogInformation("Redis realtime bridge subscribed to channels: {Channels}", string.Join(", ", Channels));

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            foreach (var channel in Channels)
            {
                await _realtimeBridgeSource.UnsubscribeAsync(channel, stoppingToken);
            }
        }
    }

    private async Task ProcessRealtimeMessageAsync(string channel, string payloadJson)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(payloadJson))
            {
                return;
            }

            var envelope = JsonSerializer.Deserialize<RedisRealtimeEnvelope>(payloadJson, JsonOptions);
            if (envelope == null || string.IsNullOrWhiteSpace(envelope.EventName))
            {
                return;
            }

            if (channel == RealtimeChannelNames.Notifications || channel == RealtimeChannelNames.Wallet)
            {
                await ForwardPresenceEventAsync(envelope.EventName, envelope.Payload);
                return;
            }

            if (channel == RealtimeChannelNames.Chat)
            {
                await ForwardChatEventAsync(envelope.EventName, envelope.Payload);
                return;
            }

            if (channel == RealtimeChannelNames.ChatFast)
            {
                await ForwardChatFastLaneEventAsync(envelope.EventName, envelope.Payload);
                return;
            }

            if (channel == RealtimeChannelNames.Gacha
                || channel == RealtimeChannelNames.Gamification
                || channel == RealtimeChannelNames.UserState)
            {
                await ForwardPresenceEventAsync(envelope.EventName, envelope.Payload);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to bridge Redis realtime message for channel {Channel}.", channel);
        }
    }

    private bool ShouldSkipDuplicatedFastLaneEvent(string? eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            return false;
        }

        var now = DateTime.UtcNow;
        var ttlCutoff = now.AddMinutes(-10);
        foreach (var pair in _bridgeDedupByEventId)
        {
            if (pair.Value < ttlCutoff)
            {
                _bridgeDedupByEventId.TryRemove(pair.Key, out _);
            }
        }

        return !_bridgeDedupByEventId.TryAdd(eventId, now);
    }
}
