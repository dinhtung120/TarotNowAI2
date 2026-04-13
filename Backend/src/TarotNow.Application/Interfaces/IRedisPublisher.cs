namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract publish realtime message lên Redis Pub/Sub.
/// </summary>
public interface IRedisPublisher
{
    /// <summary>
    /// Publish một message realtime lên channel chỉ định.
    /// </summary>
    Task PublishAsync(string channel, string eventName, object payload, CancellationToken cancellationToken = default);
}
