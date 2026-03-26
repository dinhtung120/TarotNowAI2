using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessAutoReleases(
        IChatFinanceRepository repo,
        IEscrowSettlementService escrowSettlementService,
        ITransactionCoordinator transactionCoordinator,
        CancellationToken cancellationToken)
    {
        var candidates = await repo.GetItemsForAutoReleaseAsync(cancellationToken);

        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoReleaseCandidateAsync(
                    repo,
                    escrowSettlementService,
                    transactionCoordinator,
                    candidate.Id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-release failed: {ItemId}", candidate.Id);
            }
        }
    }

    private async Task ProcessAutoReleaseCandidateAsync(
        IChatFinanceRepository repo,
        IEscrowSettlementService escrowSettlementService,
        ITransactionCoordinator transactionCoordinator,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        await transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await repo.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || !IsEligibleForAutoRelease(item, DateTime.UtcNow))
            {
                return;
            }

            await escrowSettlementService.ApplyReleaseAsync(
                item,
                isAutoRelease: true,
                cancellationToken: transactionCt);

            await repo.SaveChangesAsync(transactionCt);
            _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}", item.Id);
        }, cancellationToken);
    }

    private static bool IsEligibleForAutoRelease(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt != null
               && item.AutoReleaseAt != null
               && item.AutoReleaseAt <= now;
    }
}
