

using System;
using MediatR;

namespace TarotNow.Application.Features.Subscription.Commands.CreateSubscriptionPlan;

public record CreateSubscriptionPlanCommand(
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    string EntitlementsJson,
    int DisplayOrder
) : IRequest<Guid>;
