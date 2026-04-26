using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Admin.Commands.ResolveDispute;

public partial class ResolveDisputeCommandExecutor
{
    private Task PublishMoneyChangedAsync(
        Guid userId,
        long deltaAmount,
        string changeType,
        Guid referenceId,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = userId,
                Currency = CurrencyType.Diamond,
                ChangeType = changeType,
                DeltaAmount = deltaAmount,
                ReferenceId = referenceId.ToString()
            },
            cancellationToken);
    }
}
