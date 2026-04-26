using System.Linq;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Chat.Commands.RejectConversation;

public partial class RejectConversationCommandHandler
{
    /// <summary>
    /// Hoàn tiền các item đủ điều kiện khi reader reject conversation awaiting acceptance.
    /// Luồng xử lý: chỉ xử lý trạng thái awaiting acceptance, chạy transaction lấy session/items, hoàn tiền item hợp lệ và cập nhật session.
    /// </summary>
    private async Task<long> RefundIfNeededAsync(
        ConversationDto conversation,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (conversation.Status != ConversationStatus.AwaitingAcceptance)
        {
            // Conversation chưa vào awaiting acceptance thì không có luồng refund này.
            return 0;
        }

        var refundedAmount = 0L;
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, transactionCt);
            if (session == null)
            {
                // Edge case không có finance session: bỏ qua refund.
                return;
            }

            var items = await _financeRepository.GetItemsBySessionIdAsync(session.Id, transactionCt);
            var refundableItems = items.Where(IsRefundableBeforeReject).ToList();
            // Hoàn tiền từng item và lấy tổng refund để cập nhật frozen balance.
            refundedAmount = await RefundItemsAsync(refundableItems, now, transactionCt);

            session.TotalFrozen = Math.Max(0, session.TotalFrozen - refundedAmount);
            if (session.TotalFrozen == 0)
            {
                // Khi không còn frozen thì đánh dấu session đã refunded toàn phần.
                session.Status = ChatFinanceSessionStatus.Refunded;
            }

            session.UpdatedAt = now;
            await _financeRepository.UpdateSessionAsync(session, transactionCt);
            await _financeRepository.SaveChangesAsync(transactionCt);
        }, cancellationToken);

        return refundedAmount;
    }

    /// <summary>
    /// Hoàn tiền cho danh sách item đủ điều kiện.
    /// Luồng xử lý: lặp từng item, gọi refund wallet, cập nhật trạng thái item và cộng dồn tổng tiền hoàn.
    /// </summary>
    private async Task<long> RefundItemsAsync(
        IEnumerable<Domain.Entities.ChatQuestionItem> items,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var total = 0L;
        var disputeWindowHours = _systemConfigSettings.EscrowDisputeWindowHours;
        foreach (var item in items)
        {
            // Ghi giao dịch refund idempotent theo item để tránh hoàn trùng.
            await _walletRepository.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Reader reject refund {item.AmountDiamond}💎",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: cancellationToken);
            await _domainEventPublisher.PublishAsync(
                new MoneyChangedDomainEvent
                {
                    UserId = item.PayerId,
                    Currency = CurrencyType.Diamond,
                    ChangeType = TransactionType.EscrowRefund,
                    DeltaAmount = item.AmountDiamond,
                    ReferenceId = item.Id.ToString()
                },
                cancellationToken);

            // Cập nhật state item sau khi refund thành công.
            item.Status = QuestionItemStatus.Refunded;
            item.RefundedAt = now;
            item.DisputeWindowStart = now;
            item.DisputeWindowEnd = now.AddHours(disputeWindowHours);
            item.UpdatedAt = now;
            total += item.AmountDiamond;
            await _financeRepository.UpdateItemAsync(item, cancellationToken);
        }

        return total;
    }
}
