using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private readonly record struct AutoRefundOutcome(string? ConversationId, long RefundedAmount);

    private async Task<AutoRefundOutcome> ExecuteAutoRefundTransactionAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = new AutoRefundOutcome(null, 0);

        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || IsEligibleForAutoRefund(item, DateTime.UtcNow) == false)
            {
                return;
            }

            await dependencies.WalletRepository.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: transactionCt);

            ApplyRefundState(item, DateTime.UtcNow);
            await dependencies.FinanceRepository.UpdateItemAsync(item, transactionCt);
            await UpdateSessionFrozenBalanceAsync(dependencies.FinanceRepository, item, transactionCt);
            await MarkSessionRefundedWhenFullyReleasedAsync(dependencies.FinanceRepository, item.FinanceSessionId, transactionCt);
            await PublishAutoRefundEventAsync(dependencies, item, transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);

            outcome = new AutoRefundOutcome(item.ConversationRef, item.AmountDiamond);
        }, cancellationToken);

        return outcome;
    }

    private static bool IsEligibleForAutoRefund(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt == null
               && item.AutoRefundAt != null
               && item.AutoRefundAt <= now;
    }

    private static void ApplyRefundState(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
    }

    private static Task PublishAutoRefundEventAsync(
        RefundDependencies dependencies,
        Domain.Entities.ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        return dependencies.DomainEventPublisher.PublishAsync(new EscrowRefundedDomainEvent
        {
            ItemId = item.Id,
            UserId = item.PayerId,
            AmountDiamond = item.AmountDiamond,
            RefundSource = "reader_sla_timeout"
        }, cancellationToken);
    }
}
