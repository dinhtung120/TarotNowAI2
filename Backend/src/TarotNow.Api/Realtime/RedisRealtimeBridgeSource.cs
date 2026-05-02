using System.Collections.Concurrent;
using System.Threading.Channels;
using StackExchange.Redis;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Source realtime dựa trên Redis Pub/Sub.
/// </summary>
public sealed class RedisRealtimeBridgeSource : IRealtimeBridgeSource
{
    private const int MaxBufferedMessagesPerChannel = 2048;
    private const int PresenceBacklogWarnThreshold = 1000;

    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisRealtimeBridgeSource> _logger;
    private readonly ConcurrentDictionary<string, RedisRealtimeBridgeSubscriptionState> _subscriptions = new(StringComparer.Ordinal);

    /// <summary>
    /// Khởi tạo Redis realtime source.
    /// </summary>
    public RedisRealtimeBridgeSource(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisRealtimeBridgeSource> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public async Task SubscribeAsync(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken = default)
    {
        ValidateArguments(channel, onMessageAsync);

        if (_subscriptions.ContainsKey(channel))
        {
            return;
        }

        var state = CreateSubscriptionState(channel, onMessageAsync, cancellationToken);
        if (_subscriptions.TryAdd(channel, state) == false)
        {
            return;
        }

        var subscriber = _connectionMultiplexer.GetSubscriber();
        await subscriber.SubscribeAsync(RedisChannel.Literal(channel), state.Callback);
        _logger.LogInformation("Subscribed Redis realtime channel {Channel}.", channel);
    }

    /// <inheritdoc />
    public async Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        if (_subscriptions.TryRemove(channel, out var state) == false)
        {
            return;
        }

        var subscriber = _connectionMultiplexer.GetSubscriber();
        await subscriber.UnsubscribeAsync(RedisChannel.Literal(channel), state.Callback);
        await state.StopAsync(cancellationToken);

        _logger.LogInformation("Unsubscribed Redis realtime channel {Channel}.", channel);
    }

    private RedisRealtimeBridgeSubscriptionState CreateSubscriptionState(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken)
    {
        var useLosslessQueue = string.Equals(channel, RealtimeChannelNames.UserState, StringComparison.Ordinal);
        var messageQueue = CreateMessageQueue(useLosslessQueue);
        var state = new RedisRealtimeBridgeSubscriptionState(
            channel,
            useLosslessQueue,
            messageQueue,
            PresenceBacklogWarnThreshold);
        var worker = CreateWorker(messageQueue, state, onMessageAsync, cancellationToken);
        var callback = CreateCallback(messageQueue, state, useLosslessQueue);

        state.AttachRuntime(callback, worker);
        return state;
    }

    private Channel<RedisRealtimeBridgeMessage> CreateMessageQueue(bool useLosslessQueue)
    {
        if (useLosslessQueue)
        {
            return Channel.CreateUnbounded<RedisRealtimeBridgeMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        return Channel.CreateBounded<RedisRealtimeBridgeMessage>(new BoundedChannelOptions(MaxBufferedMessagesPerChannel)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });
    }

    private Task CreateWorker(
        Channel<RedisRealtimeBridgeMessage> messageQueue,
        RedisRealtimeBridgeSubscriptionState state,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            await foreach (var message in messageQueue.Reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    await onMessageAsync(message.Channel, message.PayloadJson);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Redis realtime callback failed. Channel={Channel}", message.Channel);
                }
                finally
                {
                    state.MarkDequeued(_logger);
                }
            }
        }, cancellationToken);
    }

    private Action<RedisChannel, RedisValue> CreateCallback(
        Channel<RedisRealtimeBridgeMessage> messageQueue,
        RedisRealtimeBridgeSubscriptionState state,
        bool useLosslessQueue)
    {
        return (redisChannel, redisValue) =>
        {
            var channel = redisChannel.ToString();
            var message = new RedisRealtimeBridgeMessage(channel, redisValue.ToString());
            if (messageQueue.Writer.TryWrite(message))
            {
                state.MarkEnqueued(_logger);
                return;
            }

            HandleFailedEnqueue(channel, useLosslessQueue);
        };
    }

    private void HandleFailedEnqueue(string channel, bool useLosslessQueue)
    {
        if (useLosslessQueue)
        {
            _logger.LogError(
                "Failed to enqueue lossless realtime message. Channel={Channel}",
                channel);
            return;
        }

        _logger.LogWarning(
            "Dropped realtime message because bridge queue is full. Channel={Channel}, Capacity={Capacity}",
            channel,
            MaxBufferedMessagesPerChannel);
    }

    private static void ValidateArguments(string channel, Func<string, string, Task> onMessageAsync)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel is required.", nameof(channel));
        }

        ArgumentNullException.ThrowIfNull(onMessageAsync);
    }
}
