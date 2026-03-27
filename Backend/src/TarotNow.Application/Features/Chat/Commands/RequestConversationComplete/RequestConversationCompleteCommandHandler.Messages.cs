using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandler
{
    private readonly record struct SystemMessageSpec(string Type, string Content, DateTime? CreatedAt);

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
        return message.CreatedAt;
    }
}
