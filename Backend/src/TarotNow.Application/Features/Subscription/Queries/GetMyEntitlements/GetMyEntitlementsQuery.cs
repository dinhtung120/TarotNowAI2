using MediatR;
using System;
using System.Collections.Generic;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Subscription.Queries.GetMyEntitlements;

public record GetMyEntitlementsQuery(Guid UserId) : IRequest<List<EntitlementBalanceDto>>;
