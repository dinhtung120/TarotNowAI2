using System.Collections.Concurrent;
using System.Text.Json;
using TarotNow.Api.Realtime;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.IntegrationTests.Realtime;

/// <summary>
/// Source realtime in-memory dùng cho integration tests routing matrix.
/// </summary>
public sealed class InMemoryRealtimeBridgeSource : IRealtimeBridgeSource
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly ConcurrentDictionary<string, ConcurrentBag<Func<string, string, Task>>> _subscriptions =
        new(StringComparer.Ordinal);

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public Task SubscribeAsync(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel is required.", nameof(channel));
        }

        ArgumentNullException.ThrowIfNull(onMessageAsync);

        var handlers = _subscriptions.GetOrAdd(channel, _ => new ConcurrentBag<Func<string, string, Task>>());
        handlers.Add(onMessageAsync);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        _subscriptions.TryRemove(channel, out _);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publish một envelope realtime vào channel đã subscribe.
    /// </summary>
    public async Task PublishEnvelopeAsync(string channel, string eventName, object payload)
    {
        var envelope = new RedisRealtimeEnvelope
        {
            EventName = eventName,
            Payload = JsonSerializer.SerializeToElement(payload, JsonOptions),
            OccurredAtUtc = DateTime.UtcNow
        };

        var payloadJson = JsonSerializer.Serialize(envelope, JsonOptions);
        if (_subscriptions.TryGetValue(channel, out var handlers) == false)
        {
            return;
        }

        foreach (var handler in handlers)
        {
            await handler(channel, payloadJson);
        }
    }

    /// <summary>
    /// Chờ cho tới khi source có đủ số channel được subscribe.
    /// </summary>
    public async Task<bool> WaitForSubscriptionCountAsync(int expectedCount, TimeSpan timeout)
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            if (_subscriptions.Count >= expectedCount)
            {
                return true;
            }

            await Task.Delay(50);
        }

        return _subscriptions.Count >= expectedCount;
    }
}
