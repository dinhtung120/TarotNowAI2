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
    private readonly ConcurrentDictionary<string, SubscriptionState> _subscriptions = new(StringComparer.Ordinal);

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

    private SubscriptionState CreateSubscriptionState(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken)
    {
        var useLosslessQueue = string.Equals(channel, RealtimeChannelNames.UserState, StringComparison.Ordinal);
        var messageQueue = useLosslessQueue
            ? Channel.CreateUnbounded<RedisRealtimeMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            })
            : Channel.CreateBounded<RedisRealtimeMessage>(new BoundedChannelOptions(MaxBufferedMessagesPerChannel)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropOldest
            });
        var state = new SubscriptionState(channel, useLosslessQueue, messageQueue);

        var worker = Task.Run(async () =>
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

        var callback = new Action<RedisChannel, RedisValue>((redisChannel, redisValue) =>
        {
            var message = new RedisRealtimeMessage(redisChannel.ToString(), redisValue.ToString());
            if (messageQueue.Writer.TryWrite(message) == false)
            {
                if (useLosslessQueue)
                {
                    _logger.LogError(
                        "Failed to enqueue lossless realtime message. Channel={Channel}",
                        redisChannel.ToString());
                    return;
                }

                _logger.LogWarning(
                    "Dropped realtime message because bridge queue is full. Channel={Channel}, Capacity={Capacity}",
                    redisChannel.ToString(),
                    MaxBufferedMessagesPerChannel);
                return;
            }

            state.MarkEnqueued(_logger);
        });

        state.AttachRuntime(callback, worker);
        return state;
    }

    private static void ValidateArguments(string channel, Func<string, string, Task> onMessageAsync)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel is required.", nameof(channel));
        }

        ArgumentNullException.ThrowIfNull(onMessageAsync);
    }

    private readonly record struct RedisRealtimeMessage(string Channel, string PayloadJson);

    private sealed class SubscriptionState
    {
        private long _pendingMessages;
        private long _lastWarnedBacklogBucket;

        public SubscriptionState(
            string channel,
            bool useLosslessQueue,
            Channel<RedisRealtimeMessage> queue)
        {
            Channel = channel;
            UseLosslessQueue = useLosslessQueue;
            Queue = queue;
        }

        public string Channel { get; }

        public Action<RedisChannel, RedisValue> Callback { get; private set; } = null!;

        public bool UseLosslessQueue { get; }

        private Channel<RedisRealtimeMessage> Queue { get; }

        private Task Worker { get; set; } = Task.CompletedTask;

        public void AttachRuntime(Action<RedisChannel, RedisValue> callback, Task worker)
        {
            Callback = callback;
            Worker = worker;
        }

        public void MarkEnqueued(ILogger logger)
        {
            if (!UseLosslessQueue)
            {
                return;
            }

            var pending = Interlocked.Increment(ref _pendingMessages);
            if (pending < PresenceBacklogWarnThreshold)
            {
                return;
            }

            var bucket = pending / PresenceBacklogWarnThreshold;
            var lastBucket = Interlocked.Read(ref _lastWarnedBacklogBucket);
            if (bucket <= lastBucket)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _lastWarnedBacklogBucket, bucket, lastBucket) != lastBucket)
            {
                return;
            }

            logger.LogWarning(
                "Presence realtime backlog is high. Channel={Channel}, PendingMessages={PendingMessages}, Threshold={Threshold}",
                Channel,
                pending,
                PresenceBacklogWarnThreshold);
        }

        public void MarkDequeued(ILogger logger)
        {
            if (!UseLosslessQueue)
            {
                return;
            }

            var pending = Interlocked.Decrement(ref _pendingMessages);
            if (pending >= PresenceBacklogWarnThreshold || pending < 0)
            {
                return;
            }

            var lastBucket = Interlocked.Read(ref _lastWarnedBacklogBucket);
            if (lastBucket <= 0)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _lastWarnedBacklogBucket, 0, lastBucket) != lastBucket)
            {
                return;
            }

            logger.LogInformation(
                "Presence realtime backlog recovered. Channel={Channel}, PendingMessages={PendingMessages}",
                Channel,
                Math.Max(0, pending));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Queue.Writer.TryComplete();

            try
            {
                await Worker.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }
        }
    }
}
