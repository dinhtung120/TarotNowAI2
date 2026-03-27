using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private async Task FreezeMainQuestionAsync(
        ConversationDto conversation,
        MainQuestionFreezeContext context,
        CancellationToken cancellationToken)
    {
        if (await _financeRepo.GetItemByIdempotencyKeyAsync(context.IdempotencyKey, cancellationToken) != null)
        {
            return;
        }

        var session = await GetOrCreatePendingSessionAsync(
            conversation,
            context.UserId,
            context.ReaderId,
            cancellationToken);
        await FreezeWalletAsync(
            conversation.Id,
            context.UserId,
            context.AmountDiamond,
            context.IdempotencyKey,
            cancellationToken);
        await AddPendingMainQuestionAsync(session.Id, conversation.Id, context, cancellationToken);
        await UpdatePendingSessionFrozenAmountAsync(session, context.AmountDiamond, cancellationToken);
    }

    private async Task<Domain.Entities.ChatFinanceSession> GetOrCreatePendingSessionAsync(
        ConversationDto conversation,
        Guid userId,
        Guid readerId,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(conversation.Id, cancellationToken);
        if (session != null)
        {
            return session;
        }

        session = new Domain.Entities.ChatFinanceSession
        {
            ConversationRef = conversation.Id,
            UserId = userId,
            ReaderId = readerId,
            Status = "pending",
            TotalFrozen = 0
        };

        await _financeRepo.AddSessionAsync(session, cancellationToken);
        return session;
    }

    private Task FreezeWalletAsync(
        string conversationId,
        Guid userId,
        long amountDiamond,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        return _walletRepo.FreezeAsync(
            userId,
            amountDiamond,
            referenceSource: "chat_question_item",
            referenceId: idempotencyKey,
            description: $"Escrow freeze {amountDiamond}💎 cho conversation {conversationId}",
            idempotencyKey: $"freeze_{idempotencyKey}",
            cancellationToken: cancellationToken);
    }
}
