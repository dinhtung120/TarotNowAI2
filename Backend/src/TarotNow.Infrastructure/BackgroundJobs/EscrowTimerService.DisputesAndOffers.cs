using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessDisputeAutoResolutions(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        CancellationToken cancellationToken)
    {
        var dueAtUtc = DateTime.UtcNow.AddHours(-48);
        var candidates = await dependencies.FinanceRepository.GetDisputedItemsForAutoResolveAsync(dueAtUtc, cancellationToken);

        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessDisputeAutoResolutionCandidateAsync(
                    dependencies,
                    escrowSettlementService,
                    candidate.Id,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Dispute auto-resolution failed: {ItemId}", candidate.Id);
            }
        }
    }

    private async Task ProcessDisputeAutoResolutionCandidateAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        string? completedConversationId = null;

        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null
                || item.Status != QuestionItemStatus.Disputed
                || (item.UpdatedAt ?? item.CreatedAt) > DateTime.UtcNow.AddHours(-48))
            {
                return;
            }

            await escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: true, cancellationToken: transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);

            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(item.ConversationRef, transactionCt);
            if (session != null)
            {
                if (session.TotalFrozen <= 0)
                {
                    session.Status = "completed";
                    completedConversationId = session.ConversationRef;
                }
                else
                {
                    session.Status = "active";
                }

                session.UpdatedAt = DateTime.UtcNow;
                await dependencies.FinanceRepository.UpdateSessionAsync(session, transactionCt);
                await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            }
        }, cancellationToken);

        if (string.IsNullOrWhiteSpace(completedConversationId) == false)
        {
            await MarkConversationCompletedAsync(
                dependencies,
                completedConversationId,
                "Dispute đã quá hạn xử lý 48 giờ. Hệ thống tự động giải ngân cho Reader.",
                cancellationToken);
        }
    }

}
