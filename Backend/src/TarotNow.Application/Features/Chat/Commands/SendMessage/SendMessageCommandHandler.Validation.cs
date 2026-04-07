using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private static void ValidateRequest(SendMessageCommand request)
    {
        if (ChatMessageType.IsValid(request.Type) == false)
        {
            throw new BadRequestException($"Loại tin nhắn không hợp lệ: {request.Type}");
        }

        if (request.Type == ChatMessageType.Text && string.IsNullOrWhiteSpace(request.Content))
        {
            throw new BadRequestException("Nội dung tin nhắn không được để trống.");
        }

        ValidateMediaRequest(request);
    }

    private async Task<ConversationDto> LoadConversationAsync(
        SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        return await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
    }

    private static void ValidateSender(ConversationDto conversation, string senderId)
    {
        if (conversation.UserId != senderId && conversation.ReaderId != senderId)
        {
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }
    }

    private static void ValidateConversationForSend(ConversationDto conversation, string senderId, string messageType)
    {
        
        if (messageType == ChatMessageType.System || messageType == ChatMessageType.CallLog)
            return;

        if (conversation.Status == ConversationStatus.Disputed || ConversationStatus.IsTerminal(conversation.Status))
        {
            throw new BadRequestException($"Cuộc trò chuyện đã kết thúc ({conversation.Status}).");
        }

        if (conversation.Status == ConversationStatus.Pending && senderId == conversation.ReaderId)
        {
            throw new BadRequestException("User chưa gửi câu hỏi đầu tiên nên Reader chưa thể nhắn tin.");
        }

        if (conversation.Status == ConversationStatus.AwaitingAcceptance)
        {
            throw new BadRequestException("Cuộc trò chuyện đang chờ Reader accept.");
        }
    }

    private static void ApplyConversationStateTransition(
        ConversationDto conversation,
        string senderId,
        DateTime? offerExpiresAtUtc)
    {
        if (conversation.Status == ConversationStatus.Pending
            && senderId == conversation.UserId
            && offerExpiresAtUtc.HasValue)
        {
            conversation.Status = ConversationStatus.AwaitingAcceptance;
            conversation.OfferExpiresAt = offerExpiresAtUtc.Value;
        }
    }

    private static ChatMessageDto BuildMessage(SendMessageCommand request, string senderId)
    {
        return new ChatMessageDto
        {
            ConversationId = request.ConversationId,
            SenderId = senderId,
            Type = request.Type,
            Content = request.Content,
            PaymentPayload = request.PaymentPayload,
            MediaPayload = request.MediaPayload,
            CallPayload = request.CallPayload,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static void IncrementUnreadCounter(ConversationDto conversation, string senderId)
    {
        if (senderId == conversation.UserId)
        {
            conversation.UnreadCountReader += 1;
        }
        else
        {
            conversation.UnreadCountUser += 1;
        }
    }
}
