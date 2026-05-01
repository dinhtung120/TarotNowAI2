using TarotNow.Application.Common.Realtime;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract phát sự kiện realtime fast-lane cho chat.
/// </summary>
public interface IChatRealtimeFastLanePublisher
{
    /// <summary>
    /// Publish một envelope chat realtime lên fast-lane channel.
    /// </summary>
    Task PublishAsync(ChatRealtimeEnvelopeV2 envelope, CancellationToken cancellationToken = default);
}
