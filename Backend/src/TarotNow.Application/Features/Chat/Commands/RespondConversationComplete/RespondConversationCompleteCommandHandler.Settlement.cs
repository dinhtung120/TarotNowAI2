using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
{
    private async Task<ConversationCompleteRespondResult> CompleteConversationAsync(
        ResponseContext context,
        CancellationToken cancellationToken)
    {
        var completedAt = await CompleteAndSettleConversationAsync(
            context.Conversation,
            context.RequesterId,
            "Yêu cầu hoàn thành đã được chấp thuận. Hệ thống đã giải ngân cho Reader.",
            cancellationToken);

        return new ConversationCompleteRespondResult
        {
            Status = context.Conversation.Status,
            Accepted = true,
            Metadata = new Dictionary<string, object> { ["completedAt"] = completedAt }
        };
    }

    private async Task<DateTime> CompleteAndSettleConversationAsync(
        ConversationDto conversation,
        string actorId,
        string completionMessage,
        CancellationToken cancellationToken)
    {
        await _transactionCoordinator.ExecuteAsync(
            transactionCt => SettleConversationSessionAsync(conversation.Id, transactionCt),
            cancellationToken);

        var completedAt = await AddSystemMessageAsync(
            conversation,
            actorId,
            new SystemMessageSpec(ChatMessageType.SystemRelease, completionMessage, DateTime.UtcNow),
            cancellationToken);

        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.Confirm!.AutoResolveAt = null;
        conversation.UpdatedAt = completedAt;
        conversation.LastMessageAt = completedAt;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
        return completedAt;
    }

    private async Task SettleConversationSessionAsync(string conversationId, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionByConversationRefAsync(conversationId, cancellationToken);
        if (session == null)
        {
            return;
        }

        var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, cancellationToken);
        foreach (var item in items.Where(item => item.Status == QuestionItemStatus.Accepted))
        {
            await _escrowSettlementService.ApplyReleaseAsync(item, isAutoRelease: false, cancellationToken);
        }

        await _financeRepository.SaveChangesAsync(cancellationToken);
        await MarkCompletedSessionWhenNoFrozenAsync(session.Id, cancellationToken);
    }

    private async Task MarkCompletedSessionWhenNoFrozenAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var lockedSession = await _financeRepository.GetSessionForUpdateAsync(sessionId, cancellationToken);
        if (lockedSession == null)
        {
            return;
        }

        if (lockedSession.TotalFrozen <= 0)
        {
            lockedSession.Status = "completed";
        }

        lockedSession.UpdatedAt = DateTime.UtcNow;
        await _financeRepository.UpdateSessionAsync(lockedSession, cancellationToken);
        await _financeRepository.SaveChangesAsync(cancellationToken);
    }
}
