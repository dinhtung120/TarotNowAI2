using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Services;

public sealed partial class EscrowSettlementService
{
    private static (string ReleaseDescription, string FeeDescription) BuildDescriptions(
        bool isAutoRelease,
        long readerAmount,
        long fee)
    {
        if (isAutoRelease)
        {
            return ($"Auto-release {readerAmount}💎 (fee {fee}💎)", $"Platform fee auto 10% = {fee}💎");
        }

        return ($"Release {readerAmount}💎 (fee {fee}💎) cho reader", $"Platform fee 10% = {fee}💎");
    }

    private static void ApplyReleasedState(ChatQuestionItem item, bool isAutoRelease)
    {
        var now = DateTime.UtcNow;
        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
        item.AutoReleaseAt = null;

        if (isAutoRelease == false)
        {
            item.ConfirmedAt = now;
        }
    }

    private async Task DecreaseSessionFrozenAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            return;
        }

        session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
        await _financeRepository.UpdateSessionAsync(session, cancellationToken);
    }

    private Task PublishReleasedEventAsync(
        ChatQuestionItem item,
        long releasedAmountDiamond,
        long feeAmountDiamond,
        bool isAutoRelease,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(new EscrowReleasedDomainEvent
        {
            ItemId = item.Id,
            PayerId = item.PayerId,
            ReceiverId = item.ReceiverId,
            GrossAmountDiamond = item.AmountDiamond,
            ReleasedAmountDiamond = releasedAmountDiamond,
            FeeAmountDiamond = feeAmountDiamond,
            IsAutoRelease = isAutoRelease
        }, cancellationToken);
    }
}
