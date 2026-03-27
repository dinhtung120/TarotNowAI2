using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

public class QueueChatModerationCommand : IRequest
{
    public ChatModerationPayload Payload { get; set; } = new();
}

public class QueueChatModerationCommandHandler : IRequestHandler<QueueChatModerationCommand>
{
    private readonly IChatModerationQueue _moderationQueue;

    public QueueChatModerationCommandHandler(IChatModerationQueue moderationQueue)
    {
        _moderationQueue = moderationQueue;
    }

    public async Task Handle(QueueChatModerationCommand request, CancellationToken cancellationToken)
    {
        await _moderationQueue.EnqueueAsync(request.Payload, cancellationToken);
    }
}
