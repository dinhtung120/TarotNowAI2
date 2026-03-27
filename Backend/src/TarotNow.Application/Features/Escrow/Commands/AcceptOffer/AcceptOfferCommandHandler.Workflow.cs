using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AcceptOffer;

public partial class AcceptOfferCommandHandler
{
    private async Task<Guid> ExecuteAcceptOfferAsync(
        AcceptOfferCommand request,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        Guid createdItemId = Guid.Empty;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await GetOrCreateSessionAsync(request, transactionCt);
            await FreezeWalletAsync(request, idempotencyKey, transactionCt);

            var item = BuildQuestionItem(request, session.Id, idempotencyKey);
            await _financeRepo.AddItemAsync(item, transactionCt);

            session.TotalFrozen += request.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);
            createdItemId = item.Id;
        }, cancellationToken);

        return createdItemId;
    }

    private async Task<ChatFinanceSession> GetOrCreateSessionAsync(
        AcceptOfferCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(request.ConversationRef, cancellationToken);
        if (session != null)
        {
            return session;
        }

        session = new ChatFinanceSession
        {
            ConversationRef = request.ConversationRef,
            UserId = request.UserId,
            ReaderId = request.ReaderId,
            Status = "active",
            TotalFrozen = 0
        };

        await _financeRepo.AddSessionAsync(session, cancellationToken);
        return session;
    }

    private async Task FreezeWalletAsync(
        AcceptOfferCommand request,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepo.FreezeAsync(
            request.UserId,
            request.AmountDiamond,
            referenceSource: "chat_question_item",
            referenceId: idempotencyKey,
            description: $"Escrow freeze {request.AmountDiamond}💎 cho conversation {request.ConversationRef}",
            idempotencyKey: $"freeze_{idempotencyKey}",
            cancellationToken: cancellationToken);
    }

    private static ChatQuestionItem BuildQuestionItem(
        AcceptOfferCommand request,
        Guid financeSessionId,
        string idempotencyKey)
    {
        var now = DateTime.UtcNow;

        return new ChatQuestionItem
        {
            FinanceSessionId = financeSessionId,
            ConversationRef = request.ConversationRef,
            PayerId = request.UserId,
            ReceiverId = request.ReaderId,
            Type = QuestionItemType.MainQuestion,
            AmountDiamond = request.AmountDiamond,
            Status = QuestionItemStatus.Accepted,
            ProposalMessageRef = request.ProposalMessageRef,
            AcceptedAt = now,
            ReaderResponseDueAt = now.AddHours(24),
            AutoRefundAt = now.AddHours(24),
            IdempotencyKey = idempotencyKey
        };
    }
}
