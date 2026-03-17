using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

/// <summary>
/// Command đánh dấu tin nhắn đã đọc + reset unread_count.
///
/// Gọi khi:
/// → User mở conversation (frontend).
/// → SignalR hub nhận read receipt.
/// </summary>
public class MarkMessagesReadCommand : IRequest<bool>
{
    /// <summary>ObjectId conversation.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID người đọc — dùng để xác định reset unread_count nào.</summary>
    public Guid ReaderId { get; set; }
}

/// <summary>
/// Handler đánh dấu đọc + reset unread counter.
/// </summary>
public class MarkMessagesReadCommandHandler : IRequestHandler<MarkMessagesReadCommand, bool>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public MarkMessagesReadCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<bool> Handle(MarkMessagesReadCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy conversation
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var readerId = request.ReaderId.ToString();

        // 2. Kiểm tra quyền — phải là member
        if (conversation.UserId != readerId && conversation.ReaderId != readerId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 3. Đánh dấu tất cả tin nhắn chưa đọc (gửi bởi người kia) là đã đọc
        await _messageRepo.MarkAsReadAsync(request.ConversationId, readerId, cancellationToken);

        // 4. Reset unread count cho người đọc — dùng $set (theo schema.md)
        if (readerId == conversation.UserId)
            conversation.UnreadCountUser = 0;
        else
            conversation.UnreadCountReader = 0;

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        return true;
    }
}
