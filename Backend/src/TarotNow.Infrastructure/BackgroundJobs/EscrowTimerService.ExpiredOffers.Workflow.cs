using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private readonly record struct ExpiredOfferOutcome(string? ConversationId, long RefundedAmount);

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
                return;
            }

            await RefundExpiredOfferAsync(dependencies, item, transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            outcome = new ExpiredOfferOutcome(item.ConversationRef, item.AmountDiamond);
        }, cancellationToken);

        return outcome;
    }

    private static bool IsEligibleForExpiredOfferRefund(ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Pending
               && item.OfferExpiresAt != null
               && item.OfferExpiresAt <= now;
    }

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

        ApplyExpiredOfferRefundState(item, DateTime.UtcNow);
        await dependencies.FinanceRepository.UpdateItemAsync(item, cancellationToken);
        await UpdateSessionFrozenBalanceAsync(dependencies.FinanceRepository, item, cancellationToken);
        await MarkSessionRefundedWhenFullyReleasedAsync(dependencies.FinanceRepository, item.FinanceSessionId, cancellationToken);
        await PublishExpiredOfferEventAsync(dependencies, item, cancellationToken);
    }

    private static void ApplyExpiredOfferRefundState(ChatQuestionItem item, DateTime now)
    {
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
        item.UpdatedAt = now;
    }

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
