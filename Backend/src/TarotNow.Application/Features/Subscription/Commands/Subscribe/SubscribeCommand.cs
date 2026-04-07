

using System;
using MediatR;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public record SubscribeCommand(Guid UserId, Guid PlanId, string IdempotencyKey) : IRequest<Guid>;
