using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Legal.Commands.RecordConsent;

public sealed class RecordConsentCommandHandler : IRequestHandler<RecordConsentCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public RecordConsentCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(RecordConsentCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new RecordConsentCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class RecordConsentCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public RecordConsentCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public RecordConsentCommandHandlerRequestedDomainEvent(RecordConsentCommand command)
    {
        Command = command;
    }
}
