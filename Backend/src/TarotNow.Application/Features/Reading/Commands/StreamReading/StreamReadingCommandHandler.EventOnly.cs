using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public sealed class StreamReadingCommandHandler : IRequestHandler<StreamReadingCommand, StreamReadingResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public StreamReadingCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<StreamReadingResult> Handle(StreamReadingCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new StreamReadingCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (StreamReadingResult)domainEvent.Result;
    }
}

public sealed class StreamReadingCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public StreamReadingCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public StreamReadingCommandHandlerRequestedDomainEvent(StreamReadingCommand command)
    {
        Command = command;
    }
}
