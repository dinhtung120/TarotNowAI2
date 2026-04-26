using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    /// <summary>
    /// Kiểm tra cả hai phía đã xác nhận hoàn tất conversation hay chưa.
    /// Luồng xử lý: xác thực cả mốc UserAt và ReaderAt đều khác null.
    /// </summary>
    private static bool BothSidesAlreadyConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }

    /// <summary>
    /// Lấy conversation tới hạn auto-resolve completion timeout nếu còn hợp lệ xử lý.
    /// Luồng xử lý: lấy theo id, kiểm tra status ongoing và AutoResolveAt đã tới hạn.
    /// </summary>
    private async Task<ConversationDto?> TryGetDueCompletionTimeoutConversationAsync(
        RefundDependencies dependencies,
        string conversationId,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Ongoing)
        {
            // Conversation không tồn tại hoặc không còn ongoing thì không xử lý timeout completion.
            return null;
        }

        if (conversation.Confirm?.AutoResolveAt == null || conversation.Confirm.AutoResolveAt > DateTime.UtcNow)
        {
            // Chưa tới hạn auto-resolve nên bỏ qua ở vòng quét hiện tại.
            return null;
        }

        return conversation;
    }

    /// <summary>
    /// Tự động settle các item accepted của conversation khi completion timeout.
    /// Luồng xử lý: mở transaction, lấy finance session, auto-release item accepted và chốt session nếu hết frozen.
    /// </summary>
    private async Task AutoSettleCompletionTimeoutAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(conversation.Id, transactionCt);
            if (session == null)
            {
                // Edge case: không có finance session thì không thể auto-settle.
                return;
            }

            await AutoReleaseAcceptedItemsAsync(dependencies, escrowSettlementService, session.Id, transactionCt);
            await MarkCompletedIfNoFrozenAsync(dependencies, session.Id, transactionCt);
            await dependencies.DomainEventPublisher.PublishAsync(
                BuildCompletionTimeoutSyncEvent(conversation),
                transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);
    }

    /// <summary>
    /// Auto-release toàn bộ item accepted trong session.
    /// Luồng xử lý: đọc item theo sessionId, lọc accepted, gọi escrowSettlementService cho từng item rồi save changes.
    /// </summary>
    private static async Task AutoReleaseAcceptedItemsAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var items = await dependencies.FinanceRepository.GetItemsBySessionIdAsync(sessionId, cancellationToken);
        foreach (var item in items.Where(item => item.Status == QuestionItemStatus.Accepted))
        {
            await escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: true, cancellationToken);
            // Chỉ release item Accepted để tránh xử lý nhầm item đã refunded/released trước đó.
        }
    }

    /// <summary>
    /// Đánh dấu session completed khi không còn số dư frozen.
    /// Luồng xử lý: lock session for update, set status nếu TotalFrozen <= 0, cập nhật timestamp và save changes.
    /// </summary>
    private static async Task MarkCompletedIfNoFrozenAsync(
        RefundDependencies dependencies,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var lockedSession = await dependencies.FinanceRepository.GetSessionForUpdateAsync(sessionId, cancellationToken);
        if (lockedSession == null)
        {
            // Session có thể đã bị xóa hoặc không còn tồn tại ở thời điểm xử lý.
            await dependencies.FinanceRepository.SaveChangesAsync(cancellationToken);
            return;
        }

        if (lockedSession.TotalFrozen <= 0)
        {
            lockedSession.Status = ChatFinanceSessionStatus.Completed;
            // Chỉ chốt completed khi chắc chắn không còn tiền bị giữ trong session.
        }

        lockedSession.UpdatedAt = DateTime.UtcNow;
        await dependencies.FinanceRepository.UpdateSessionAsync(lockedSession, cancellationToken);
    }

    private static CompletionTimeoutConversationSyncRequestedDomainEvent BuildCompletionTimeoutSyncEvent(
        ConversationDto conversation)
    {
        const string message = "Yêu cầu hoàn thành đã quá hạn phản hồi. Hệ thống tự động giải ngân cho Reader.";
        var actorId = conversation.Confirm?.RequestedBy ?? conversation.ReaderId;
        return new CompletionTimeoutConversationSyncRequestedDomainEvent(
            conversation.Id,
            actorId,
            message,
            DateTime.UtcNow);
    }
}
