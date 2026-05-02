using System.Threading.Channels;
using StackExchange.Redis;

namespace TarotNow.Api.Realtime;

internal readonly record struct RedisRealtimeBridgeMessage(string Channel, string PayloadJson);

internal sealed class RedisRealtimeBridgeSubscriptionState
{
    private long _pendingMessages;
    private long _lastWarnedBacklogBucket;

    public RedisRealtimeBridgeSubscriptionState(
        string channel,
        bool useLosslessQueue,
        Channel<RedisRealtimeBridgeMessage> queue,
        int backlogWarnThreshold)
    {
        Channel = channel;
        UseLosslessQueue = useLosslessQueue;
        Queue = queue;
        BacklogWarnThreshold = backlogWarnThreshold;
    }

    public string Channel { get; }

    public Action<RedisChannel, RedisValue> Callback { get; private set; } = null!;

    public bool UseLosslessQueue { get; }

    private int BacklogWarnThreshold { get; }

    private Channel<RedisRealtimeBridgeMessage> Queue { get; }

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
        if (pending < BacklogWarnThreshold)
        {
            return;
        }

        var bucket = pending / BacklogWarnThreshold;
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
            BacklogWarnThreshold);
    }

    public void MarkDequeued(ILogger logger)
    {
        if (!UseLosslessQueue)
        {
            return;
        }

        var pending = Interlocked.Decrement(ref _pendingMessages);
        if (pending >= BacklogWarnThreshold || pending < 0)
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
