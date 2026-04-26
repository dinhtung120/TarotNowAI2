

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

// Command đánh dấu toàn bộ message trong conversation là đã đọc cho một participant.
public class MarkMessagesReadCommand : IRequest<bool>
{
    // Định danh conversation cần đánh dấu đã đọc.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh participant thực hiện thao tác read.
    public Guid ReaderId { get; set; }
}

// Handler xử lý đánh dấu message đã đọc.
public class MarkMessagesReadCommandExecutor : ICommandExecutionExecutor<MarkMessagesReadCommand, bool>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler mark messages read.
    /// Luồng xử lý: nhận conversation repo để kiểm tra quyền và message repo để cập nhật trạng thái đọc.
    /// </summary>
    public MarkMessagesReadCommandExecutor(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command đánh dấu đã đọc.
    /// Luồng xử lý: tải conversation, kiểm tra participant, mark read cho message, reset unread counter tương ứng.
    /// </summary>
    public async Task<bool> Handle(MarkMessagesReadCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var readerId = request.ReaderId.ToString();

        if (conversation.UserId != readerId && conversation.ReaderId != readerId)
        {
            // Chỉ participant của conversation mới được phép mark read.
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }

        // Cập nhật trạng thái read ở bảng message theo conversation + participant.
        await _messageRepo.MarkAsReadAsync(request.ConversationId, readerId, cancellationToken);

        // Đồng bộ unread counter trên conversation theo phía vừa đọc.
        if (readerId == conversation.UserId)
        {
            conversation.UnreadCountUser = 0;
        }
        else
        {
            conversation.UnreadCountReader = 0;
        }

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);
        var occurredAtUtc = DateTime.UtcNow;

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.UnreadCountChangedDomainEvent
            {
                ConversationId = conversation.Id,
                UserId = conversation.UserId,
                ReaderId = conversation.ReaderId,
                OccurredAtUtc = occurredAtUtc
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ConversationUpdatedDomainEvent(conversation.Id, "message_read", occurredAtUtc),
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new Domain.Events.ChatMessageReadDomainEvent
            {
                ConversationId = conversation.Id,
                UserId = readerId,
                OccurredAtUtc = occurredAtUtc
            },
            cancellationToken);

        return true;
    }
}
