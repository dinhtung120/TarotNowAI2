using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessAutoRefunds(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken cancellationToken)
    {
        var candidates = await repo.GetItemsForAutoRefundAsync(cancellationToken);
        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoRefundCandidateAsync(repo, wallet, transactionCoordinator, candidate.Id, cancellationToken);

                _logger.LogInformation(
                    "[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎",
                    candidate.Id,
                    candidate.AmountDiamond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", candidate.Id);
            }
        }
    }

    private async Task ProcessAutoRefundCandidateAsync(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        await transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await repo.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || !IsEligibleForAutoRefund(item, DateTime.UtcNow))
            {
                return;
            }

            await wallet.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: transactionCt);

            ApplyRefundState(item, DateTime.UtcNow);
            await repo.UpdateItemAsync(item, transactionCt);
            await UpdateSessionFrozenBalanceAsync(repo, item, transactionCt);
            await repo.SaveChangesAsync(transactionCt);
        }, cancellationToken);
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
}
