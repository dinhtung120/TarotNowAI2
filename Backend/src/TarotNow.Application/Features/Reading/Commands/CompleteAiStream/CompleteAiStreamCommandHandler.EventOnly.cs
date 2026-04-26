using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public sealed class CompleteAiStreamCommandHandler : IRequestHandler<CompleteAiStreamCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public CompleteAiStreamCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(CompleteAiStreamCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new CompleteAiStreamCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class CompleteAiStreamCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public CompleteAiStreamCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(CompleteAiStreamCommandHandlerRequestedDomainEvent),
        Command.AiRequestId.ToString("N"));

    public CompleteAiStreamCommandHandlerRequestedDomainEvent(CompleteAiStreamCommand command)
    {
        Command = command;
    }
}
