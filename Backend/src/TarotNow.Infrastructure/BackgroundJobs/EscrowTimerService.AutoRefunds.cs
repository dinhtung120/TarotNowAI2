using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessAutoRefunds(
        RefundDependencies dependencies,
        CancellationToken cancellationToken)
    {
        var candidates = await dependencies.FinanceRepository.GetItemsForAutoRefundAsync(cancellationToken);
        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoRefundCandidateAsync(
                    dependencies,
                    candidate.Id,
                    cancellationToken);

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
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = await ExecuteAutoRefundTransactionAsync(dependencies, candidateId, cancellationToken);
        if (string.IsNullOrWhiteSpace(outcome.ConversationId) == false && outcome.RefundedAmount > 0)
        {
            await MarkConversationExpiredAsync(
                dependencies,
                outcome.ConversationId,
                $"Reader không phản hồi trong SLA đã cam kết. Đã hoàn {outcome.RefundedAmount} 💎.",
                cancellationToken);
        }
    }
}
