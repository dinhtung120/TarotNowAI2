using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
{
    // Mô tả dữ liệu đầu vào để ghi system message theo đúng loại/nội dung/thời điểm.
    private readonly record struct SystemMessageSpec(string Type, string Content, DateTime? CreatedAt);

    /// <summary>
    /// Hủy đề nghị cộng thêm tiền đang chờ duyệt khi bắt đầu luồng yêu cầu hoàn thành.
    /// Luồng xử lý: tìm offer pending gần nhất, nếu có thì ghi message từ chối offer rồi ghi thêm message giải thích lý do hủy.
    /// </summary>
    private async Task<DateTime?> CancelPendingAddMoneyOfferAsync(
        ConversationDto conversation,
        string actorId,
        CancellationToken cancellationToken)
    {
        var pendingOffer = await _chatMessageRepository.FindLatestPendingPaymentOfferAsync(
            conversation.Id,
            cancellationToken);

        if (pendingOffer == null)
        {
            // Không có offer pending thì không cần phát sinh system message hủy.
            return null;
        }

        var rejectPayload = $"{{\"offerMessageId\":\"{pendingOffer.Id}\",\"proposalId\":\"{pendingOffer.PaymentPayload?.ProposalId ?? string.Empty}\",\"note\":\"cancelled_by_complete_request\"}}";
        var rejectAt = await AddSystemMessageAsync(
            conversation,
            actorId,
            new SystemMessageSpec(ChatMessageType.PaymentReject, rejectPayload, conversation.LastMessageAt),
            cancellationToken);

        return await AddSystemMessageAsync(
            conversation,
            actorId,
            new SystemMessageSpec(
                ChatMessageType.System,
                "Yêu cầu cộng thêm tiền đã bị hủy do một bên yêu cầu hoàn thành cuộc trò chuyện.",
                rejectAt),
            cancellationToken);
    }

    /// <summary>
    /// Thêm một system message vào conversation và trả về mốc thời gian đã ghi.
    /// Luồng xử lý: map dữ liệu từ spec sang DTO, lưu vào repository, rồi trả CreatedAt để đồng bộ timeline.
    /// </summary>
    private async Task<DateTime> AddSystemMessageAsync(
        ConversationDto conversation,
        string senderId,
        SystemMessageSpec spec,
        CancellationToken cancellationToken)
    {
        var message = new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = senderId,
            Type = spec.Type,
            Content = spec.Content,
            IsRead = false,
            CreatedAt = spec.CreatedAt ?? DateTime.UtcNow
        };

        await _chatMessageRepository.AddAsync(message, cancellationToken);
        // Trả lại CreatedAt thực tế để các bước sau cập nhật LastMessageAt chính xác.
        return message.CreatedAt;
    }
}
