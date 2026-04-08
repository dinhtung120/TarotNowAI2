using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.QueueChatModeration;

// Command đẩy payload chat vào hàng đợi moderation.
public class QueueChatModerationCommand : IRequest
{
    // Payload nội dung cần kiểm duyệt.
    public ChatModerationPayload Payload { get; set; } = new();
}

// Handler enqueue payload moderation vào queue backend.
public class QueueChatModerationCommandHandler : IRequestHandler<QueueChatModerationCommand>
{
    private readonly IChatModerationQueue _moderationQueue;

    /// <summary>
    /// Khởi tạo handler queue chat moderation.
    /// Luồng xử lý: nhận hàng đợi moderation để enqueue payload kiểm duyệt.
    /// </summary>
    public QueueChatModerationCommandHandler(IChatModerationQueue moderationQueue)
    {
        _moderationQueue = moderationQueue;
    }

    /// <summary>
    /// Xử lý command đưa payload vào hàng đợi moderation.
    /// Luồng xử lý: gọi enqueue async với payload và cancellation token hiện tại.
    /// </summary>
    public async Task Handle(QueueChatModerationCommand request, CancellationToken cancellationToken)
    {
        await _moderationQueue.EnqueueAsync(request.Payload, cancellationToken);
    }
}
