

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public CreateSubscriptionPlanCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Guid> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        
        var plan = new SubscriptionPlan(
            name: request.Name,
            description: request.Description,
            priceDiamond: request.PriceDiamond,
            durationDays: request.DurationDays,
            entitlementsJson: request.EntitlementsJson, 
            displayOrder: request.DisplayOrder
        );
        
        
        await _subscriptionRepository.AddPlanAsync(plan, cancellationToken);
        await _subscriptionRepository.SaveChangesAsync(cancellationToken);
        
        return plan.Id;
    }
}
