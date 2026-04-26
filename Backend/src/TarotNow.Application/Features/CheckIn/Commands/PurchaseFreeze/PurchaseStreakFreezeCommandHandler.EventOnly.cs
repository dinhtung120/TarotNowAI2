using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

public sealed class PurchaseStreakFreezeCommandHandler : IRequestHandler<PurchaseStreakFreezeCommand, PurchaseStreakFreezeResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PurchaseStreakFreezeCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<PurchaseStreakFreezeResult> Handle(PurchaseStreakFreezeCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PurchaseStreakFreezeCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (PurchaseStreakFreezeResult)domainEvent.Result;
    }
}

public sealed class PurchaseStreakFreezeCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public PurchaseStreakFreezeCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(PurchaseStreakFreezeCommandHandlerRequestedDomainEvent),
        Command.IdempotencyKey);

    public PurchaseStreakFreezeCommandHandlerRequestedDomainEvent(PurchaseStreakFreezeCommand command)
    {
        Command = command;
    }
}
