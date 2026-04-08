using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Xử lý các payment offer add-money đã hết hạn 24h.
    /// Luồng xử lý: lấy offer pending quá hạn, xử lý từng offer và bắt lỗi cục bộ theo item.
    /// </summary>
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
                // Không dừng toàn job khi một offer gặp lỗi.
            }
        }
    }

    /// <summary>
    /// Hủy một add-money offer đã hết hạn và phát sinh các system/payment reject message liên quan.
    /// Luồng xử lý: kiểm tra conversation còn ongoing, tránh xử lý trùng, tạo message reject + system, cập nhật timestamp.
    /// </summary>
    private static async Task ProcessExpiredAddMoneyOfferAsync(
        RefundDependencies dependencies,
        ChatMessageDto offer,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(offer.ConversationId, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Ongoing)
        {
            // Chỉ xử lý offer trong conversation còn ongoing để tránh ghi trạng thái sai ngữ cảnh.
            return;
        }

        var alreadyHandled = await dependencies.MessageRepository.HasPaymentOfferResponseAsync(
            offer.ConversationId,
            offer.Id,
            cancellationToken);
        if (alreadyHandled)
        {
            // Edge case: offer đã có phản hồi accept/reject thì không auto-reject nữa.
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
        // Tạo payment reject tự động với note timeout_24h để hệ thống phía client hiểu lý do.

        await dependencies.MessageRepository.AddAsync(new ChatMessageDto
        {
            ConversationId = offer.ConversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.System,
            Content = "Yêu cầu cộng thêm tiền đã hết hạn sau 24 giờ và được tự động hủy.",
            IsRead = false,
            CreatedAt = now
        }, cancellationToken);
        // Gửi thêm system message để giải thích rõ offer đã tự hủy do quá hạn.

        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
