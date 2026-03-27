using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private static async Task MarkSessionRefundedWhenFullyReleasedAsync(
        IChatFinanceRepository financeRepository,
        Guid financeSessionId,
        CancellationToken cancellationToken)
    {
        var session = await financeRepository.GetSessionForUpdateAsync(financeSessionId, cancellationToken);
        if (session == null || session.TotalFrozen > 0)
        {
            return;
        }

        session.Status = "refunded";
        session.UpdatedAt = DateTime.UtcNow;
        await financeRepository.UpdateSessionAsync(session, cancellationToken);
    }

    private static async Task MarkConversationExpiredAsync(
        RefundDependencies dependencies,
        string conversationId,
        string systemMessage,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null)
        {
            return;
        }

        if (ConversationStatus.IsTerminal(conversation.Status) || conversation.Status == ConversationStatus.Disputed)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var message = new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.SystemRefund,
            Content = systemMessage,
            IsRead = false,
            CreatedAt = now
        };
        await dependencies.MessageRepository.AddAsync(message, cancellationToken);

        conversation.Status = ConversationStatus.Expired;
        conversation.OfferExpiresAt = null;
        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }

    private static async Task MarkConversationCompletedAsync(
        RefundDependencies dependencies,
        string conversationId,
        string systemMessage,
        CancellationToken cancellationToken)
    {
        var conversation = await dependencies.ConversationRepository.GetByIdAsync(conversationId, cancellationToken);
        if (conversation == null)
        {
            return;
        }

        if (conversation.Status is not (ConversationStatus.Ongoing or ConversationStatus.AwaitingAcceptance or ConversationStatus.Disputed))
        {
            return;
        }

        var now = DateTime.UtcNow;
        var message = new ChatMessageDto
        {
            ConversationId = conversationId,
            SenderId = conversation.ReaderId,
            Type = ChatMessageType.SystemRelease,
            Content = systemMessage,
            IsRead = false,
            CreatedAt = now
        };
        await dependencies.MessageRepository.AddAsync(message, cancellationToken);

        conversation.Status = ConversationStatus.Completed;
        conversation.OfferExpiresAt = null;
        conversation.LastMessageAt = now;
        conversation.UpdatedAt = now;
        await dependencies.ConversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
