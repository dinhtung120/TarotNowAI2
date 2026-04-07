

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents;

public class SubscriptionActivatedEventHandler : INotificationHandler<SubscriptionActivatedNotification>
{
    private readonly ICacheService _cacheService;
    

    public SubscriptionActivatedEventHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(SubscriptionActivatedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        
        var cacheKey = $"entitlement_balance:{ev.UserId}";
        await _cacheService.RemoveAsync(cacheKey);

        
        
    }
}
