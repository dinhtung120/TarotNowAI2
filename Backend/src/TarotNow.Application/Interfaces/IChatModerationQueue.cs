using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface IChatModerationQueue
{
    ValueTask EnqueueAsync(ChatModerationPayload payload, CancellationToken cancellationToken = default);
}
