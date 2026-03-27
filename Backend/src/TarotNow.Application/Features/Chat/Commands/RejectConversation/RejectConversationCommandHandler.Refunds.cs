using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public partial class RejectConversationCommandHandler
{
    private async Task<long> RefundIfNeededAsync(
        ConversationDto conversation,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (conversation.Status != ConversationStatus.AwaitingAcceptance)
        {
            return 0;
        }

        var refundedAmount = 0L;
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, transactionCt);
            if (session == null)
            {
                return;
            }

            var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, transactionCt);
            var refundableItems = items.Where(IsRefundableBeforeReject).ToList();
            refundedAmount = await RefundItemsAsync(refundableItems, now, transactionCt);

            session.TotalFrozen = Math.Max(0, session.TotalFrozen - refundedAmount);
            if (session.TotalFrozen == 0)
            {
                session.Status = "refunded";
            }

            session.UpdatedAt = now;
            await _financeRepository.UpdateSessionAsync(session, transactionCt);
            await _financeRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return refundedAmount;
    }

    private async Task<long> RefundItemsAsync(
        IEnumerable<Domain.Entities.ChatQuestionItem> items,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var total = 0L;
        foreach (var item in items)
        {
            await _walletRepository.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Reader reject refund {item.AmountDiamond}💎",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: cancellationToken);

            item.Status = QuestionItemStatus.Refunded;
            item.RefundedAt = now;
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.AddHours(24);
            item.UpdatedAt = now;
            total += item.AmountDiamond;
            await _financeRepository.UpdateItemAsync(item, cancellationToken);
        }

        return total;
    }
}
