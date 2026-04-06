using MediatR;
using System;
using System.Collections.Generic;
using TarotNow.Application.Features.Subscription.Queries.Dtos;

namespace TarotNow.Application.Features.Subscription.Queries.GetSubscriptionPlans;

public record GetSubscriptionPlansQuery : IRequest<List<SubscriptionPlanDto>>;
