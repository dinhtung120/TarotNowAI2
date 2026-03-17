using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

/// <summary>
/// Handler persist tin nhắn + cập nhật conversation metadata.
///
/// Không broadcast qua SignalR ở đây — SignalR Hub gọi handler rồi tự broadcast.
/// Tách logic persist khỏi realtime delivery để dễ test.
/// </summary>
public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public SendMessageCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate message type
        if (!ChatMessageType.IsValid(request.Type))
            throw new BadRequestException($"Loại tin nhắn không hợp lệ: {request.Type}");

        // 2. Validate content — text phải có nội dung
        if (request.Type == ChatMessageType.Text && string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Nội dung tin nhắn không được để trống.");

        // 3. Lấy conversation — kiểm tra tồn tại
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // 4. Kiểm tra quyền truy cập — sender phải là member
        var senderId = request.SenderId.ToString();
        if (conversation.UserId != senderId && conversation.ReaderId != senderId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 5. Kiểm tra conversation status — chỉ active/pending mới gửi được
        if (conversation.Status != ConversationStatus.Active && conversation.Status != ConversationStatus.Pending)
            throw new BadRequestException($"Cuộc trò chuyện đã kết thúc ({conversation.Status}).");

        // 6. Auto-activate khi tin đầu tiên
        if (conversation.Status == ConversationStatus.Pending)
        {
            conversation.Status = ConversationStatus.Active;
        }

        // 7. Tạo message DTO
        var message = new ChatMessageDto
        {
            ConversationId = request.ConversationId,
            SenderId = senderId,
            Type = request.Type,
            Content = request.Content,
            PaymentPayload = request.PaymentPayload,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // 8. Persist message
        await _messageRepo.AddAsync(message, cancellationToken);

        // 9. Cập nhật conversation metadata
        conversation.LastMessageAt = message.CreatedAt;
        conversation.UpdatedAt = message.CreatedAt;

        // Tăng unread count cho người nhận (không phải sender)
        // Dùng $set thay vì $inc theo schema.md recommendation
        if (senderId == conversation.UserId)
            conversation.UnreadCountReader += 1;
        else
            conversation.UnreadCountUser += 1;

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        return message;
    }
}
