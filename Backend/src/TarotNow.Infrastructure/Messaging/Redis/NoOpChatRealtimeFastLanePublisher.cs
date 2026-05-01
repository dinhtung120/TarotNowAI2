using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Messaging.Redis;

/// <summary>
/// Publisher no-op cho chat fast-lane khi Redis unavailable.
/// </summary>
public sealed class NoOpChatRealtimeFastLanePublisher : IChatRealtimeFastLanePublisher
{
    /// <inheritdoc />
    public Task PublishAsync(ChatRealtimeEnvelopeV2 envelope, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
