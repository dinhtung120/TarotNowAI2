using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Quét và xử lý các item đủ điều kiện auto-release.
    /// Luồng xử lý: lấy candidates, xử lý từng item trong transaction, bắt lỗi cục bộ để job không dừng.
    /// </summary>
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
                // Giữ tiến trình quét tiếp tục cho các candidate còn lại.
            }
        }
    }

    /// <summary>
    /// Xử lý một candidate auto-release và cập nhật conversation khi session đã hoàn tất.
    /// Luồng xử lý: lock item, kiểm tra eligibility, apply release, cập nhật session, rồi mark conversation completed.
    /// </summary>
    private async Task ProcessAutoReleaseCandidateAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await dependencies.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || !IsEligibleForAutoRelease(item, DateTime.UtcNow))
            {
                // Item không hợp lệ hoặc đã được xử lý trước đó thì bỏ qua.
                return;
            }

            await escrowSettlementService.ApplyReleaseAsync(
                item,
                isAutoRelease: true,
                cancellationToken: transactionCt);
            // Chạy settlement chuẩn để release tiền và cập nhật state item liên quan.
            _logger.LogInformation("[EscrowTimer] Auto-release: {ItemId}", item.Id);

            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(item.ConversationRef, transactionCt);
            if (session != null && session.TotalFrozen <= 0)
            {
                session.Status = ChatFinanceSessionStatus.Completed;
                session.UpdatedAt = DateTime.UtcNow;
                await dependencies.FinanceRepository.UpdateSessionAsync(session, transactionCt);
                // Khi không còn frozen thì chốt session completed để đóng phiên tài chính.
                if (string.IsNullOrWhiteSpace(item.ConversationRef) == false && item.AmountDiamond > 0)
                {
                    var now = DateTime.UtcNow;
                    await PublishConversationSyncRequestedAsync(
                        dependencies,
                        new EscrowConversationSyncRequestedDomainEvent
                        {
                            ConversationId = item.ConversationRef,
                            TargetStatus = ConversationStatus.Completed,
                            MessageType = ChatMessageType.SystemRelease,
                            ActorId = item.ReceiverId.ToString("D"),
                            MessageContent = $"Hệ thống đã tự động giải ngân {item.AmountDiamond} 💎 cho Reader theo timeout.",
                            SyncReason = "auto_release",
                            ResolvedAtUtc = now,
                            OccurredAtUtc = now
                        },
                        transactionCt);
                    // Publish trong transaction để outbox và settlement commit atomically.
                }
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
