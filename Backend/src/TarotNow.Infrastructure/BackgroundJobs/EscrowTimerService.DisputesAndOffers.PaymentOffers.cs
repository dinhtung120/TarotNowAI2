using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessExpiredAddMoneyOffers(
        RefundDependencies dependencies,
        CancellationToken cancellationToken)
    {
        var expiredOffers = await dependencies.MessageRepository.GetExpiredPendingPaymentOffersAsync(
            DateTime.UtcNow,
            200,
            cancellationToken);

        foreach (var offer in expiredOffers)
        {
            try
            {
                await ProcessExpiredAddMoneyOfferAsync(dependencies, offer, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Expired add-money offer handling failed: {OfferId}", offer.Id);
            }
        }
    }

    private static async Task ProcessExpiredAddMoneyOfferAsync(
        RefundDependencies dependencies,
        ChatMessageDto offer,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(offer.ConversationId, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Ongoing)
        {
            return;
        }

        var alreadyHandled = await dependencies.MessageRepository.HasPaymentOfferResponseAsync(
            offer.ConversationId,
            offer.Id,
            cancellationToken);
        if (alreadyHandled)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var rejectPayload = $"{{\"offerMessageId\":\"{offer.Id}\",\"proposalId\":\"{offer.PaymentPayload?.ProposalId ?? string.Empty}\",\"note\":\"timeout_24h\"}}";

        await dependencies.MessageRepository.AddAsync(new ChatMessageDto
        {
            ConversationId = offer.ConversationId,
            SenderId = conversation.UserId,
            Type = ChatMessageType.PaymentReject,
            Content = rejectPayload,
            IsRead = false,
            CreatedAt = now
        }, cancellationToken);

        await dependencies.MessageRepository.AddAsync(new ChatMessageDto
        {
            ConversationId = offer.ConversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.System,
            Content = "Yêu cầu cộng thêm tiền đã hết hạn sau 24 giờ và được tự động hủy.",
            IsRead = false,
            CreatedAt = now
        }, cancellationToken);

        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
