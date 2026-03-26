using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandHandler
{
    private Task PublishReleaseDomainEventAsync(
        ChatQuestionItem item,
        long readerAmount,
        long feeAmount,
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
            IsAutoRelease = false
        }, cancellationToken);
    }

    private Task PublishRefundDomainEventAsync(
        ChatQuestionItem item,
        string refundSource,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(new EscrowRefundedDomainEvent
        {
            ItemId = item.Id,
            UserId = item.PayerId,
            AmountDiamond = item.AmountDiamond,
            RefundSource = refundSource
        }, cancellationToken);
    }
}
