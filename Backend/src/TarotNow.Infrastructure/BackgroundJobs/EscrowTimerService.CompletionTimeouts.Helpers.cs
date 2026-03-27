using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private static bool BothSidesAlreadyConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }

    private async Task<ConversationDto?> TryGetDueCompletionTimeoutConversationAsync(
        RefundDependencies dependencies,
        string conversationId,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null || conversation.Status != ConversationStatus.Ongoing)
        {
            return null;
        }

        if (conversation.Confirm?.AutoResolveAt == null || conversation.Confirm.AutoResolveAt > DateTime.UtcNow)
        {
            return null;
        }

        return conversation;
    }

    private async Task AutoSettleCompletionTimeoutAsync(
        RefundDependencies dependencies,
        IEscrowSettlementService escrowSettlementService,
        string conversationId,
        CancellationToken cancellationToken)
    {
        await dependencies.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await dependencies.FinanceRepository.GetSessionByConversationRefAsync(conversationId, transactionCt);
            if (session == null)
            {
                return;
            }

            await AutoReleaseAcceptedItemsAsync(dependencies, escrowSettlementService, session.Id, transactionCt);
            await MarkCompletedIfNoFrozenAsync(dependencies, session.Id, transactionCt);
        }, cancellationToken);
    }

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
        }

        await dependencies.FinanceRepository.SaveChangesAsync(cancellationToken);
    }

    private static async Task MarkCompletedIfNoFrozenAsync(
        RefundDependencies dependencies,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var lockedSession = await dependencies.FinanceRepository.GetSessionForUpdateAsync(sessionId, cancellationToken);
        if (lockedSession == null)
        {
            return;
        }

        if (lockedSession.TotalFrozen <= 0)
        {
            lockedSession.Status = "completed";
        }

        lockedSession.UpdatedAt = DateTime.UtcNow;
        await dependencies.FinanceRepository.UpdateSessionAsync(lockedSession, cancellationToken);
        await dependencies.FinanceRepository.SaveChangesAsync(cancellationToken);
    }

    private static ChatMessageDto BuildCompletionTimeoutMessage(ConversationDto conversation)
    {
        return new ChatMessageDto
        {
            ConversationId = conversation.Id,
            SenderId = conversation.Confirm?.RequestedBy ?? conversation.ReaderId,
            Type = ChatMessageType.SystemRelease,
            Content = "Yêu cầu hoàn thành đã quá hạn phản hồi. Hệ thống tự động giải ngân cho Reader.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }
}
