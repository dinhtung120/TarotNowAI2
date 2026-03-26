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

        if (!isAutoRelease)
        {
            item.ConfirmedAt = now;
        }
    }

    private Task PublishReleaseEventAsync(
        ChatQuestionItem item,
        long readerAmount,
        long feeAmount,
        bool isAutoRelease,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(new EscrowReleasedDomainEvent
        {
            ItemId = item.Id,
            PayerId = item.PayerId,
            ReceiverId = item.ReceiverId,
            GrossAmountDiamond = item.AmountDiamond,
            ReleasedAmountDiamond = readerAmount,
            FeeAmountDiamond = feeAmount,
            IsAutoRelease = isAutoRelease
        }, cancellationToken);
    }
}
