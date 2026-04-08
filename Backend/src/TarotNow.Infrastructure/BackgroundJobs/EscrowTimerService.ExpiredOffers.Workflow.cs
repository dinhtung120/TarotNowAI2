using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    // Kết quả xử lý refund expired offer để dùng ở bước cập nhật conversation.
    private readonly record struct ExpiredOfferOutcome(string? ConversationId, long RefundedAmount);

    /// <summary>
    /// Thực thi transaction refund cho một offer đã hết hạn.
    /// Luồng xử lý: lock item, kiểm tra eligibility pending+expired, refund ví, cập nhật state/session và commit.
    /// </summary>
    private async Task<ExpiredOfferOutcome> ExecuteExpiredOfferRefundAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = new ExpiredOfferOutcome(null, 0);

        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || IsEligibleForExpiredOfferRefund(item, DateTime.UtcNow) == false)
            {
                // Item không tồn tại hoặc không còn đủ điều kiện hết hạn thì bỏ qua.
                return;
            }

            await RefundExpiredOfferAsync(dependencies, item, transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            // Commit sau khi refund + update state để đảm bảo atomicity của một candidate.
            outcome = new ExpiredOfferOutcome(item.ConversationRef, item.AmountDiamond);
        }, cancellationToken);

        return outcome;
    }

    /// <summary>
    /// Kiểm tra item có đủ điều kiện refund vì offer hết hạn hay không.
    /// Luồng xử lý: yêu cầu status pending, có OfferExpiresAt và thời điểm hiện tại đã vượt mốc này.
    /// </summary>
    private static bool IsEligibleForExpiredOfferRefund(ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Pending
               && item.OfferExpiresAt != null
               && item.OfferExpiresAt <= now;
    }

    /// <summary>
    /// Thực hiện refund cho expired offer và cập nhật toàn bộ state liên quan.
    /// Luồng xử lý: refund ví payer, cập nhật state item, trừ frozen session, mark refunded session và publish event.
    /// </summary>
    private static async Task RefundExpiredOfferAsync(
        RefundDependencies dependencies,
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        await dependencies.WalletRepository.RefundAsync(
            item.PayerId,
            item.AmountDiamond,
            referenceSource: "chat_question_item",
            referenceId: item.Id.ToString(),
            description: $"Expired offer refund {item.AmountDiamond}💎",
            idempotencyKey: $"settle_refund_{item.Id}",
            cancellationToken: cancellationToken);
        // Hoàn tiền theo idempotency key cố định để tránh refund trùng khi retry.

        ApplyExpiredOfferRefundState(item, DateTime.UtcNow);
        await dependencies.FinanceRepository.UpdateItemAsync(item, cancellationToken);
        await UpdateSessionFrozenBalanceAsync(dependencies.FinanceRepository, item, cancellationToken);
        await MarkSessionRefundedWhenFullyReleasedAsync(dependencies.FinanceRepository, item.FinanceSessionId, cancellationToken);
        await PublishExpiredOfferEventAsync(dependencies, item, cancellationToken);
    }

    /// <summary>
    /// Áp trạng thái refunded cho expired offer item.
    /// Luồng xử lý: set status/refundedAt/dispute window và updatedAt tại cùng mốc thời gian.
    /// </summary>
    private static void ApplyExpiredOfferRefundState(ChatQuestionItem item, DateTime now)
    {
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
        item.UpdatedAt = now;
    }

    /// <summary>
    /// Publish domain event EscrowRefunded cho trường hợp offer hết hạn.
    /// Luồng xử lý: dựng payload với RefundSource = offer_expired và gửi qua publisher.
    /// </summary>
    private static Task PublishExpiredOfferEventAsync(
        RefundDependencies dependencies,
        ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        return dependencies.DomainEventPublisher.PublishAsync(new EscrowRefundedDomainEvent
        {
            ItemId = item.Id,
            UserId = item.PayerId,
            AmountDiamond = item.AmountDiamond,
            RefundSource = "offer_expired"
        }, cancellationToken);
    }
}
