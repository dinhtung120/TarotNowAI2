using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Validate dữ liệu đầu vào cơ bản cho lệnh gửi tin nhắn.
    /// Luồng xử lý: kiểm tra message type hợp lệ, kiểm tra content bắt buộc với text, rồi validate media khi cần.
    /// </summary>
    private static void ValidateRequest(SendMessageCommand request)
    {
        if (ChatMessageType.IsValid(request.Type) == false)
        {
            // Chặn loại tin nhắn lạ để tránh phá vỡ contract xử lý downstream.
            throw new BadRequestException($"Loại tin nhắn không hợp lệ: {request.Type}");
        }

        if (request.Type == ChatMessageType.Text && string.IsNullOrWhiteSpace(request.Content))
        {
            // Tin nhắn text bắt buộc có nội dung để tránh tạo message rỗng.
            throw new BadRequestException("Nội dung tin nhắn không được để trống.");
        }

        ValidateMediaRequest(request);
    }

    /// <summary>
    /// Tải conversation phục vụ luồng gửi tin nhắn.
    /// Luồng xử lý: lấy conversation theo id và ném lỗi nếu không tồn tại.
    /// </summary>
    private async Task<ConversationDto> LoadConversationAsync(
        SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        return await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
    }

    /// <summary>
    /// Kiểm tra sender có thuộc conversation hay không.
    /// Luồng xử lý: so khớp senderId với userId/readerId của conversation.
    /// </summary>
    private static void ValidateSender(ConversationDto conversation, string senderId)
    {
        if (conversation.UserId != senderId && conversation.ReaderId != senderId)
        {
            // Chặn người dùng ngoài conversation gửi tin nhắn trái phép.
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }
    }

    /// <summary>
    /// Kiểm tra trạng thái conversation có cho phép gửi message loại hiện tại hay không.
    /// Luồng xử lý: cho phép system message đi qua, còn lại áp dụng rule trạng thái terminal/pending/awaiting acceptance.
    /// </summary>
    private static void ValidateConversationForSend(ConversationDto conversation, string senderId, string messageType)
    {
        if (messageType == ChatMessageType.System)
        {
            // System message là message kỹ thuật, không chặn theo flow chat thông thường.
            return;
        }

        if (conversation.Status == ConversationStatus.Disputed || ConversationStatus.IsTerminal(conversation.Status))
        {
            // Conversation đã kết thúc/tranh chấp thì không cho gửi message nghiệp vụ mới.
            throw new BadRequestException($"Cuộc trò chuyện đã kết thúc ({conversation.Status}).");
        }

        if (conversation.Status == ConversationStatus.Pending && senderId == conversation.ReaderId)
        {
            // Reader chưa được nhắn khi user chưa gửi tin đầu để kích hoạt phiên.
            throw new BadRequestException("User chưa gửi câu hỏi đầu tiên nên Reader chưa thể nhắn tin.");
        }

        if (conversation.Status == ConversationStatus.AwaitingAcceptance)
        {
            // Khi đang chờ accept thì chỉ xử lý flow acceptance, không cho gửi message tự do.
            throw new BadRequestException("Cuộc trò chuyện đang chờ Reader accept.");
        }
    }

    /// <summary>
    /// Áp dụng chuyển trạng thái conversation sau khi gửi tin đầu tiên của user.
    /// Luồng xử lý: từ Pending sang AwaitingAcceptance và set OfferExpiresAt khi có freeze hợp lệ.
    /// </summary>
    private static void ApplyConversationStateTransition(
        ConversationDto conversation,
        string senderId,
        DateTime? offerExpiresAtUtc)
    {
        if (conversation.Status == ConversationStatus.Pending
            && senderId == conversation.UserId
            && offerExpiresAtUtc.HasValue)
        {
            // Đây là điểm đổi state chính từ mở chat sang chờ reader chấp nhận.
            conversation.Status = ConversationStatus.AwaitingAcceptance;
            conversation.OfferExpiresAt = offerExpiresAtUtc.Value;
        }
    }

    /// <summary>
    /// Dựng ChatMessageDto từ request đã qua validate.
    /// Luồng xử lý: map dữ liệu chính và gắn CreatedAt UTC để chuẩn hóa timeline.
    /// </summary>
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
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Tăng bộ đếm unread cho phía đối diện khi có tin nhắn mới.
    /// Luồng xử lý: nếu sender là user thì tăng unread của reader, ngược lại tăng unread của user.
    /// </summary>
    private static void IncrementUnreadCounter(ConversationDto conversation, string senderId)
    {
        if (senderId == conversation.UserId)
        {
            // User gửi thì reader là phía chưa đọc.
            conversation.UnreadCountReader += 1;
        }
        else
        {
            // Reader gửi thì user là phía chưa đọc.
            conversation.UnreadCountUser += 1;
        }
    }
}
