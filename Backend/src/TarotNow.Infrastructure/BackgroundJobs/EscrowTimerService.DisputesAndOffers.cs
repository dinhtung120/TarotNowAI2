using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét và xử lý auto-resolution cho dispute đã quá hạn 48 giờ.
    /// Luồng xử lý: lấy candidate disputed tới hạn, xử lý tuần tự từng item và bắt lỗi cục bộ.
    /// </summary>
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
                // Giữ job chạy tiếp để không bỏ sót candidate khác.
            }
        }
    }

    /// <summary>
    /// Xử lý một candidate dispute auto-resolution.
    /// Luồng xử lý: lock item, xác nhận điều kiện disputed quá hạn, apply release, cập nhật session, rồi mark conversation completed.
    /// </summary>
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
                // Candidate không còn disputed hợp lệ hoặc chưa đủ thời gian quá hạn.
                return;
            }

            await escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: true, cancellationToken: transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
            // Chốt release cho disputed item theo chính sách auto-resolve quá hạn.

            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(item.ConversationRef, transactionCt);
            if (session != null)
            {
                if (session.TotalFrozen <= 0)
                {
                    session.Status = "completed";
                    completedConversationId = session.ConversationRef;
                    // Không còn frozen thì conversation có thể chốt completed.
                }
                else
                {
                    session.Status = "active";
                    // Còn frozen thì giữ session active để chờ xử lý item khác.
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
            // Đồng bộ trạng thái conversation sau khi auto-resolve dispute thành công.
        }
    }
}
