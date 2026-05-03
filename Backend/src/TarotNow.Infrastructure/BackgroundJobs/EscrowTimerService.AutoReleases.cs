using Microsoft.Extensions.Logging;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét và xử lý các item đủ điều kiện auto-release.
    /// Luồng xử lý: lấy candidates đã due, gom theo session, xử lý từng session trong transaction, bắt lỗi cục bộ để job không dừng.
    /// </summary>
    private async Task ProcessAutoReleases(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        CancellationToken cancellationToken)
    {
        var candidates = await dependencies.FinanceRepository.GetItemsForAutoReleaseAsync(cancellationToken);
        var candidateSessionIds = candidates
            .Select(candidate => candidate.FinanceSessionId)
            .Distinct()
            .ToList();

        foreach (var sessionId in candidateSessionIds)
        {
            try
            {
                await ProcessAutoReleaseSessionAsync(
                    dependencies,
                    escrowSettlementService,
                    sessionId,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-release failed: {SessionId}", sessionId);
                // Giữ tiến trình quét tiếp tục cho các candidate còn lại.
            }
        }
    }

    /// <summary>
    /// Xử lý auto-release ở cấp session và cập nhật conversation khi session đã hoàn tất.
    /// Luồng xử lý: lock session, xác thực toàn bộ accepted item đều đủ điều kiện, apply session release, rồi sync completed.
    /// </summary>
    private async Task ProcessAutoReleaseSessionAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await dependencies.FinanceRepository.GetSessionForUpdateAsync(sessionId, transactionCt);
            if (session == null)
            {
                // Session không còn tồn tại thì bỏ qua.
                return;
            }

            var items = await dependencies.FinanceRepository.GetItemsBySessionIdAsync(session.Id, transactionCt);
            var acceptedItems = items.Where(item => item.Status == QuestionItemStatus.Accepted).ToList();
            if (acceptedItems.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            if (acceptedItems.All(item => IsEligibleForAutoRelease(item, now)) == false)
            {
                // Chỉ auto-release khi toàn bộ accepted item trong session đã đủ điều kiện theo SLA.
                return;
            }

            var summary = await escrowSettlementService.ApplySessionReleaseAsync(
                session,
                items,
                isAutoRelease: true,
                transactionCt);
            if (summary == null)
            {
                return;
            }

            _logger.LogInformation(
                "[EscrowTimer] Auto-release session: {SessionId}, Released={ReleasedAmount}💎, ReleasedItemCount={ReleasedItemCount}",
                summary.FinanceSessionId,
                summary.ReleasedAmountDiamond,
                summary.ReleasedItemCount);

            if (session.TotalFrozen <= 0
                && string.IsNullOrWhiteSpace(session.ConversationRef) == false
                && summary.ReleasedAmountDiamond > 0)
            {
                await PublishConversationSyncRequestedAsync(
                    dependencies,
                    new EscrowConversationSyncRequestedDomainEvent
                    {
                        ConversationId = session.ConversationRef,
                        TargetStatus = ConversationStatus.Completed,
                        MessageType = ChatMessageType.SystemRelease,
                        ActorId = session.ReaderId.ToString("D"),
                        MessageContent = $"Hệ thống đã tự động giải ngân {summary.ReleasedAmountDiamond} 💎 cho Reader theo timeout.",
                        SyncReason = "auto_release",
                        ResolvedAtUtc = now,
                        OccurredAtUtc = now
                    },
                    transactionCt);
                // Publish trong transaction để outbox và settlement commit atomically.
            }

            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra item có đủ điều kiện auto-release theo SLA hay không.
    /// Luồng xử lý: yêu cầu Accepted, đã có phản hồi reader, có AutoReleaseAt và đã tới hạn.
    /// </summary>
    private static bool IsEligibleForAutoRelease(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt != null
               && item.AutoReleaseAt != null
               && item.AutoReleaseAt <= now;
    }
}
