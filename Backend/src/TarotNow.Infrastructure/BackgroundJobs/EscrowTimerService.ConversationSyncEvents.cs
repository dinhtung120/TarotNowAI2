using TarotNow.Application.Common;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private static Task PublishConversationSyncRequestedAsync(
        RefundDependencies dependencies,
        EscrowConversationSyncRequestedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        return dependencies.DomainEventPublisher.PublishAsync(domainEvent, cancellationToken);
    }
}
