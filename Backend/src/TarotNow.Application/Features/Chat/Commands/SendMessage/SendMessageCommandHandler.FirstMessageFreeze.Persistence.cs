using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private async Task AddPendingMainQuestionAsync(
        Guid sessionId,
        string conversationId,
        MainQuestionFreezeContext context,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        await _financeRepo.AddItemAsync(new Domain.Entities.ChatQuestionItem
        {
            FinanceSessionId = sessionId,
            ConversationRef = conversationId,
            PayerId = context.UserId,
            ReceiverId = context.ReaderId,
            Type = QuestionItemType.MainQuestion,
            AmountDiamond = context.AmountDiamond,
            Status = QuestionItemStatus.Pending,
            OfferExpiresAt = context.OfferExpiresAtUtc,
            IdempotencyKey = context.IdempotencyKey,
            CreatedAt = now,
            UpdatedAt = now
        }, cancellationToken);
    }

    private async Task UpdatePendingSessionFrozenAmountAsync(
        Domain.Entities.ChatFinanceSession session,
        long amountDiamond,
        CancellationToken cancellationToken)
    {
        session.TotalFrozen += amountDiamond;
        session.Status = "pending";
        session.UpdatedAt = DateTime.UtcNow;

        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
        await _financeRepo.SaveChangesAsync(cancellationToken);
    }
}
