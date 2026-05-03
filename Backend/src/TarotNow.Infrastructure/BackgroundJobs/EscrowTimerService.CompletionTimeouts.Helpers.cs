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
    /// Luồng xử lý: mở transaction, lấy finance session, giải ngân gộp theo session, rồi phát sync event completed.
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

            var items = await dependencies.FinanceRepository.GetItemsBySessionIdAsync(session.Id, transactionCt);
            await escrowSettlementService.ApplySessionReleaseAsync(
                session,
                items,
                isAutoRelease: true,
                transactionCt);

            await dependencies.DomainEventPublisher.PublishAsync(
                BuildCompletionTimeoutSyncEvent(conversation),
                transactionCt);
            await dependencies.FinanceRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);
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
