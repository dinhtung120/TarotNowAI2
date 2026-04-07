

using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Subscription.Queries.Dtos;

public record SubscriptionPlanDto(
    Guid Id,
    string Name,
    string? Description,
    long PriceDiamond,
    int DurationDays,
    bool IsActive,
    Dictionary<string, int> Entitlements,
    DateTime CreatedAt
);
