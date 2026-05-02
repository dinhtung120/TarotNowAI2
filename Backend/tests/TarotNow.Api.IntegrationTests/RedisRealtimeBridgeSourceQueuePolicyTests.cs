using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StackExchange.Redis;
using TarotNow.Api.Realtime;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.IntegrationTests;

public sealed class RedisRealtimeBridgeSourceQueuePolicyTests
{
    [Fact]
    public async Task UserStateChannel_ShouldDeliverAllMessagesWithoutDropUnderBacklogPressure()
    {
        var callbacks = new Dictionary<string, Action<RedisChannel, RedisValue>>(StringComparer.Ordinal);
        var source = CreateSource(callbacks);
        var processedCount = 0;
        const int publishCount = 3_000;

        await source.SubscribeAsync(
            RealtimeChannelNames.UserState,
            async (_, _) =>
            {
                Interlocked.Increment(ref processedCount);
                await Task.Delay(2);
            });

        var callback = callbacks[RealtimeChannelNames.UserState];
        for (var index = 0; index < publishCount; index++)
        {
            callback(RedisChannel.Literal(RealtimeChannelNames.UserState), RedisValue.Null);
        }

        await WaitUntilAsync(
            () => Volatile.Read(ref processedCount) == publishCount,
            TimeSpan.FromSeconds(20));

        await source.UnsubscribeAsync(RealtimeChannelNames.UserState);
        Assert.Equal(publishCount, Volatile.Read(ref processedCount));
    }

    [Fact]
    public async Task NonCriticalChannel_ShouldDropMessagesWhenQueueOverflows()
    {
        var callbacks = new Dictionary<string, Action<RedisChannel, RedisValue>>(StringComparer.Ordinal);
        var source = CreateSource(callbacks);
        var processedCount = 0;
        const int publishCount = 5_000;

        await source.SubscribeAsync(
            RealtimeChannelNames.Notifications,
            async (_, _) =>
            {
                Interlocked.Increment(ref processedCount);
                await Task.Delay(5);
            });

        var callback = callbacks[RealtimeChannelNames.Notifications];
        for (var index = 0; index < publishCount; index++)
        {
            callback(RedisChannel.Literal(RealtimeChannelNames.Notifications), RedisValue.Null);
        }

        await source.UnsubscribeAsync(RealtimeChannelNames.Notifications);
        Assert.True(Volatile.Read(ref processedCount) < publishCount);
    }

    private static RedisRealtimeBridgeSource CreateSource(
        IDictionary<string, Action<RedisChannel, RedisValue>> callbacks)
    {
        var subscriber = new Mock<ISubscriber>(MockBehavior.Strict);
        subscriber
            .Setup(x => x.SubscribeAsync(
                It.IsAny<RedisChannel>(),
                It.IsAny<Action<RedisChannel, RedisValue>>(),
                It.IsAny<CommandFlags>()))
            .Callback<RedisChannel, Action<RedisChannel, RedisValue>, CommandFlags>((channel, callback, _) =>
            {
                callbacks[channel.ToString()] = callback;
            })
            .Returns(Task.CompletedTask);
        subscriber
            .Setup(x => x.UnsubscribeAsync(
                It.IsAny<RedisChannel>(),
                It.IsAny<Action<RedisChannel, RedisValue>>(),
                It.IsAny<CommandFlags>()))
            .Returns(Task.CompletedTask);

        var multiplexer = new Mock<IConnectionMultiplexer>(MockBehavior.Strict);
        multiplexer
            .Setup(x => x.GetSubscriber(It.IsAny<object>()))
            .Returns(subscriber.Object);

        return new RedisRealtimeBridgeSource(
            multiplexer.Object,
            NullLogger<RedisRealtimeBridgeSource>.Instance);
    }

    private static async Task WaitUntilAsync(Func<bool> condition, TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow + timeout;
        while (DateTime.UtcNow < deadline)
        {
            if (condition())
            {
                return;
            }

            await Task.Delay(25);
        }

        throw new Xunit.Sdk.XunitException("Condition was not met within timeout.");
    }
}
