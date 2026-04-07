

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents;

public class EntitlementConsumedEventHandler : INotificationHandler<EntitlementConsumedNotification>
{
    private readonly ICacheService _cacheService;

    public EntitlementConsumedEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(EntitlementConsumedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        
        var cacheKey = $"entitlement_balance:{ev.UserId}";
        await _cacheService.RemoveAsync(cacheKey);
    }
}
