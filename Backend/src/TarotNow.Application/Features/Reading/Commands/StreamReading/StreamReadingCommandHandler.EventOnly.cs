using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
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

public sealed class StreamReadingCommandHandlerRequestedDomainEvent : IIdempotentDomainEvent
{
    public StreamReadingCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public string EventIdempotencyKey => CommandEventIdempotencyKey.Build(
        nameof(StreamReadingCommandHandlerRequestedDomainEvent),
        ResolveRawIdempotencyKey(Command));

    public StreamReadingCommandHandlerRequestedDomainEvent(StreamReadingCommand command)
    {
        Command = command;
    }

    private static string ResolveRawIdempotencyKey(StreamReadingCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey) == false)
        {
            return command.IdempotencyKey!;
        }

        var followup = string.IsNullOrWhiteSpace(command.FollowupQuestion)
            ? "none"
            : command.FollowupQuestion.Trim().ToLowerInvariant();
        return $"{command.UserId:N}:{command.ReadingSessionId.Trim().ToLowerInvariant()}:{followup}";
    }
}
