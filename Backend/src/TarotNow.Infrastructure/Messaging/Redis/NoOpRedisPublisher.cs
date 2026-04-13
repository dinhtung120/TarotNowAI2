using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Messaging.Redis;

/// <summary>
/// Redis publisher no-op khi hệ thống chạy không có Redis.
/// </summary>
public sealed class NoOpRedisPublisher : IRedisPublisher
{
    /// <inheritdoc />
    public Task PublishAsync(string channel, string eventName, object payload, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
