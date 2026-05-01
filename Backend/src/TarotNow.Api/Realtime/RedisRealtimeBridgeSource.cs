using System.Collections.Concurrent;
using System.Threading.Channels;
using StackExchange.Redis;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Source realtime dựa trên Redis Pub/Sub.
/// </summary>
public sealed class RedisRealtimeBridgeSource : IRealtimeBridgeSource
{
    private const int MaxBufferedMessagesPerChannel = 2048;

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
        var messageQueue = Channel.CreateBounded<RedisRealtimeMessage>(new BoundedChannelOptions(MaxBufferedMessagesPerChannel)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });

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
            }
        }, cancellationToken);

        var callback = new Action<RedisChannel, RedisValue>((redisChannel, redisValue) =>
        {
            var message = new RedisRealtimeMessage(redisChannel.ToString(), redisValue.ToString());
            if (messageQueue.Writer.TryWrite(message) == false)
            {
                _logger.LogWarning(
                    "Dropped realtime message because bridge queue is full. Channel={Channel}, Capacity={Capacity}",
                    redisChannel.ToString(),
                    MaxBufferedMessagesPerChannel);
            }
        });

        return new SubscriptionState(channel, callback, messageQueue, worker);
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
        public SubscriptionState(
            string channel,
            Action<RedisChannel, RedisValue> callback,
            Channel<RedisRealtimeMessage> queue,
            Task worker)
        {
            Channel = channel;
            Callback = callback;
            Queue = queue;
            Worker = worker;
        }

        public string Channel { get; }

        public Action<RedisChannel, RedisValue> Callback { get; }

        private Channel<RedisRealtimeMessage> Queue { get; }

        private Task Worker { get; }

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
