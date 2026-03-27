using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessAutoReleases(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        CancellationToken cancellationToken)
    {
        var candidates = await dependencies.FinanceRepository.GetItemsForAutoReleaseAsync(cancellationToken);

        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoReleaseCandidateAsync(
                    dependencies,
                    escrowSettlementService,
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
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        string? completedConversationId = null;
        long releasedAmount = 0;

        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || !IsEligibleForAutoRelease(item, DateTime.UtcNow))
            {
                return;
            }

            await escrowSettlementService.ApplyReleaseAsync(
                item,
                isAutoRelease: true,
                cancellationToken: transactionCt);

            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}", item.Id);

            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(item.ConversationRef, transactionCt);
            if (session != null && session.TotalFrozen <= 0)
            {
                session.Status = "completed";
                session.UpdatedAt = DateTime.UtcNow;
                await dependencies.FinanceRepository.UpdateSessionAsync(session, transactionCt);
                await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
                completedConversationId = item.ConversationRef;
                releasedAmount = item.AmountDiamond;
            }
        }, cancellationToken);

        if (string.IsNullOrWhiteSpace(completedConversationId) == false && releasedAmount > 0)
        {
            await MarkConversationCompletedAsync(
                dependencies,
                completedConversationId,
                $"Hệ thống đã tự động giải ngân {releasedAmount} 💎 cho Reader theo timeout.",
                cancellationToken);
        }
    }

    private static bool IsEligibleForAutoRelease(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt != null
               && item.AutoReleaseAt != null
               && item.AutoReleaseAt <= now;
    }
}
